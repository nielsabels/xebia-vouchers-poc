using Microsoft.Extensions.DependencyInjection;

namespace Xebia.Vouchers.Adapter.VoucherPersistence.InMemory
{
    public class DependencyRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<ICreateVouchers, VoucherRepository>();
            services.AddSingleton<IClaimVouchers, VoucherRepository>();
        }
    }
}