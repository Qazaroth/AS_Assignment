using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AS_Assignment.Models;
using AS_Assignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
                    User u = getUser(email);

                    HttpContext.Session.SetInt32("SSID", u.id);
                    HttpContext.Session.SetString("SSEmail", u.email);

                    return RedirectToPagePermanent("/Index");
                }
                else
                {
                    error = "Invalid account details. Please try again!";
                    return Page();
                }
            }
            
            return Page();
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
