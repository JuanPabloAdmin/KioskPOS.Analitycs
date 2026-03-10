using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace KioskPos.Analytics.Web.Pages;

public class IndexModel : AnalyticsPageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
