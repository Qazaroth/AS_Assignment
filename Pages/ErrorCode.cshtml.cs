using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AS_Assignment.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorCodeModel : PageModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public bool isDev = false;
        public string errMessage { get; set; }
        public string error { get; set; }

        public void OnGet(string code)
        {
            error = code;
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var errHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (errHandlerPathFeature?.Error is FileNotFoundException)
            {
                errMessage = "The file was not found.";
            }

            if (errHandlerPathFeature?.Path == "/")
            {
                errMessage ??= string.Empty;
                errMessage += " Page: Home.";
            }
        }
    }
}
