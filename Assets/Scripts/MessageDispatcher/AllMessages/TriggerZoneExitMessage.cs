namespace MessageDispatcher.AllMessages
{
    public class TriggerZoneExitMessage : IMessage
    {
        public TriggerZone.ZoneType zoneType;

        public TriggerZoneExitMessage(TriggerZone.ZoneType type)
        {
            zoneType = type;
        }
    }
}