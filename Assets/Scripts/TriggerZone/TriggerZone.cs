
using MessageDispatcher.AllMessages;
using UnityEngine;

namespace TriggerZone
{
    [RequireComponent(typeof(CameraTrigger))]
    public class TriggerZone : MonoBehaviour
    {
        [SerializeField] private ZoneType zoneType;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                MessageDispatcher.MessageDispatcher.Publish(new TriggerZoneEnterMessage(zoneType));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                MessageDispatcher.MessageDispatcher.Publish(new TriggerZoneExitMessage(zoneType));
            }
        }
    }
}