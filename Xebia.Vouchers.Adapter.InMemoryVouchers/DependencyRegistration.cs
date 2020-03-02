using Microsoft.Extensions.DependencyInjection;

namespace Xebia.Vouchers.Adapter.VoucherPersistence.InMemory
{
    public class DependencyRegistration
    {
        public static void Register(IServiceCollection services)
        {
            var voucherRepository = new VoucherRepository();
            services.AddSingleton<ICreateVouchers>(voucherRepository);
            services.AddSingleton<IClaimVouchers>(voucherRepository);
        }
    }
}