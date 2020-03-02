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
        readonly HttpClient _client = new HttpClientFactory().Get();
        
        [Fact]
        public async Task WhenPerformingValidRequest_ShouldReturn201Created()
        {
            var voucherTypeDto = new VoucherTypeDto();
            voucherTypeDto.VoucherType = VoucherTypeEnum.FreeShipping;

            using (var jsonContent = JsonStringContent(JsonConvert.SerializeObject(voucherTypeDto)))
            {
                HttpResponseMessage response = await _client.PostAsync("vouchers", jsonContent);
                
                response.StatusCode.Should().Be(HttpStatusCode.Created,
                    "the request is properly formatted and contains valid contents, " +
                    "which should lead to the creation of a voucher");
            }
        }
        
        [Fact]
        public async Task WhenSupplyingUnknownVoucherType_ShouldNotGenerateVoucher()
        {
            var voucherTypeDto = new VoucherTypeDto();
            voucherTypeDto.VoucherType = VoucherTypeEnum.ThisShouldLeadToFailures;

            using (var jsonContent = JsonStringContent(JsonConvert.SerializeObject(voucherTypeDto)))
            {
                HttpResponseMessage response = await _client.PostAsync("vouchers", jsonContent);
                
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                    "we've supplied incorrect information on purpose");
            }
        }
        
        private static StringContent JsonStringContent(string serializedJson)
        {
            return new StringContent(serializedJson, Encoding.UTF8, "application/json");
        }
    }
}
