using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Inventory.Pages
{
    [CookieAuth("AllowedAccessCookieValue")]
    public class WelcomeModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
