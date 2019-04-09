using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rstolsmark.Owin.PasswordAuthentication {

	using AppFunc = Func<IDictionary<string, object>, Task>;
	public class PasswordAuthenticationMiddleware {
		private AppFunc next;

		public void Initialize(AppFunc next) {
			this.next = next;
		}

		public async Task Invoke(IDictionary<string, object> environment) {
			Console.WriteLine("Begin Request");
			await next.Invoke(environment);
			Console.WriteLine("End Request");
		}
	}
}
