using MessageDispatcher;
using MessageDispatcher.AllMessages;
using TriggerZone;
using UI.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UI
{
    public class HeadExperimentUI : ExperimentUI
    {
        [SerializeField] private UIDocument headExperimentUI;

        private void Start()
        {
            headExperimentUI.enabled = false;
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
            if (msg.zoneType != ZoneType.HeadExperiment) return;
            headExperimentUI.enabled = true;
        }
        

        public override void Hide(IMessage message)
        {
            if (message is not UIHideMessage msg) return;
            if (msg.zoneType != ZoneType.HeadExperiment) return;
            headExperimentUI.enabled = false;
        }
    }
}

