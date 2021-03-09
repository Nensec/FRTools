using FRTools.Data.DataModels.FlightRisingModels;

namespace FRTools.Data.Messages
{
    public class NewItemMessage : GenericMessage
    {
        public override string MessageType { get; } = nameof(NewItemMessage);

        public NewItemMessage(MessageCategory source, FRItem item) : base(source, "New item")
        {
            Item = item;
        }

        public FRItem Item { get; private set; }
    }
}
