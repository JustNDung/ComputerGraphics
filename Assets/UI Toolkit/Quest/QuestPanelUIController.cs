using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TriggerZone;
using MessageDispatcher.AllMessages;
using UnityEngine.UIElements.Experimental;

public class QuestUIController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private float panelWidth = 420f;
    [SerializeField] private int animationMs = 300;
    [SerializeField] private float hoverEdgeSize = 20f;

    private UIDocument doc;

    private VisualElement root;
    private VisualElement panel;
    private VisualElement overlay;
    private ScrollView scroll;

    private Button openBtn;
    private Button closeBtn;

    private Label title;
    private Label subtitle;

    private bool isOpen;

    private ZoneType currentZone = ZoneType.None;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        MessageDispatcher.MessageDispatcher.Subscribe<TriggerZoneEnterMessage>(OnZoneEnter);
        MessageDispatcher.MessageDispatcher.Subscribe<TriggerZoneExitMessage>(OnZoneExit);
    }

    private void OnDisable()
    {
        MessageDispatcher.MessageDispatcher.Unsubscribe<TriggerZoneEnterMessage>(OnZoneEnter);
        MessageDispatcher.MessageDispatcher.Unsubscribe<TriggerZoneExitMessage>(OnZoneExit);
    }

    private void Start()
    {
        root = doc.rootVisualElement;

        panel = root.Q<VisualElement>("quest-panel");
        overlay = root.Q<VisualElement>("overlay");
        scroll = root.Q<ScrollView>("quest-scroll");

        openBtn = root.Q<Button>("quest-btn");
        closeBtn = root.Q<Button>("close-btn");

        title = root.Q<Label>("title");
        subtitle = root.Q<Label>("subtitle");

        SetupDefault();
        RegisterUIEvents();

        ShowZone(ZoneType.None);
    }

    private void SetupDefault()
    {
        panel.style.right = -panelWidth;

        overlay.style.display = DisplayStyle.None;
        overlay.style.opacity = 0f;
    }

    private void RegisterUIEvents()
    {
        openBtn.clicked += TogglePanel;
        closeBtn.clicked += ClosePanel;

        overlay.RegisterCallback<ClickEvent>(_ =>
        {
            ClosePanel();
        });

        root.RegisterCallback<PointerMoveEvent>(evt =>
        {
            if (!isOpen && evt.position.x >= Screen.width - hoverEdgeSize)
                OpenPanel();
        });
    }

    private void OnZoneEnter(TriggerZoneEnterMessage msg)
    {
        ShowZone(msg.zoneType);
        OpenPanel();
    }

    private void OnZoneExit(TriggerZoneExitMessage msg)
    {
        if (msg.zoneType == currentZone)
        {
            ShowZone(ZoneType.None);
        }
    }

    public void ShowZone(ZoneType zone)
    {
        currentZone = zone;

        switch (zone)
        {
            case ZoneType.None:
                ShowGlobal();
                break;

            case ZoneType.DoorExperiment:
                ShowDoorExperiment();
                break;
        }
    }

    private void ShowGlobal()
    {
        title.text = "QUESTS";
        subtitle.text = "Nearby Activities";

        SetQuestList(new List<string>()
        {
            "Go to Door Experiment",
            "Complete 1 experiment",
            "Earn reward points"
        });
    }

    private void ShowDoorExperiment()
    {
        title.text = "DOOR EXPERIMENT";
        subtitle.text = "Learn torque";

        SetQuestList(new List<string>()
        {
            "Push near hinge",
            "Push center",
            "Push far edge",
            "Compare movement",
            "Complete report"
        });
    }

    private void SetQuestList(List<string> quests)
    {
        scroll.Clear();

        foreach (var q in quests)
        {
            AddQuestCard(q);
        }
    }

    private void AddQuestCard(string text)
    {
        VisualElement card = new VisualElement();

        card.style.minHeight = 70;
        card.style.marginBottom = 12;
        card.style.paddingLeft = 14;
        card.style.paddingRight = 14;
        card.style.paddingTop = 12;
        card.style.paddingBottom = 12;

        card.style.backgroundColor = new Color(1,1,1,0.08f);

        card.style.borderTopLeftRadius = 12;
        card.style.borderTopRightRadius = 12;
        card.style.borderBottomLeftRadius = 12;
        card.style.borderBottomRightRadius = 12;

        Label label = new Label("• " + text);
        label.style.color = Color.white;
        label.style.fontSize = 15;
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
        label.style.whiteSpace = WhiteSpace.Normal;

        card.Add(label);
        scroll.Add(card);
    }

    public void TogglePanel()
    {
        if (isOpen) ClosePanel();
        else OpenPanel();
    }

    public void OpenPanel()
    {
        if (isOpen) return;

        isOpen = true;

        overlay.style.display = DisplayStyle.Flex;

        panel.experimental.animation.Start(
            new StyleValues { right = 0 },
            animationMs
        );

        overlay.experimental.animation.Start(
            new StyleValues { opacity = 1f },
            animationMs
        );
    }

    public void ClosePanel()
    {
        if (!isOpen) return;

        isOpen = false;

        panel.experimental.animation.Start(
            new StyleValues { right = -panelWidth },
            animationMs
        );

        overlay.experimental.animation.Start(
            new StyleValues { opacity = 0f },
            animationMs
        ).OnCompleted(() =>
        {
            overlay.style.display = DisplayStyle.None;
        });
    }
}