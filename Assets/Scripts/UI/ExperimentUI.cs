
using MessageDispatcher;
using UnityEngine;

namespace UI
{
    public abstract class ExperimentUI : MonoBehaviour
    {
        public abstract void Show(IMessage message);
        public abstract void Hide(IMessage message);
    }
}