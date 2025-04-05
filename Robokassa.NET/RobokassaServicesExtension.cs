using Microsoft.Extensions.DependencyInjection;

namespace Robokassa.NET
{
    public static class RobokassaServicesExtension
    {
        public static IServiceCollection AddRobokassa(this IServiceCollection services, string shopName,
            string password1, string password2, string testpassword1, string testpassword2)
        {
            var robokassaOptions = new RobokassaOptions(shopName, password1, password2, testpassword1, testpassword2);
            services.AddSingleton<IRobokassaService>(_ => new RobokassaService(robokassaOptions));

            services.AddSingleton<IRobokassaPaymentValidator>(_ => new RobokassaCallbackValidator(robokassaOptions));
            return services;
        }
    }
}