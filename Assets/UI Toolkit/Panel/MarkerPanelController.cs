using UnityEngine.UIElements;
using UnityEngine;

[UxmlElement]
public partial class MarkerPanelController : VisualElement
{
    private VisualElement _panel;
    private VisualElement _marker;
    private Label _statusLabel;
    private Button _btnClose;
    private const string HIDDEN_CLASS = "panel-hidden";

    public MarkerPanelController()
    {
        RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
    }

    private void OnAttachToPanel(AttachToPanelEvent evt)
    {
        _marker = this.Q<VisualElement>("Marker");
        _panel = this.Q<VisualElement>("InfoPanel");
        _statusLabel = this.Q<Label>(className: "description-text");
        _btnClose = this.Q<Button>("BtnClose");

        if (_marker != null)
        {
            _marker.RegisterCallback<MouseEnterEvent>(e => ShowPanel());
            _marker.RegisterCallback<ClickEvent>(e => TogglePanel());
        }

        if (_btnClose != null) _btnClose.clicked += HidePanel;

        // CHỈ ẨN PANEL KHI GAME ĐANG CHẠY
        // Trong UI Builder (Editor), điều kiện này sẽ sai nên HidePanel() không bị gọi
        if (Application.isPlaying)
        {
            HidePanel();
        }
    }

    private void ShowPanel() => _panel?.RemoveFromClassList(HIDDEN_CLASS);
    private void HidePanel() => _panel?.AddToClassList(HIDDEN_CLASS);
    
    private void TogglePanel()
    {
        if (_panel == null) return;
        if (_panel.ClassListContains(HIDDEN_CLASS)) ShowPanel();
        else HidePanel();
    }
}