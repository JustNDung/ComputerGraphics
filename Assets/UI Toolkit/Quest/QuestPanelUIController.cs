using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using Quest;
using TriggerZone;
using MessageDispatcher.AllMessages;

public class QuestPanelUIController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private float panelWidth = 460f;
    [SerializeField] private float hoverEdge = 24f;
    [SerializeField] private int animationMs = 240;

    [Header("Auto Scale With Font")]
    [SerializeField] private Font uiFont;
    [SerializeField] private int baseFontSize = 14;
    [SerializeField] private float widthPerFontSize = 22f;   // panel rộng thêm theo font
    [SerializeField] private float minWidth = 420f;
    [SerializeField] private float maxWidth = 760f;

    [Header("Theme")]
    [SerializeField] private Color cardColor = new Color(1f,1f,1f,0.06f);
    [SerializeField] private Color cardHoverColor = new Color(1f,1f,1f,0.10f);
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private Color dimTextColor = new Color(1f,1f,1f,0.72f);
    [SerializeField] private Color completeColor = new Color(0.35f,1f,0.45f,1f);
    [SerializeField] private Color progressColor = new Color(0.35f,0.75f,1f,1f);

    private UIDocument _doc;

    private VisualElement _root;
    private VisualElement _panel;
    private VisualElement _overlay;
    private ScrollView _scroll;

    private Button _openBtn;
    private Button _closeBtn;

    private bool _isOpen;
    private ZoneType _currentZone = ZoneType.None;
    private QuestInstance _expandedQuest;

    private int TitleSize => Mathf.RoundToInt(baseFontSize * 1.28f);
    private int BodySize => baseFontSize;
    private int SmallSize => Mathf.RoundToInt(baseFontSize * 0.85f);

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        MessageDispatcher.MessageDispatcher
            .Subscribe<TriggerZoneEnterMessage>(OnZoneEnter);

        MessageDispatcher.MessageDispatcher
            .Subscribe<TriggerZoneExitMessage>(OnZoneExit);
    }

    private void OnDisable()
    {
        MessageDispatcher.MessageDispatcher
            .Unsubscribe<TriggerZoneEnterMessage>(OnZoneEnter);

        MessageDispatcher.MessageDispatcher
            .Unsubscribe<TriggerZoneExitMessage>(OnZoneExit);
    }

    private void Start()
    {
        SetupUI();
        BindUI();
        BindQuestEvents();
        RefreshUI();
    }

    private void OnDestroy()
    {
        if (QuestManager.Instance != null)
            QuestManager.Instance.OnQuestUpdated -= RefreshUI;
    }

    private void SetupUI()
    {
        _root = _doc.rootVisualElement;

        _panel = _root.Q<VisualElement>("quest-panel");
        _overlay = _root.Q<VisualElement>("overlay");
        _scroll = _root.Q<ScrollView>("quest-scroll");

        _openBtn = _root.Q<Button>("quest-btn");
        _closeBtn = _root.Q<Button>("close-btn");

        ApplyTypography();
        AutoResizePanel();

        _panel.style.right = -panelWidth;
        _overlay.style.display = DisplayStyle.None;
    }

    private void ApplyTypography()
    {
        if (uiFont != null)
            _panel.style.unityFont = uiFont;

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
        _openBtn.clicked += TogglePanel;
        _closeBtn.clicked += ClosePanel;

        _overlay.RegisterCallback<ClickEvent>(_ =>
        {
            ClosePanel();
        });

        _root.RegisterCallback<PointerMoveEvent>(evt =>
        {
            if (!_isOpen &&
                evt.position.x >= Screen.width - hoverEdge)
            {
                OpenPanel();
            }
        });
    }

    private void BindQuestEvents()
    {
        if (QuestManager.Instance != null)
            QuestManager.Instance.OnQuestUpdated += RefreshUI;
    }

    private void OnZoneEnter(TriggerZoneEnterMessage msg)
    {
        _currentZone = msg.zoneType;
        AutoExpandZoneQuest();
        OpenPanel();
        RefreshUI();
    }

    private void OnZoneExit(TriggerZoneExitMessage msg)
    {
        if (msg.zoneType != _currentZone) return;

        _currentZone = ZoneType.None;
        _expandedQuest = null;

        RefreshUI();
    }

    private void AutoExpandZoneQuest()
    {
        _expandedQuest = null;

        foreach (var quest in QuestManager.Instance.ActiveQuests)
        {
            if (quest.data.zoneType == _currentZone)
            {
                _expandedQuest = quest;
                return;
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
        _isOpen = true;

        _overlay.style.display = DisplayStyle.Flex;

        _panel.experimental.animation.Start(
            new StyleValues { right = 0 },
            animationMs);
    }

    private void ClosePanel()
    {
        _isOpen = false;

        _panel.experimental.animation.Start(
            new StyleValues { right = -panelWidth },
            animationMs);

        _overlay.style.display = DisplayStyle.None;
    }

    private void RefreshUI()
    {
        if (_scroll == null) return;
        if (QuestManager.Instance == null) return;

        _scroll.Clear();

        foreach (var quest in QuestManager.Instance.ActiveQuests)
        {
            if (!ShouldShowQuest(quest))
                continue;

            DrawQuestCard(quest);
        }
    }

    private bool ShouldShowQuest(QuestInstance quest)
    {
        if (_currentZone == ZoneType.None)
            return quest.data.isMainQuest;

        return quest.data.zoneType == _currentZone;
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

    private VisualElement CreateCard()
    {
        VisualElement card = new VisualElement();

        card.style.marginBottom = 12;
        card.style.paddingLeft = 14;
        card.style.paddingRight = 14;
        card.style.paddingTop = 12;
        card.style.paddingBottom = 12;

        card.style.backgroundColor = cardColor;

        card.style.borderTopLeftRadius = 10;
        card.style.borderTopRightRadius = 10;
        card.style.borderBottomLeftRadius = 10;
        card.style.borderBottomRightRadius = 10;

        return card;
    }

    private void RegisterHover(VisualElement card)
    {
        card.RegisterCallback<PointerEnterEvent>(_ =>
        {
            card.style.backgroundColor = cardHoverColor;
        });

        card.RegisterCallback<PointerLeaveEvent>(_ =>
        {
            card.style.backgroundColor = cardColor;
        });
    }

    private VisualElement CreateHeader(
        QuestInstance quest,
        bool expanded)
    {
        VisualElement row = new VisualElement();

        row.style.flexDirection = FlexDirection.Row;
        row.style.justifyContent = Justify.SpaceBetween;
        row.style.alignItems = Align.Center;

        Label title = new Label(
            (expanded ? "▼ " : "▶ ") + quest.data.title);

        title.style.fontSize = TitleSize;
        title.style.unityFontStyleAndWeight = FontStyle.Bold;
        title.style.whiteSpace = WhiteSpace.Normal;
        title.style.flexGrow = 1;

        title.style.color =
            quest.IsCompleted ? completeColor : textColor;

        Label state = new Label(
            quest.IsCompleted ? "DONE" : "ACTIVE");

        state.style.fontSize = SmallSize;
        state.style.marginLeft = 8;
        state.style.color =
            quest.IsCompleted ? completeColor : dimTextColor;

        row.Add(title);
        row.Add(state);

        row.RegisterCallback<ClickEvent>(_ =>
        {
            ToggleQuest(quest);
        });

        return row;
    }

    private void ToggleQuest(QuestInstance quest)
    {
        _expandedQuest =
            _expandedQuest == quest ? null : quest;

        RefreshUI();
    }

    private void DrawProgressBar(
        VisualElement parent,
        QuestInstance quest)
    {
        int total = quest.data.objectives.Count;
        int completed = 0;

        foreach (var obj in quest.data.objectives)
        {
            if (quest.progress[obj] >= obj.targetAmount)
                completed++;
        }

        float ratio =
            total == 0 ? 0f : (float)completed / total;

        Label txt = new Label(
            $"{completed}/{total} Objectives");

        txt.style.fontSize = SmallSize;
        txt.style.marginTop = 6;
        txt.style.color = dimTextColor;

        parent.Add(txt);

        VisualElement bg = new VisualElement();
        bg.style.height = 6;
        bg.style.marginTop = 4;
        bg.style.backgroundColor =
            new Color(1f,1f,1f,0.08f);

        bg.style.borderTopLeftRadius = 999;
        bg.style.borderTopRightRadius = 999;
        bg.style.borderBottomLeftRadius = 999;
        bg.style.borderBottomRightRadius = 999;

        VisualElement fill = new VisualElement();
        fill.style.height = 6;
        fill.style.width = Length.Percent(ratio * 100f);
        fill.style.backgroundColor =
            quest.IsCompleted ? completeColor : progressColor;

        fill.style.borderTopLeftRadius = 999;
        fill.style.borderTopRightRadius = 999;
        fill.style.borderBottomLeftRadius = 999;
        fill.style.borderBottomRightRadius = 999;

        bg.Add(fill);
        parent.Add(bg);
    }

    private void DrawDescription(
        VisualElement parent,
        QuestInstance quest)
    {
        if (string.IsNullOrWhiteSpace(
            quest.data.description))
            return;

        Label desc = new Label(
            quest.data.description);

        desc.style.marginTop = 10;
        desc.style.fontSize = BodySize;
        desc.style.whiteSpace = WhiteSpace.Normal;
        desc.style.color = dimTextColor;

        parent.Add(desc);
    }

    private void DrawDivider(VisualElement parent)
    {
        VisualElement line = new VisualElement();

        line.style.height = 1;
        line.style.marginTop = 10;
        line.style.marginBottom = 10;

        line.style.backgroundColor =
            new Color(1f,1f,1f,0.08f);

        parent.Add(line);
    }

    private void DrawObjectives(
        VisualElement parent,
        QuestInstance quest)
    {
        foreach (var obj in quest.data.objectives)
        {
            int cur = quest.progress[obj];
            int max = obj.targetAmount;

            bool done = cur >= max;

            Label line = new Label(
                (done ? "✓ " : "• ") +
                obj.description +
                $" ({cur}/{max})");

            line.style.marginTop = 5;
            line.style.marginLeft = 8;
            line.style.fontSize = BodySize;
            line.style.whiteSpace = WhiteSpace.Normal;

            line.style.color =
                done ? completeColor : textColor;

            parent.Add(line);
        }
    }
}