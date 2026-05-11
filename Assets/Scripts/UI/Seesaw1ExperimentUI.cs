using MessageDispatcher;
using MessageDispatcher.AllMessages;
using TriggerZone;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class Seesaw1ExperimentUI : ExperimentUI
    {
        [SerializeField] private UIDocument seesaw1ExperimentUI;

        private void Start()
        {
            seesaw1ExperimentUI.enabled = false;
        }
        
        private void OnEnable()
        {
            MessageDispatcher.MessageDispatcher.Subscribe<UIDisplayMessage>(Show);
            MessageDispatcher.MessageDispatcher.Subscribe<UIHideMessage>(Hide);
        }
        
        private void OnDisable()
        {
            MessageDispatcher.MessageDispatcher.Unsubscribe<UIDisplayMessage>(Show);
            MessageDispatcher.MessageDispatcher.Unsubscribe<UIHideMessage>(Hide);
        }
        
        public override void Show(IMessage message)
        {
            if (message is not UIDisplayMessage msg) return;
            if (msg.zoneType != ZoneType.Seesaw1Experiment) return;
            seesaw1ExperimentUI.enabled = true;
        }
        

        public override void Hide(IMessage message)
        {
            if (message is not UIHideMessage msg) return;
            if (msg.zoneType != ZoneType.Seesaw1Experiment) return;
            seesaw1ExperimentUI.enabled = false;
        }
    }
}
