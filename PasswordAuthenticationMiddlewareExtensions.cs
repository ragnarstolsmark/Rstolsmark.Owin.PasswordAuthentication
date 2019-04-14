using System;
using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>,
        System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>,
        System.Threading.Tasks.Task>>;
namespace Rstolsmark.Owin.PasswordAuthentication
{
    public static class PasswordAuthenticationMiddlewareExtensions
    {
        public static Action<MidFunc> UsePasswordAuthentication(this Action<MidFunc> builder, PasswordAuthenticationOptions options = null)
        {
            builder(next => new PasswordAuthenticationMiddleware(next, options).Invoke);
            return builder;
        }

    }
}