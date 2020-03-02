using System;
using Xebia.Vouchers.Domain;

namespace Xebia.Vouchers
{
    public interface IClaimVouchers
    {
        ClaimedVoucher Claim(Guid voucherId);
    }
}