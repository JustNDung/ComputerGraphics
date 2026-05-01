using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Scene;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeDuration = 0.35f;

    private Queue<SceneLoadOptions> _queue = new();
    private bool _isLoading = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ======================
    // PUBLIC API
    // ======================

    public void Load(SceneID scene)
    {
        Load(new SceneLoadOptions(scene));
    }

    public void Load(SceneID scene, bool useLoadingUI)
    {
        Load(new SceneLoadOptions(scene)
        {
            useLoadingUI = useLoadingUI
        });
    }

    public void Load(SceneLoadOptions options)
    {
        _queue.Enqueue(options);

        if (!_isLoading)
            StartCoroutine(ProcessQueue());
    }

    // ======================
    // QUEUE
    // ======================

    IEnumerator ProcessQueue()
    {
        while (_queue.Count > 0)
        {
            yield return LoadRoutine(_queue.Dequeue());
        }
    }

    // ======================
    // MAIN LOAD
    // ======================

    IEnumerator LoadRoutine(SceneLoadOptions options)
    {
        _isLoading = true;

        if (options.useFade)
            yield return Fade(1);

        if (options.useLoadingUI && LoadingUIScreen.Instance != null)
            LoadingUIScreen.Instance.Show();

        float timer = 0f;

        AsyncOperation op =
            SceneManager.LoadSceneAsync(
                SceneDatabase.Names[options.scene]);

        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            float p = Mathf.Clamp01(op.progress / 0.9f);

            if (options.useLoadingUI &&
                LoadingUIScreen.Instance != null)
            {
                LoadingUIScreen.Instance.SetProgress(p);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        while (timer < options.minLoadTime)
        {
            timer += Time.deltaTime;

            if (options.useLoadingUI &&
                LoadingUIScreen.Instance != null)
            {
                float fake =
                    Mathf.Lerp(0.9f, 1f,
                    timer / options.minLoadTime);

                LoadingUIScreen.Instance.SetProgress(fake);
            }

            yield return null;
        }

        op.allowSceneActivation = true;

        while (!op.isDone)
            yield return null;

        if (options.useLoadingUI &&
            LoadingUIScreen.Instance != null)
        {
            LoadingUIScreen.Instance.Hide();
        }

        if (options.useFade)
            yield return Fade(0);

        options.onComplete?.Invoke();

        _isLoading = false;
    }

    // ======================
    // FADE
    // ======================

    IEnumerator Fade(float target)
    {
        if (fadeCanvas == null)
            yield break;

        float start = fadeCanvas.alpha;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            fadeCanvas.alpha =
                Mathf.Lerp(start, target, t / fadeDuration);

            yield return null;
        }

        fadeCanvas.alpha = target;
    }

    public bool IsLoading()
    {
        return _isLoading;
    }
}