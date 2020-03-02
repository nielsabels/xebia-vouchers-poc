using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Xebia.Vouchers.Tests.Acceptance.Dto;
using Xunit;

namespace Xebia.Vouchers.Tests.Acceptance
{
    public class GivenApiUserNeedsAVoucher
    {
        static readonly HttpClient Client = new HttpClient();
        
        [Fact]
        public async Task WhenPerformingValidRequest_ShouldReturn201Created()
        {
            var voucherTypeDto = new VoucherTypeDto();
            voucherTypeDto.VoucherType = VoucherTypeEnum.FreeShipping;

            Client.Timeout = TimeSpan.FromSeconds(5);
            HttpResponseMessage response = await Client.PostAsync(
                "http://localhost:8080/api/vouchers", 
                new StringContent(
                    JsonConvert.SerializeObject(voucherTypeDto), 
                    Encoding.UTF8, 
                    "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.Created,
                "the request is properly formatted and contains valid contents, " +
                "which should lead to the creation of a voucher");
        }
        
        [Fact]
        public async Task WhenSupplyingUnknownVoucherType_ShouldNotGenerateVoucher()
        {
            var voucherTypeDto = new VoucherTypeDto();
            voucherTypeDto.VoucherType = VoucherTypeEnum.ThisShouldLeadToFailures;

            Client.Timeout = TimeSpan.FromSeconds(5);
            HttpResponseMessage response = await Client.PostAsync(
                "http://localhost:8080/api/vouchers", 
                new StringContent(
                    JsonConvert.SerializeObject(voucherTypeDto), 
                    Encoding.UTF8, 
                    "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                "we've supplied incorrect information on purpose");
        }
    }
}
