using System;

namespace Xebia.Vouchers.Domain
{
    public class ClaimedVoucher
    {
        public Guid Id { get; }
        public VoucherType VoucherType { get; }   
        public DateTime ClaimedOn { get; }

        public ClaimedVoucher(Guid id, VoucherType voucherType, DateTime claimedOn)
        {
            Id = id;
            VoucherType = voucherType;
            ClaimedOn = claimedOn;
        }
    }
}