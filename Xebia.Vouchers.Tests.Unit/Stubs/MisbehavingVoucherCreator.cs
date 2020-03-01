using System;
using Xebia.Vouchers.Domain;

namespace Xebia.Vouchers.Tests.Unit.Stubs
{
    public class MisbehavingVoucherCreator : ICreateVouchers
    {
        public NewVoucher Create(VoucherType voucherType)
        {
            throw new Exception("I always throw an exception when I get called");
        }
    }
}