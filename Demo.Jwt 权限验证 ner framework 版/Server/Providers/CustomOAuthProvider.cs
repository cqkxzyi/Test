using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace CY.AuthorizationServer.Api.Providers
{
    /// <summary>
    /// OAuth身份认证
    /// 两种方式（1：clientId   2：UserName ）
    /// </summary>
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {

        #region 方式1
        /// <summary>
        /// 对应客户端请求参数：
        /// grant_type:client_credentials
        /// client_id:111
        /// client_secret:2222
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override  Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string symmetricKeyAsBase64 = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                //return;
                return Task.FromResult<object>(null);
            }

            var audience = AudiencesStore.FindAudience(context.ClientId);

            if (audience == null)
            {
                context.SetError("invalid_clientId", string.Format("Invalid client_id '{0}'", context.ClientId));
                // return;
                return Task.FromResult<object>(null);
            }

            context.Validated();

            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, "Xiaomi"));
            oAuthIdentity.AddClaim(new Claim("aa", context.ClientId));
            oAuthIdentity.AddClaim(new Claim("bb", "bb"));
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                         "audience", (context.ClientId == null) ? string.Empty : context.ClientId
                    }
                });

            var ticket = new AuthenticationTicket(oAuthIdentity, props);
            context.Validated(ticket);
            return base.GrantClientCredentials(context);
        }

        #endregion




        #region 方式2
        public override  Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //业务逻辑、验证账户、密码
            if (context.UserName != context.Password)
            {
                //context.SetError("invalid_grant", "密码错误");
                //return Task.FromResult<object>(null);
            }

            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Manager"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Supervisor"));
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                         "audience", (context.ClientId == null) ? string.Empty : context.ClientId
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

            return base.GrantResourceOwnerCredentials(context);
        }
        #endregion
    }
}