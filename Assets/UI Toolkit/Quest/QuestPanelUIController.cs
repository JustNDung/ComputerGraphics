using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using Quest;
using TriggerZone;
using MessageDispatcher.AllMessages;
using System.Collections.Generic;

[UxmlElement]
public partial class QuestPanelUIController : VisualElement
{
    [UxmlAttribute] private float panelWidth = 460f;
    [UxmlAttribute] private float hoverEdge = 24f;
    [UxmlAttribute] private int animationMs = 500;

    [Header("Auto Scale With Font")]
    [UxmlAttribute] private int baseFontSize = 14;
    [UxmlAttribute] private float widthPerFontSize = 22f;
    [UxmlAttribute] private float minWidth = 420f;
    [UxmlAttribute] private float maxWidth = 760f;

    [Header("Theme")]
    [UxmlAttribute] private Color cardColor = new Color(1f, 1f, 1f, 0.06f);
    [UxmlAttribute] private Color cardHoverColor = new Color(1f, 1f, 1f, 0.10f);
    [UxmlAttribute] private Color textColor = Color.white;
    [UxmlAttribute] private Color dimTextColor = new Color(1f, 1f, 1f, 0.72f);
    [UxmlAttribute] private Color completeColor = new Color(0.35f, 1f, 0.45f, 1f);
    [UxmlAttribute] private Color progressColor = new Color(0.35f, 0.75f, 1f, 1f);

    private VisualElement _panel;
    private VisualElement _overlay;
    private ScrollView _scroll;
    private Button _openBtn;
    private Button _closeBtn;
    private Label _questCompletePopup;

    private bool _isOpen;
    private ZoneType _currentZone = ZoneType.None;
    private QuestInstance _expandedQuest;
    
    // Biến flag để tránh việc di chuột làm mở lại panel ngay khi vừa bấm nút đóng
    private float _lastCloseTime;
    private const float HoverDelayAfterClose = 0.5f; 

    private int TitleSize => Mathf.RoundToInt(baseFontSize * 1.28f);
    private int BodySize => baseFontSize;
    private int SmallSize => Mathf.RoundToInt(baseFontSize * 0.85f);

    public QuestPanelUIController()
    {
        RegisterCallback<AttachToPanelEvent>(OnAttach);
        RegisterCallback<DetachFromPanelEvent>(OnDetach);
    }

    private void OnAttach(AttachToPanelEvent evt)
    {
        SetupUI();
        BindUI();
        BindQuestEvents();
        
        MessageDispatcher.MessageDispatcher.Subscribe<TriggerZoneEnterMessage>(OnZoneEnter);
        MessageDispatcher.MessageDispatcher.Subscribe<TriggerZoneExitMessage>(OnZoneExit);

        RefreshUI();
    }

    private void OnDetach(DetachFromPanelEvent evt)
    {
        MessageDispatcher.MessageDispatcher.Unsubscribe<TriggerZoneEnterMessage>(OnZoneEnter);
        MessageDispatcher.MessageDispatcher.Unsubscribe<TriggerZoneExitMessage>(OnZoneExit);

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestUpdated -= RefreshUI;
            QuestManager.Instance.OnQuestCompleted -= ShowQuestComplete;
        }
    }

    private void SetupUI()
    {
        _panel = this.Q<VisualElement>("quest-panel");
        _overlay = this.Q<VisualElement>("overlay");
        _scroll = this.Q<ScrollView>("quest-scroll");
        _openBtn = this.Q<Button>("quest-btn");
        _closeBtn = this.Q<Button>("close-btn");

        if (_panel == null) return;

        AutoResizePanel();
        ApplyTypography();

        _panel.style.right = -panelWidth;
        _overlay.style.display = DisplayStyle.None;
        CreateQuestCompletePopup();
    }
    
    private void ShowQuestComplete(QuestInstance quest)
    {
        _questCompletePopup.text =
            "✓ QUEST COMPLETED: " + quest.data.title;

        _questCompletePopup.style.opacity = 1;

        _questCompletePopup.experimental.animation
            .Start(
                new StyleValues
                {
                    opacity = 0
                },
                3000
            );
    }
    
    private void CreateQuestCompletePopup()
    {
        _questCompletePopup = new Label();

        _questCompletePopup.style.position = Position.Absolute;
        _questCompletePopup.style.top = 80;
        _questCompletePopup.style.left = Length.Percent(50);

        _questCompletePopup.style.translate =
            new Translate(new Length(-50, LengthUnit.Percent), 0);

        _questCompletePopup.style.paddingLeft = 24;
        _questCompletePopup.style.paddingRight = 24;
        _questCompletePopup.style.paddingTop = 14;
        _questCompletePopup.style.paddingBottom = 14;

        _questCompletePopup.style.backgroundColor =
            new Color(0.1f, 0.9f, 0.3f, 0.95f);

        _questCompletePopup.style.color = Color.white;

        _questCompletePopup.style.fontSize = 24;

        _questCompletePopup.style.unityFontStyleAndWeight =
            FontStyle.Bold;

        _questCompletePopup.style.borderTopLeftRadius = 14;
        _questCompletePopup.style.borderTopRightRadius = 14;
        _questCompletePopup.style.borderBottomLeftRadius = 14;
        _questCompletePopup.style.borderBottomRightRadius = 14;

        _questCompletePopup.style.opacity = 0;

        Add(_questCompletePopup);
    }

    private void ApplyTypography()
    {
        _panel.style.fontSize = baseFontSize;
    }

    private void AutoResizePanel()
    {
        float dynamicWidth = 280f + (baseFontSize * widthPerFontSize);
        panelWidth = Mathf.Clamp(dynamicWidth, minWidth, maxWidth);

        _panel.style.width = panelWidth;
        _panel.style.minWidth = minWidth;
        _panel.style.maxWidth = maxWidth;
    }

    private void BindUI()
    {
        // Sử dụng ClickEvent rõ ràng và ngăn chặn stopPropagation để tránh nhảy sự kiện
        _openBtn.RegisterCallback<ClickEvent>(evt => {
            TogglePanel();
            evt.StopPropagation();
        });

        _closeBtn.RegisterCallback<ClickEvent>(evt => {
            ClosePanel();
            evt.StopPropagation();
        });

        _overlay.RegisterCallback<ClickEvent>(evt => {
            ClosePanel();
            evt.StopPropagation();
        });

        // SỬA LỖI: Kiểm tra thời gian sau khi đóng để tránh việc Hover tự kích hoạt lại panel
        this.RegisterCallback<PointerMoveEvent>(evt =>
        {
            if (!_isOpen && Time.time > _lastCloseTime + HoverDelayAfterClose)
            {
                // Kiểm tra nếu chuột ở mép phải màn hình
                if (evt.position.x >= this.layout.width - hoverEdge)
                {
                    OpenPanel();
                }
            }
        });
    }

    private void BindQuestEvents()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestUpdated += RefreshUI;

            // NEW
            QuestManager.Instance.OnQuestCompleted += ShowQuestComplete;
        }
    }

    private void OnZoneEnter(TriggerZoneEnterMessage msg)
    {
        _currentZone = msg.zoneType;
        AutoExpandZoneQuest(); // Tìm và gán quest thuộc zone vào _expandedQuest
        OpenPanel();           // Tự động bật panel
        RefreshUI();
    }

    private void OnZoneExit(TriggerZoneExitMessage msg)
    {
        if (msg.zoneType != _currentZone) return;
        
        _currentZone = ZoneType.None;
        _expandedQuest = null; // Reset trạng thái expand
        ClosePanel();          // Tự động đóng panel khi rời vùng (Về trạng thái bình thường)
        RefreshUI();
    }

    private void AutoExpandZoneQuest()
    {
        if (QuestManager.Instance == null) return;
        _expandedQuest = null;
        foreach (var quest in QuestManager.Instance.ActiveQuests)
        {
            if (quest.data.zoneType == _currentZone)
            {
                _expandedQuest = quest;
                break;
            }
        }
    }

    private void TogglePanel()
    {
        if (_isOpen) ClosePanel();
        else OpenPanel();
    }

    private void OpenPanel()
    {
        if (_isOpen) return;
        _isOpen = true;
        _overlay.style.display = DisplayStyle.Flex;
        _panel.experimental.animation.Start(new StyleValues { right = 0f }, animationMs);
    }

    private void ClosePanel()
    {
        if (!_isOpen) return;
        _isOpen = false;
        _lastCloseTime = Time.time; // Lưu lại thời điểm đóng để chặn hover re-open
        _panel.experimental.animation.Start(new StyleValues { right = -panelWidth }, animationMs);
        _overlay.style.display = DisplayStyle.None;
    }

    private void RefreshUI()
    {
        if (_scroll == null || QuestManager.Instance == null) return;

        _scroll.Clear();
        foreach (var quest in QuestManager.Instance.ActiveQuests)
        {
            if (ShouldShowQuest(quest))
                DrawQuestCard(quest);
        }
    }

    private bool ShouldShowQuest(QuestInstance quest)
    {
        // Nếu đang ở trong Zone, chỉ hiện quest của Zone đó. 
        // Nếu không ở trong Zone, hiện Main Quest.
        return _currentZone == ZoneType.None ? quest.data.isMainQuest : quest.data.zoneType == _currentZone;
    }

    private void DrawQuestCard(QuestInstance quest)
    {
        bool expanded = (_expandedQuest == quest);
        VisualElement card = CreateCard();
        RegisterHover(card);

        card.Add(CreateHeader(quest, expanded));
        DrawProgressBar(card, quest);

        if (expanded)
        {
            DrawDescription(card, quest);
            DrawDivider(card);
            DrawObjectives(card, quest);
        }

        _scroll.Add(card);
    }
    
    // ... (Các hàm CreateCard, RegisterHover, v.v. giữ nguyên như cũ)
    private VisualElement CreateHeader(
        QuestInstance quest,
        bool expanded)
    {
        VisualElement row = new VisualElement();

        row.style.flexDirection = FlexDirection.Row;
        row.style.justifyContent = Justify.SpaceBetween;
        row.style.alignItems = Align.Center;

        string icon = quest.IsCompleted ? "✓ " :
            expanded ? "▼ " : "▶ ";

        Label title =
            new Label(icon + quest.data.title);

        title.style.fontSize = TitleSize;
        title.style.unityFontStyleAndWeight =
            FontStyle.Bold;

        title.style.whiteSpace = WhiteSpace.Normal;
        title.style.flexGrow = 1;

        title.style.color =
            quest.IsCompleted ? completeColor : textColor;

        if (quest.IsCompleted)
        {
            title.style.unityTextOutlineColor =
                new Color(0.2f, 1f, 0.3f);

            title.style.unityTextOutlineWidth = 1;
        }

        Label state = new Label(
            quest.IsCompleted ? "COMPLETED" : "ACTIVE"
        );

        state.style.fontSize = SmallSize;
        state.style.marginLeft = 8;

        state.style.color =
            quest.IsCompleted
                ? completeColor
                : dimTextColor;

        row.Add(title);
        row.Add(state);

        row.RegisterCallback<ClickEvent>(
            _ => ToggleQuest(quest));

        return row;
    }
    private VisualElement CreateCard() { /* Giữ nguyên logic của bạn */ VisualElement card = new VisualElement(); card.style.marginBottom = 12; card.style.paddingLeft = 14; card.style.paddingRight = 14; card.style.paddingTop = 12; card.style.paddingBottom = 12; card.style.backgroundColor = cardColor; card.style.borderTopLeftRadius = 10; card.style.borderTopRightRadius = 10; card.style.borderBottomLeftRadius = 10; card.style.borderBottomRightRadius = 10; return card; }
    private void RegisterHover(VisualElement card) { card.RegisterCallback<PointerEnterEvent>(_ => card.style.backgroundColor = cardHoverColor); card.RegisterCallback<PointerLeaveEvent>(_ => card.style.backgroundColor = cardColor); }
    private void ToggleQuest(QuestInstance quest) { _expandedQuest = (_expandedQuest == quest) ? null : quest; RefreshUI(); }
    private void DrawProgressBar(VisualElement parent, QuestInstance quest) { int total = quest.data.objectives.Count; int completed = 0; foreach (var obj in quest.data.objectives) { if (quest.progress[obj] >= obj.targetAmount) completed++; } float ratio = total == 0 ? 0f : (float)completed / total; Label txt = new Label($"{completed}/{total} Objectives"); txt.style.fontSize = SmallSize; txt.style.marginTop = 6; txt.style.color = dimTextColor; parent.Add(txt); VisualElement bg = new VisualElement(); bg.style.height = 6; bg.style.marginTop = 4; bg.style.backgroundColor = new Color(1f, 1f, 1f, 0.08f); bg.style.borderTopLeftRadius = 999; bg.style.borderBottomLeftRadius = 999; VisualElement fill = new VisualElement(); fill.style.height = 6; fill.style.width = Length.Percent(ratio * 100f); fill.style.backgroundColor = quest.IsCompleted ? completeColor : progressColor; fill.style.borderTopLeftRadius = 999; fill.style.borderBottomLeftRadius = 999; bg.Add(fill); parent.Add(bg); }
    private void DrawDescription(VisualElement parent, QuestInstance quest) { if (string.IsNullOrWhiteSpace(quest.data.description)) return; Label desc = new Label(quest.data.description); desc.style.marginTop = 10; desc.style.fontSize = BodySize; desc.style.whiteSpace = WhiteSpace.Normal; desc.style.color = dimTextColor; parent.Add(desc); }
    private void DrawDivider(VisualElement parent) { VisualElement line = new VisualElement(); line.style.height = 1; line.style.marginTop = 10; line.style.marginBottom = 10; line.style.backgroundColor = new Color(1f, 1f, 1f, 0.08f); parent.Add(line); }
    private void DrawObjectives(VisualElement parent, QuestInstance quest) { foreach (var obj in quest.data.objectives) { int cur = quest.progress[obj]; int max = obj.targetAmount; bool done = cur >= max; Label line = new Label((done ? "✓ " : "• ") + obj.description + $" ({cur}/{max})"); line.style.marginTop = 5; line.style.marginLeft = 8; line.style.fontSize = BodySize; line.style.whiteSpace = WhiteSpace.Normal; line.style.color = done ? completeColor : textColor; parent.Add(line); } }
}