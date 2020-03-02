using Newtonsoft.Json.Converters;

namespace Xebia.Vouchers.Tests.Acceptance.Dto
{
    public class VoucherTypeDto
    {
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public VoucherTypeEnum VoucherType { get; set; } = VoucherTypeEnum.FreeShipping;
    }
}