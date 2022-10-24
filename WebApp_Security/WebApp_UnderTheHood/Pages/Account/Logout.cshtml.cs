using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_UnderTheHood.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            const string myCookieAuth = "MyCookieAuth";
            await HttpContext.SignOutAsync(myCookieAuth);

            return RedirectToPage("/Index");
        }
    }
}
