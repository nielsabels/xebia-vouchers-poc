using System;
using System.Configuration;
using Xebia.Vouchers.Domain;

namespace Xebia.Vouchers.API.Dto
{
    public class NewVoucherDto
    {
        public Guid Id { get; set; }
        public VoucherTypeEnum VoucherType { get; set; }

        public static NewVoucherDto FromDomain(NewVoucher newVoucher)
        {
            var dto = new NewVoucherDto()
            {
                Id = newVoucher.Id,
                VoucherType = (VoucherTypeEnum) ((int) newVoucher.VoucherType)
            };
            
            return dto;
        }       
    }
}