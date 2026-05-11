
using MessageDispatcher;
using MessageDispatcher.AllMessages;
using UI.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UI
{
    public class DoorExperimentUI : ExperimentUI
    {
        [SerializeField] private UIDocument doorExperimentUI;
        
        private void OnEnable()
        {
            MessageDispatcher.MessageDispatcher.Subscribe<DoorExperimentUIDisplayMessage>(Show);
            MessageDispatcher.MessageDispatcher.Subscribe<DoorExperimentUIHideMessage>(Hide);
        }
        
        private void OnDisable()
        {
            MessageDispatcher.MessageDispatcher.Unsubscribe<DoorExperimentUIDisplayMessage>(Show);
            MessageDispatcher.MessageDispatcher.Unsubscribe<DoorExperimentUIHideMessage>(Hide);
        }
        
        public override void Show(IMessage message)
        {
            if (message is not DoorExperimentUIDisplayMessage msg) return;
            doorExperimentUI.enabled = true;
        }
        

        public override void Hide(IMessage message)
        {
            if (message is not DoorExperimentUIHideMessage msg) return;
            doorExperimentUI.enabled = false;
        }
    }
}