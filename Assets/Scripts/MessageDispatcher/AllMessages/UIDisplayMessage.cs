namespace MessageDispatcher.AllMessages
{
    public class UIDisplayMessage : IMessage
    {
        public TriggerZone.ZoneType zoneType;
        
        public UIDisplayMessage(TriggerZone.ZoneType type)
        {
            zoneType = type;
        }
    }
}