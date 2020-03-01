using System;
using Xebia.Vouchers.Adapter.VoucherPersistence.InMemory;
using Xebia.Vouchers.Domain;
using Xebia.Vouchers.UseCases;
using Xunit;
using FluentAssertions;
using Xebia.Vouchers.Exceptions;
using Xebia.Vouchers.Tests.Unit.Stubs;

namespace Xebia.Vouchers.Tests.Unit
{
    public class GivenCreationOfAVoucher
    {
        private CreateVoucherUseCase _sut;
        
        public GivenCreationOfAVoucher()
        {
            var voucherRepository = new VoucherRepository();

            ICreateVouchers sutVoucherCreator = voucherRepository;
            IClaimVouchers sutVoucherClaimer = voucherRepository;
            _sut = new CreateVoucherUseCase(sutVoucherCreator);
        }
        
        [Fact]
        public void WhenValidVoucherTypeSupplied_ShouldReturnNewVoucherWithCorrectType()
        {
            var voucherType = VoucherType.FreeShipping;
            var newVoucher = _sut.Create(voucherType);

            newVoucher.VoucherType.Should().Be(voucherType);
        }

        [Fact]
        public void WhenValidVoucherTypeSupplied_ShouldReturnNewVoucherWithNonEmptyGuid()
        {
            var voucherType = VoucherType.FreeShipping;
            var newVoucher = _sut.Create(voucherType);

            newVoucher.Id.Should().NotBeEmpty();
            newVoucher.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void WhenCreatingTwoVouchers_TheirIdentifiersShouldNotBeTheSame()
        {
            var voucherType = VoucherType.FreeShipping;
            var newVoucher1 = _sut.Create(voucherType);
            var newVoucher2 = _sut.Create(voucherType);

            newVoucher1.Id.Should().NotBe(newVoucher2.Id);
        }

        [Fact]
        public void WhenTheAdapterThrowsException_TheUseCaseShouldWrapTheExceptionInACustomException()
        {
            var voucherCreator = new MisbehavingVoucherCreator();
            var sut = new CreateVoucherUseCase(voucherCreator);
            Record.Exception(() => 
                { sut.Create(VoucherType.FreeShipping); })
                .Should()
                .BeOfType<CouldNotCreateAVoucher>();
        }
    }
}