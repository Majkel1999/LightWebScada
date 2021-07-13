using Microsoft.AspNetCore.Antiforgery;

namespace FrontEnd.Areas.Identity.Data
{
    public class TokenProvider
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string XsrfToken { get; set; }
    }
}
