using System;
using Xebia.Vouchers.Domain;

namespace Xebia.Vouchers.Adapter.VoucherPersistence.InMemory
{
    public class VoucherDto
    {
        public Guid Id { get; }
        public VoucherType VoucherType { get; }
        public bool Claimed { get; set; }

        public VoucherDto(VoucherType voucherType)
        {
            VoucherType = voucherType;
            Id = Guid.NewGuid();
            Claimed = false;
        }
    }
}