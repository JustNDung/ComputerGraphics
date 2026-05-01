namespace UI
{
    using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CinematicUIAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform rectTransform;

        [Header("Animation Settings")]
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float delay = 0f;

        [Header("Fade")]
        [SerializeField] private bool useFade = true;

        [Header("Scale")]
        [SerializeField] private bool useScale = true;
        [SerializeField] private Vector3 startScale = new Vector3(0.8f, 0.8f, 0.8f);
        [SerializeField] private Vector3 endScale = Vector3.one;

        [Header("Slide")]
        [SerializeField] private bool useSlide = false;
        [SerializeField] private Vector2 startOffset = new Vector2(0, -100);

        [Header("Bounce")]
        [SerializeField] private bool useBounce = true;
        [SerializeField] private float bounceStrength = 1.1f;

        private Vector2 originalPos;

        private void Awake()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();

            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            originalPos = rectTransform.anchoredPosition;
        }

        public void PlayShow()
        {
            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(AnimateIn());
        }

        public void PlayHide()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateOut());
        }

        private IEnumerator AnimateIn()
        {
            yield return new WaitForSeconds(delay);

            float time = 0f;

            if (useFade)
                canvasGroup.alpha = 0;

            if (useScale)
                rectTransform.localScale = startScale;

            if (useSlide)
                rectTransform.anchoredPosition = originalPos + startOffset;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;

                // easing (smooth cinematic)
                float ease = EaseOutCubic(t);

                if (useFade)
                    canvasGroup.alpha = Mathf.Lerp(0, 1, ease);

                if (useScale)
                {
                    Vector3 scale = Vector3.Lerp(startScale, endScale, ease);

                    if (useBounce)
                    {
                        float bounce = Mathf.Sin(t * Mathf.PI) * (bounceStrength - 1f);
                        scale *= (1 + bounce);
                    }

                    rectTransform.localScale = scale;
                }

                if (useSlide)
                    rectTransform.anchoredPosition = Vector2.Lerp(originalPos + startOffset, originalPos, ease);

                yield return null;
            }

            // đảm bảo đúng state cuối
            canvasGroup.alpha = 1;
            rectTransform.localScale = endScale;
            rectTransform.anchoredPosition = originalPos;
        }

        private IEnumerator AnimateOut()
        {
            float time = 0f;

            Vector3 initialScale = rectTransform.localScale;
            float initialAlpha = canvasGroup.alpha;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;

                float ease = EaseInCubic(t);

                if (useFade)
                    canvasGroup.alpha = Mathf.Lerp(initialAlpha, 0, ease);

                if (useScale)
                    rectTransform.localScale = Vector3.Lerp(initialScale, startScale, ease);

                yield return null;
            }

            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        // 🎯 Easing functions (tạo cảm giác cinematic)
        private float EaseOutCubic(float t)
        {
            return 1 - Mathf.Pow(1 - t, 3);
        }

        private float EaseInCubic(float t)
        {
            return t * t * t;
        }
    }
}
}