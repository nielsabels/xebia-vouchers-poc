using System;
using System.Collections.Generic;
using Xebia.Vouchers.Domain;
using Xebia.Vouchers.Exceptions;

namespace Xebia.Vouchers.Adapter.VoucherPersistence.InMemory
{
    public class VoucherRepository : ICreateVouchers, IClaimVouchers
    {
        private readonly object syncRoot = new object();
        
        readonly Dictionary<Guid, VoucherDto> _vouchers = new Dictionary<Guid, VoucherDto>();
        
        public NewVoucher Create(VoucherType voucherType)
        {
            var voucher = new VoucherDto(voucherType);
            _vouchers.Add(voucher.Id, voucher);
            
            return new NewVoucher(voucher.Id, voucher.VoucherType);
        }

        public ClaimedVoucher Claim(Guid voucherId)
        {
            lock (syncRoot)
            {
                if (!_vouchers.ContainsKey(voucherId))
                    throw new VoucherDoesNotExist($"voucher ({voucherId}) can't be found");

                var voucher = _vouchers[voucherId];

                if (voucher.Claimed)
                    throw new VoucherAlreadyClaimed($"voucher ({voucherId}) has been claimed before");

                voucher.Claimed = true;

                return new ClaimedVoucher(voucherId, voucher.VoucherType, DateTime.UtcNow);
            }
        }
    }
}