using System;
using System.Collections.Generic;
using Xebia.Vouchers.Domain;

namespace Xebia.Vouchers.Adapter.VoucherPersistence.InMemory
{
    public class VoucherRepository : ICreateVouchers, IClaimVouchers
    {
        readonly Dictionary<Guid, VoucherDto> _vouchers = new Dictionary<Guid, VoucherDto>();
        
        public NewVoucher Create(VoucherType voucherType)
        {
            var voucher = new VoucherDto(voucherType);
            _vouchers.Add(voucher.Id, voucher);
            
            return new NewVoucher(voucher.Id, voucher.VoucherType);
        }
    }
}