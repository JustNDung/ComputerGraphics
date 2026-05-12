namespace MessageDispatcher.AllMessages
{
    public class QuestCheckMessage  : IMessage
    {
        public TriggerZone.ZoneType zoneType;
        
        public QuestCheckMessage(TriggerZone.ZoneType type)
        {
            zoneType = type;
        }
    }
}