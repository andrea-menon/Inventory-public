using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Inventory.Pages
{
    public class AccediModel : PageModel
    {
        [BindProperty]
        public string AccessValue { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(AccessValue))
            {
                // Imposta il cookie
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(720), // Cookie valido per 12 ore
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("AccessLevel", AccessValue, cookieOptions);

                return RedirectToPage("/Welcome");
            }

            return Page();
        }
    }
}
