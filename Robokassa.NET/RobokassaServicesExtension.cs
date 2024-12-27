using Microsoft.Extensions.DependencyInjection;

namespace Robokassa.NET
{
    public static class RobokassaServicesExtension
    {
        public static IServiceCollection AddRobokassa(this IServiceCollection services, string shopName,
            string password1, string password2, bool isTestEnv)
        {
            services.AddSingleton<IRobokassaService>(x =>
                new RobokassaService(new RobokassaOptions(shopName, password1, password2), isTestEnv));

            services.AddSingleton<IRobokassaPaymentValidator>(x => new RobokassaCallbackValidator(password1, password2));
            return services;
        }
    }
}