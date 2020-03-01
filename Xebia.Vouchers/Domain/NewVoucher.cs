using System;
using Xebia.Vouchers.Exceptions;

namespace Xebia.Vouchers.Domain
{
    public class NewVoucher
    {
        public Guid Id { get; }
        public VoucherType VoucherType { get; }        
        
        public NewVoucher(Guid id, VoucherType voucherType)
        {
            if (id == Guid.Empty)
                throw new CouldNotConstructDomainObject($"Empty Guid supplied for field: {Id}, please provide a non-empty Guid instead");
            
            Id = id;
            VoucherType = voucherType;
        }
    }
}