using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xebia.Vouchers.UseCases;

namespace Xebia.Vouchers.API
{
    public class DependencyRegistration
    {
        internal static void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<CreateVoucherUseCase>();
            serviceCollection.AddSingleton(Log.Logger);
            
            Xebia.Vouchers.Adapter.VoucherPersistence.InMemory.DependencyRegistration.Register(serviceCollection);
        }
    }
}