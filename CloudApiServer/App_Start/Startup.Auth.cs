using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CloudApiServer.Providers;
using CloudApiServer.Models;
using Microsoft.Owin.Security.Infrastructure;

namespace CloudApiServer
{
    public partial class Startup
    {
        public static void ConfigureAuth(IAppBuilder app)
        {
            string publicClientId = "self";
            //todo testing factory
            UserManager<User> UserManagerFactory() => new UserManager<User>(new UserStore<User>(new CloudDbContext()));

            app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(publicClientId, UserManagerFactory),
                //AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                //ONLY FOR DEVELOPING: ALLOW INSECURE HTTP!
                AllowInsecureHttp = true
            });
        }
    }
}