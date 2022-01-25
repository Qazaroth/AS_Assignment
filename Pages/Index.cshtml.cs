using AS_Assignment.Models;
using AS_Assignment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_Assignment.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public User u { get; set; }

        private readonly ILogger<IndexModel> _logger;
        private readonly UserService _ctx;

        public IndexModel(ILogger<IndexModel> logger, UserService _ctx)
        {
            _logger = logger;
            this._ctx = _ctx;
        }

        public void OnGet()
        {
            if (ModelState.IsValid && HttpContext.Session.GetString("SSEmail") != null)
            {
                string ue = HttpContext.Session.GetString("SSEmail");
                u = getUser(ue);
            }
            else
            {
                u = null;
            }
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
    }
}
