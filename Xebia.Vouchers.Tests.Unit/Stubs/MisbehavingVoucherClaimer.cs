using System;
using Xebia.Vouchers.Domain;

namespace Xebia.Vouchers.Tests.Unit.Stubs
{
    public class MisbehavingVoucherClaimer : IClaimVouchers
    {
        public ClaimedVoucher Claim(Guid voucherId)
        {
            throw new Exception("I always throw an exception when I get called");
        }
    }
}