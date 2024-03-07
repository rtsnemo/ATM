using Serilog;

namespace ATM
{
    public static class Services
    {
        public static void AddService(this IServiceCollection services)
        {
            services.AddSingleton<ATMLogic>();
            services.AddSingleton<LogLogic>();
        }
    }
}
