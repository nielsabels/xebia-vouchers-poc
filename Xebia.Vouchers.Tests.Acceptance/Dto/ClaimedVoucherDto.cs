using System;

namespace Xebia.Vouchers.Tests.Acceptance.Dto
{
    public class ClaimedVoucherDto
    {
        public Guid Id { get; set; }
        public VoucherTypeEnum VoucherType { get; set; }
        public DateTime ClaimedOn { get; set; }
    }
}