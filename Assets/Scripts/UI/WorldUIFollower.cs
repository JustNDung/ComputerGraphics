using UnityEngine;

namespace UI
{
    public class WorldUIFollower : MonoBehaviour
    {
        public enum FollowMode
        {
            Transform,
            WorldPosition,
            LocalPoint
        }

        [Header("Follow Settings")]
        public FollowMode followMode = FollowMode.Transform;

        public Transform targetTransform;
        public Vector3 worldPosition;
        public Vector3 localPoint;

        [Header("UI")]
        public RectTransform uiElement;
        public Camera cam;

        [Header("Offset")]
        public Vector3 worldOffset;
        public Vector2 screenOffset;

        [Header("Behavior")]
        public bool hideWhenBehind = true;
        public bool smoothFollow = true;

        [Header("Smooth Settings")]
        public float positionSmoothTime = 0.05f;
        public float alphaSmoothTime = 0.1f;
        public float scaleSmoothTime = 0.1f;

        [Header("Scale With Distance")]
        public bool scaleByDistance = false;
        public float scaleMultiplier = 10f;
        public Vector2 scaleClamp = new Vector2(0.5f, 1.5f);

        // Internal
        private Vector3 positionVelocity;
        private float alphaVelocity;
        private float scaleVelocity;

        private Canvas canvas;
        private CanvasGroup canvasGroup;

        void Awake()
        {
            if (uiElement == null)
                uiElement = GetComponent<RectTransform>();

            canvas = uiElement.GetComponentInParent<Canvas>();

            canvasGroup = uiElement.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = uiElement.gameObject.AddComponent<CanvasGroup>();
        }

        void LateUpdate()
        {
            if (cam == null || uiElement == null) return;

            Vector3 worldPos = GetWorldPosition();
            Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

            bool isBehind = screenPos.z < 0;

            // ===== POSITION =====
            screenPos += (Vector3)screenOffset;

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                UpdatePositionOverlay(screenPos);
            }
            else
            {
                UpdatePositionCamera(screenPos);
            }

            // ===== FADE =====
            if (hideWhenBehind)
            {
                float targetAlpha = isBehind ? 0f : 1f;

                canvasGroup.alpha = Mathf.SmoothDamp(
                    canvasGroup.alpha,
                    targetAlpha,
                    ref alphaVelocity,
                    alphaSmoothTime
                );

                uiElement.gameObject.SetActive(canvasGroup.alpha > 0.01f);
            }

            // ===== SCALE =====
            UpdateScale(screenPos, isBehind);
        }

        // ================= POSITION =================

        void UpdatePositionOverlay(Vector3 screenPos)
        {
            if (smoothFollow)
            {
                uiElement.position = Vector3.SmoothDamp(
                    uiElement.position,
                    screenPos,
                    ref positionVelocity,
                    positionSmoothTime
                );
            }
            else
            {
                uiElement.position = screenPos;
            }
        }

        void UpdatePositionCamera(Vector3 screenPos)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                cam,
                out localPos
            );

            if (smoothFollow)
            {
                uiElement.localPosition = Vector3.SmoothDamp(
                    uiElement.localPosition,
                    localPos,
                    ref positionVelocity,
                    positionSmoothTime
                );
            }
            else
            {
                uiElement.localPosition = localPos;
            }
        }

        // ================= SCALE =================

        void UpdateScale(Vector3 screenPos, bool isBehind)
        {
            float targetScale = isBehind ? 0.8f : 1f;

            if (scaleByDistance && screenPos.z > 0)
            {
                float distanceScale = Mathf.Clamp(
                    scaleMultiplier / screenPos.z,
                    scaleClamp.x,
                    scaleClamp.y
                );

                targetScale *= distanceScale;
            }

            float currentScale = Mathf.SmoothDamp(
                uiElement.localScale.x,
                targetScale,
                ref scaleVelocity,
                scaleSmoothTime
            );

            uiElement.localScale = Vector3.one * currentScale;
        }

        // ================= WORLD POSITION =================

        Vector3 GetWorldPosition()
        {
            switch (followMode)
            {
                case FollowMode.Transform:
                    if (targetTransform != null)
                        return targetTransform.position + worldOffset;
                    break;

                case FollowMode.WorldPosition:
                    return worldPosition + worldOffset;

                case FollowMode.LocalPoint:
                    if (targetTransform != null)
                        return targetTransform.TransformPoint(localPoint + worldOffset);
                    break;
            }

            return Vector3.zero;
        }

        // ================= API =================

        public void SetTarget(Transform t)
        {
            followMode = FollowMode.Transform;
            targetTransform = t;
        }

        public void SetWorldPosition(Vector3 pos)
        {
            followMode = FollowMode.WorldPosition;
            worldPosition = pos;
        }

        public void SetLocalPoint(Transform t, Vector3 localPos)
        {
            followMode = FollowMode.LocalPoint;
            targetTransform = t;
            localPoint = localPos;
        }
    }
}