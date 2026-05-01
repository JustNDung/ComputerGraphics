using UnityEngine;
using UnityEngine.UIElements;

namespace Scene
{
    public class LoadingUIScreen : MonoBehaviour
    {
        public static LoadingUIScreen Instance;

        private UIDocument _ui;
        private VisualElement _root;
        private ProgressBar _bar;
        private Label _text;

        void Awake()
        {
            Instance = this;

            _ui = GetComponent<UIDocument>();
            _root = _ui.rootVisualElement;

            _bar = _root.Q<ProgressBar>("loadingBar");
            _text = _root.Q<Label>("loadingText");

            Hide();
        }

        public void Show()
        {
            _root.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            _root.style.display = DisplayStyle.None;
        }

        public void SetProgress(float value)
        {
            if (_bar != null)
                _bar.value = value * 100f;

            if (_text != null)
                _text.text = $"Loading {(int)(value * 100)}%";
        }
    }
}