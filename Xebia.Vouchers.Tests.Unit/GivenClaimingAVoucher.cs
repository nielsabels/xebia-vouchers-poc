using System;
using FluentAssertions;
using Xebia.Vouchers.Adapter.VoucherPersistence.InMemory;
using Xebia.Vouchers.Domain;
using Xebia.Vouchers.Exceptions;
using Xebia.Vouchers.Tests.Unit.Stubs;
using Xebia.Vouchers.UseCases;
using Xunit;

namespace Xebia.Vouchers.Tests.Unit
{
    public class GivenClaimingAVoucher
    {
        private ClaimVoucherUseCase _sut;
        private ICreateVouchers _sutVoucherCreator;
        private IClaimVouchers _sutVoucherClaimer;
        
        public GivenClaimingAVoucher()
        {
            var voucherRepository = new VoucherRepository();

            _sutVoucherCreator = voucherRepository;
            _sutVoucherClaimer = voucherRepository;
            _sut = new ClaimVoucherUseCase(_sutVoucherClaimer);
        }
        
        [Fact]
        public void WhenNonExistingVoucherIdIsUsed_ShouldClaimCantFindVoucher()
        {
            var exception = Record.Exception(() => _sut.Claim(Guid.NewGuid()));

            exception.Should().BeOfType<VoucherDoesNotExist>(
                "we used a random Guid which should never lead to an existing voucher");
        }

        [Fact]
        public void WhenNewVoucherIsUsed_ShouldBeAbleToClaim()
        {
            var newVoucher = _sutVoucherCreator.Create(VoucherType.FreeShipping);
            var claimedVoucher = _sutVoucherClaimer.Claim(newVoucher.Id);

            claimedVoucher.Id.Should().Be(newVoucher.Id);
        }

        [Fact]
        public void WhenVoucherIsUsedMoreThanOnce_ShouldNotClaimVoucher()
        {
            var newVoucher = _sutVoucherCreator.Create(VoucherType.FreeShipping);
            
            var firstAttempt = _sutVoucherClaimer.Claim(newVoucher.Id);
            var secondAttempt = Record.Exception(() => _sutVoucherClaimer.Claim(newVoucher.Id));

            secondAttempt.Should().BeOfType<VoucherAlreadyClaimed>("a voucher can't be used twice");
        }

        [Fact]
        public void WhenTheAdapterThrowsException_TheUseCaseShouldWrapTheExceptionInACustomException()
        {
            var voucherClaimer = new MisbehavingVoucherClaimer();
            var sut = new ClaimVoucherUseCase(voucherClaimer);
            Record.Exception(() => 
                    { sut.Claim(Guid.NewGuid()); })
                .Should()
                .BeOfType<CouldNotClaimVoucher>();
        }
    }
}