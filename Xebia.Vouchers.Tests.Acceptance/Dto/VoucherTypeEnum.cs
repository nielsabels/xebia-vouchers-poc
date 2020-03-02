using Newtonsoft.Json.Converters;

namespace Xebia.Vouchers.Tests.Acceptance.Dto
{
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum VoucherTypeEnum
    {
        FreeShipping = 0,
        ThisShouldLeadToFailures = 1024
    }    
}