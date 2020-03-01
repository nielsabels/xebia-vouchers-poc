using Microsoft.Extensions.DependencyInjection;
using Xebia.Vouchers.UseCases;

namespace Xebia.Vouchers.API
{
    public class DependencyRegistration
    {
        internal static void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<CreateVoucherUseCase>();
            
            Xebia.Vouchers.Adapter.VoucherPersistence.InMemory.DependencyRegistration.Register(serviceCollection);
        }
    }
}