using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AS_Assignment.Models;
using AS_Assignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AS_Assignment.Pages.Shared
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public String email { get; set; }
        [BindProperty]
        public String password { get; set; }
        [BindProperty]
        public String error { get; set; }
        [BindProperty]
        public object e { get; set; }

        private readonly UserService _ctx;

        public LoginModel(UserService _ctx)
        {
            this._ctx = _ctx;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            /**if (tries >= 3)
            {
                error = "Login temporarily disabled. Try again in 5 minutes...";
                return Page();
            }  **/
            User a = _ctx.getUserByEmail(email);

            if (a != null && a.lockedOut == 1 && a.lockEnd == DateTime.Now)
            {
                a.lockEnd = null;
                a.lockedOut = 0;
                a.tries = 0;
            }
            if (a != null && a.lockedOut == 1)
            {
                error = "Login temporarily disabled. Try again later...";
                return Page();
            }

            if (CaptchaPassed())
            {
                string p = this.password;
                string e = this.email;

                SHA512 hashing = SHA512.Create();
                string hash = getDBHash(email);
                string salt = getDBSalt(email);

                if (salt != null && salt.Length > 0 && hash != null && hash.Length > 0)
                {
                    string pwdWithSalt = p + salt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash.Equals(hash))
                    {
                        User u = a;
                        _ctx.updateUser(u);

                        HttpContext.Session.SetInt32("SSID", u.id);
                        HttpContext.Session.SetString("SSEmail", u.email);

                        return RedirectToPagePermanent("/Index");
                    }
                    else
                    {
                        if (a.tries >= 3)
                        {
                            a.lockEnd = DateTime.Now.AddMinutes(5);
                            a.lockedOut = 1;
                            error = "Maximum attempt reached, try again later!";
                        }
                        else
                        {
                            a.tries += 1;
                            error = "Invalid account details. Please try again!";
                        }
                        _ctx.updateUser(a);
                        return Page();
                    }
                }

                return Page();
            }
            else
                return Page();
        }

        public bool CaptchaPassed()
        {
            HttpClient c = new HttpClient();

            var res = c.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=secret&response={Request.Form["foo"]}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
                return false;

            string JsonRes = res.Content.ReadAsStringAsync().Result;
            dynamic Jsondata = JObject.Parse(JsonRes);

            if (Jsondata.success != "true" || Jsondata.score <= 0.5m)
                return false;

            return true;
        }
        protected User getUser(String email)
        {
            List<User> users = _ctx.getAllUsers();
            User u = null;

            foreach (User a in users)
            {
                if (a.email == email)
                {
                    u = a;
                    break;
                }
            }

            return u;
        }

        protected String getDBHash(String email)
        {
            string h = null;
            User u = null;
            List<User> users = _ctx.getAllUsers();

            foreach (User a in users)
            {
                if (a.email == email)
                {
                    u = a;
                    break;
                }
            }

            if (u != null) h = u.password;

            return h;
        }

        protected String getDBSalt(String email)
        {
            string h = null;
            User u = null;
            List<User> users = _ctx.getAllUsers();

            foreach (User a in users)
            {
                if (a.email == email)
                {
                    u = a;
                    break;
                }
            }

            if (u != null) h = u.salt;
  

            return h;
        }
    }
}
