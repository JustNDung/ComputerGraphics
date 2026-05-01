namespace MessageDispatcher.AllMessages
{
    public class TriggerZoneEnterMessage : IMessage
    {
        public TriggerZone.ZoneType zoneType;

        public TriggerZoneEnterMessage(TriggerZone.ZoneType type)
        {
            zoneType = type;
        }
    }
}