using Scene;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument _ui;

    private Button _continueBtn;
    private Button _newGameBtn;
    private Button _optionsBtn;
    private Button _achievementsBtn;

    void Awake()
    {
        _ui = GetComponent<UIDocument>();
    }

    void Start()
    {
        var root = _ui.rootVisualElement;

        _continueBtn     = root.Q<Button>("Continue");
        _newGameBtn      = root.Q<Button>("NewGame");
        _optionsBtn      = root.Q<Button>("Options");
        _achievementsBtn = root.Q<Button>("Achievements");

        RegisterButton(_newGameBtn, StartNewGame);
        RegisterButton(_continueBtn, ContinueGame);
        RegisterButton(_optionsBtn, OpenOptions);
        RegisterButton(_achievementsBtn, OpenAchievements);
    }

    void RegisterButton(Button button, System.Action onClick)
    {
        if (button == null) return;

        button.clicked += () => onClick?.Invoke();

        button.RegisterCallback<MouseEnterEvent>(_ =>
        {
            button.AddToClassList("menu-hover");
        });

        button.RegisterCallback<MouseLeaveEvent>(_ =>
        {
            button.RemoveFromClassList("menu-hover");
        });
    }

    void StartNewGame()
    {
        if (SceneLoader.Instance.IsLoading()) return;

        SceneLoader.Instance.Load(
            new SceneLoadOptions(SceneID.Gameplay)
            {
                useFade = false,
                useLoadingUI = false,
                minLoadTime = 1f
            });
    }

    void ContinueGame()
    {
        Debug.Log("Chưa có save.");
    }

    void OpenOptions()
    {
        Debug.Log("Mở options.");
    }

    void OpenAchievements()
    {
        Debug.Log("Mở achievements.");
    }
}