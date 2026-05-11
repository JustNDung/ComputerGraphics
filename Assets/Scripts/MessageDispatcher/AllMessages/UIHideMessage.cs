namespace MessageDispatcher.AllMessages
{
    public class UIHideMessage : IMessage
    {
        public TriggerZone.ZoneType zoneType;
        
        public UIHideMessage(TriggerZone.ZoneType type)
        {
            zoneType = type;
        }
    }
}