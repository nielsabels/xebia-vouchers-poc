using System;
using Xebia.Vouchers.Domain;

namespace Xebia.Vouchers.API.Dto
{
    public class ClaimedVoucherDto
    {
        public Guid Id { get; set; }
        public VoucherTypeEnum VoucherType { get; set; }
        public DateTime ClaimedOn { get; set; }

        public static ClaimedVoucherDto FromDomain(ClaimedVoucher claimedVoucher)
        {
            var dto = new ClaimedVoucherDto()
            {
                Id = claimedVoucher.Id,
                VoucherType = (VoucherTypeEnum) ((int) claimedVoucher.VoucherType),
                ClaimedOn = claimedVoucher.ClaimedOn
            };
            
            return dto;
        }    
    }
}