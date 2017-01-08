using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Web.Graph.Models.Auth;
using Owin.Security.Providers.GitHub;
using Owin.Security.Providers.Slack;

//using Microsoft.Owin.Security.Google;

namespace Web.Graph
{
    public partial class Startup
    {
        // Para obtener más información para configurar la autenticación, visite http://go.microsoft.com/fwlink/?LinkId=301864
        /// <summary>
        /// Configura Auth.
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure el contexto de base de datos, el administrador de usuarios y el administrador de inicios de sesión para usar una única instancia por solicitud
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Permitir que la aplicación use una cookie para almacenar información para el usuario que inicia sesión
            // y una cookie para almacenar temporalmente información sobre un usuario que inicia sesión con un proveedor de inicio de sesión de terceros
            // Configurar cookie de inicio de sesión
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Permite a la aplicación validar la marca de seguridad cuando el usuario inicia sesión.
                    // Es una característica de seguridad que se usa cuando se cambia una contraseña o se agrega un inicio de sesión externo a la cuenta.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Permite que la aplicación almacene temporalmente la información del usuario cuando se verifica el segundo factor en el proceso de autenticación de dos factores.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Permite que la aplicación recuerde el segundo factor de verificación de inicio de sesión, como el teléfono o correo electrónico.
            // Cuando selecciona esta opción, el segundo paso de la verificación del proceso de inicio de sesión se recordará en el dispositivo desde el que ha iniciado sesión.
            // Es similar a la opción Recordarme al iniciar sesión.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Quitar los comentarios de las siguientes líneas para habilitar el inicio de sesión con proveedores de inicio de sesión de terceros
            app.UseMicrosoftAccountAuthentication(
                clientId: "810bf223-e8b9-4d76-80b4-0158546f9f6e",
                clientSecret: "FpQ8SHn0dCjYAnh9YiMfXBh");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            app.UseGitHubAuthentication(
                clientId: "0c096a9a1ef60bf5e550",
                clientSecret: "178940fa40bd5561db9225418a8dd472ca32e00d"
            );

            var options = new SlackAuthenticationOptions
            {
                ClientId = "124669683251.124031221440",
                ClientSecret = "43afe63c4bcbe904a163e5fe66ffafe2",
                TeamId = "" // optional
            };
            options.Scope.Add("identify");
            app.UseSlackAuthentication(options);

            app.UseFacebookAuthentication(
               appId: "1531070826922053",
               appSecret: "8f8c185ab6bd649c4a7b27fc1d2c2fe2");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "800840438376-j64c3l8uqb0fdf1m69l5l8pj698s1u9h.apps.googleusercontent.com",
                ClientSecret = "F-yMc8f7vfN1DRy7hu4fSMEl"
            });
        }
    }
}