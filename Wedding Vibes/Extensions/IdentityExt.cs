using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace WeddingVibes.Extensions
{
    public static class IdentityExt
    {
        public static Task<T> GetCurrentUser<T>(this UserManager<T> manager, HttpContext httpContext) where T : class
        {
            return manager.GetUserAsync(httpContext.User);
        }
    }
}
