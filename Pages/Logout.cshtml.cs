using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AS_Assignment.Pages
{
    public class LogoutModel : PageModel
    {
        [BindProperty]
        public String email { get; set; }
        [BindProperty]
        public int id { get; set; }

        public IActionResult OnGet()
        {
            if (ModelState.IsValid && HttpContext.Session.GetInt32("SSID") != null)
                HttpContext.Session.Clear();
            return RedirectToPagePermanent("/Index");
        }
    }
}
