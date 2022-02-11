using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AS_Assignment.Pages.Accounts
{
    public class logoutModel : PageModel
    {
        private string email;
        private int id = -1;
        public IActionResult OnGet()
        {
            if (ModelState.IsValid)
            {
                email = HttpContext.Session.GetString("SSEmail");

                if (HttpContext.Session.GetInt32("SSID") != null)
                    id = (int)HttpContext.Session.GetInt32("SSID");

                if (email != null && id != -1)
                {
                    HttpContext.Session.Clear();
                }

                return RedirectToPage("/Index");
            }
            else
                return Page();
        }
    }
}
