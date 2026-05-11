using MessageDispatcher;
using MessageDispatcher.AllMessages;
using TriggerZone;
using UI.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UI
{
    public class Seesaw2ExperimentUI : ExperimentUI
    {
        [SerializeField] private UIDocument seesaw2ExperimentUI;

        private void Start()
        {
            seesaw2ExperimentUI.enabled = false;
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
            if (msg.zoneType != ZoneType.Seesaw2Experiment) return;
            seesaw2ExperimentUI.enabled = true;
        }
        

        public override void Hide(IMessage message)
        {
            if (message is not UIHideMessage msg) return;
            if (msg.zoneType != ZoneType.Seesaw2Experiment) return;
            seesaw2ExperimentUI.enabled = false;
        }
    }
}

