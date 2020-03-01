using Xebia.Vouchers.Domain;

namespace Xebia.Vouchers
{
    public interface ICreateVouchers
    {
        NewVoucher Create(VoucherType voucherType);
    }
}