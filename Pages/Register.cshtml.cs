using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AS_Assignment.Models;
using AS_Assignment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AS_Assignment.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public User user { get; set; }

        [BindProperty]
        public String error { get; set; }

        [BindProperty]
        public Boolean hasError { get; set; }

        private UserService _ctx;

        public RegisterModel(UserService _ctx)
        {
            this._ctx = _ctx;
        }

        public IActionResult OnGet()
        {
            if (ModelState.IsValid && HttpContext.Session.GetString("SSEmail") != null)
            {
                return RedirectToPagePermanent("/Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            hasError = false;
            string pwd = user.password;
            string cc = user.creditCard;
            string cv = user.cvc;

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            byte[] saltByte = new byte[8];

            rng.GetBytes(saltByte);
            string salt = Convert.ToBase64String(saltByte);

            SHA512 hashing = SHA512.Create();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            string finalHash = Convert.ToBase64String(hashWithSalt);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();

            user.password = finalHash;
            user.salt = salt;
            user.creditCard = Convert.ToBase64String(Encoding.UTF8.GetBytes(cc));
            user.cvc = Convert.ToBase64String(Encoding.UTF8.GetBytes(cv));

            user.dateTimeRegistered = DateTime.Now;

            if (_ctx.addUser(user))
                return RedirectToPagePermanent("/Login");
            else
            {
                hasError = true;
                error = "An account with such data already exists! Try logging in <a href=\"/Login\">here</a>!";
                return Page();
            }
        }
    }
}
