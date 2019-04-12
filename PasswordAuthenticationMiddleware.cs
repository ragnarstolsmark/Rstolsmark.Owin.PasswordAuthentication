﻿using System;
using System.Net;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using static Rstolsmark.PasswordHashLib.PasswordHasher;

namespace Rstolsmark.Owin.PasswordAuthentication {

	using AppFunc = Func<IDictionary<string, object>, Task>;
	public class PasswordAuthenticationMiddleware {
		private string _hashedPassword, _realm;
		private AppFunc next;
		public PasswordAuthenticationMiddleware(PasswordAuthenticationOptions passwordAuthenticationOptions)
		{
			_hashedPassword = passwordAuthenticationOptions.HashedPassword;
			_realm = passwordAuthenticationOptions.Realm;
		}
		public void Initialize(AppFunc next) {
			this.next = next;
		}

		public async Task Invoke(IDictionary<string, object> environment) {
			try{
				var headers = environment["owin.RequestHeaders"] as IDictionary<string, string[]>;
				var authHeader = AuthenticationHeaderValue.Parse(headers["Authorization"][0]);
				var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
				var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
				var inpassword = credentials[1];
				if(!VerifyHashedPassword(_hashedPassword, inpassword)){
					ChallengeUser();
					return;
				}
				await next.Invoke(environment);
			} catch(Exception){
				ChallengeUser();
			}
			
			void ChallengeUser() {
				environment["owin.ResponseStatusCode"] = (int)HttpStatusCode.Unauthorized;
				var responseHeaders = environment["owin.ResponseHeaders"] as IDictionary<string, string[]>;
				var realmstring = string.IsNullOrWhiteSpace(_realm) ? string.Empty : $" realm=\"{_realm}\"";
                responseHeaders["WWW-Authenticate"] = new string[]{ $"Basic{realmstring}" };
            }
		}
	}
}
