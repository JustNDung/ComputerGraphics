
using MessageDispatcher;
using MessageDispatcher.AllMessages;
using UI.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DoorExperimentUI : ExperimentUI
    {
        [SerializeField] private Image hingePoint;
        [SerializeField] private Image handlePoint;
        
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
            hingePoint.gameObject.GetComponent<CinematicUIAnimator>().PlayShow();
            handlePoint.gameObject.GetComponent<CinematicUIAnimator>().PlayShow();
        }
        

        public override void Hide(IMessage message)
        {
            if (message is not DoorExperimentUIHideMessage msg) return;
            hingePoint.gameObject.GetComponent<CinematicUIAnimator>().PlayHide();
            handlePoint.gameObject.GetComponent<CinematicUIAnimator>().PlayHide();
        }
    }
}