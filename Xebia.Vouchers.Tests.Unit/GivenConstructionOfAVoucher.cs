using System;
using FluentAssertions;
using Xebia.Vouchers.Domain;
using Xebia.Vouchers.Exceptions;
using Xunit;

namespace Xebia.Vouchers.Tests.Unit
{
    public class GivenConstructionOfAVoucher
    {
        [Fact]
        public void WhenAnInvalidGuidIsSupplied_ObjectCantBeCreated()
        {
            Record.Exception(() => 
                new NewVoucher(
                    Guid.Empty, 
                    VoucherType.FreeShipping))
                .Should()
                .BeOfType(typeof(CouldNotConstructDomainObject));
        }
    }
}