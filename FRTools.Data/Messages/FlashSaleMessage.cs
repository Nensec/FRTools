using FRTools.Data.DataModels.FlightRisingModels;

namespace FRTools.Data.Messages
{
    public class FlashSaleMessage : GenericMessage
    {
        public override string MessageType { get; set; } = nameof(FlashSaleMessage);

        public FlashSaleMessage(MessageCategory source, FRItem item) : base(source, "Flash sale")
        {
            Item = item;
        }

        public FRItem Item { get; private set; }
    }
}
