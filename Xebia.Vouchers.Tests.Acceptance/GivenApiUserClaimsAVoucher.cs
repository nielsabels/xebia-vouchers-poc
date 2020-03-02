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
    public class GivenApiUserClaimsAVoucher
    {
        readonly HttpClient _client = new HttpClientFactory().Get();
        
        [Fact]
        public async Task WhenSupplyingAnInvalidVoucherId_ShouldGetBadRequest()
        {
            var id = "invalid-id";
            var response = await _client.PostAsync($"vouchers/{id}/claim", new StringContent(""));
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                "the voucher id is intentionally malformed");
        }        

        [Fact]
        public async Task WhenVoucherIsAlreadyClaimed_ShouldGetForbidden()
        {
            var voucherTypeDto = new VoucherTypeDto();
            voucherTypeDto.VoucherType = VoucherTypeEnum.FreeShipping;

            Guid voucherId = Guid.Empty;

            using (var jsonContent = JsonStringContent(JsonConvert.SerializeObject(voucherTypeDto)))
            {
                HttpResponseMessage response = await _client.PostAsync("vouchers", jsonContent);
                response.StatusCode.Should().Be(HttpStatusCode.Created, "test setup: the voucher should have been created");
                var createdVoucher = JsonConvert.DeserializeObject<ClaimedVoucherDto>(await response.Content.ReadAsStringAsync());
                voucherId = createdVoucher.Id;
            }
            
            var claimAttempt1 = await _client.PostAsync($"vouchers/{voucherId}/claim", new StringContent(""));
            claimAttempt1.StatusCode.Should().Be(HttpStatusCode.OK,
                "test setup: the initial claim attempt of the voucher should succeed");
            
            var claimAttempt2 = await _client.PostAsync($"vouchers/{voucherId}/claim", new StringContent(""));
            claimAttempt2.StatusCode.Should().Be(HttpStatusCode.Forbidden,
                "voucher can not be claimed for the second time");
        }        

        [Fact]
        public async Task WhenVoucherDoesNotExist_ShouldGetNotFound()
        {
            var id = Guid.NewGuid();
            var response = await _client.PostAsync($"vouchers/{id}/claim", new StringContent(""));
            
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                "the voucher id was randomly generated");
        }
        

        [Fact]
        public async Task WhenSupplyingValidVoucherId_ShouldGetOk()
        {
            var voucherTypeDto = new VoucherTypeDto();
            voucherTypeDto.VoucherType = VoucherTypeEnum.FreeShipping;

            Guid voucherId = Guid.Empty;

            using (var jsonContent = JsonStringContent(JsonConvert.SerializeObject(voucherTypeDto)))
            {
                HttpResponseMessage response = await _client.PostAsync("vouchers", jsonContent);
                response.StatusCode.Should().Be(HttpStatusCode.Created, "test setup: the voucher should have been created");
                var createdVoucher = JsonConvert.DeserializeObject<ClaimedVoucherDto>(await response.Content.ReadAsStringAsync());
                voucherId = createdVoucher.Id;
            }            
            
            var claimAttempt1 = await _client.PostAsync($"vouchers/{voucherId}/claim", new StringContent(""));
            claimAttempt1.StatusCode.Should().Be(HttpStatusCode.OK,
                "the initial claim attempt of the voucher should succeed");
        }
        
        private static StringContent JsonStringContent(string serializedJson)
        {
            return new StringContent(serializedJson, Encoding.UTF8, "application/json");
        }
    }
}

