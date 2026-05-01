namespace UI
{
    using UnityEngine;
    using DG.Tweening;

    public class ForcePointIndicator : MonoBehaviour
    {
        public Camera cam;
        public SpriteRenderer sr;

        [Header("Effect")]
        public float rotateSpeed = 35f;
        public float pulseScale = 1.15f;
        public float pulseDuration = 0.8f;

        [Header("Distance Show")]
        public Transform player;
        public float showDistance = 3f;

        Vector3 startScale;
        Color baseColor;

        void Start()
        {
            if (cam == null) cam = Camera.main;
            if (sr == null) sr = GetComponent<SpriteRenderer>();

            startScale = transform.localScale;
            baseColor = sr.color;

            transform.DOScale(startScale * pulseScale, pulseDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        void Update()
        {
            Billboard();
            RotateRing();
            DistanceFade();
        }

        void Billboard()
        {
            if (cam == null) return;

            transform.forward = cam.transform.forward;
        }

        void RotateRing()
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }

        void DistanceFade()
        {
            if (player == null) return;

            float d = Vector3.Distance(player.position, transform.position);

            float alpha = d <= showDistance ? 1f : 0f;

            Color c = sr.color;
            c.a = Mathf.Lerp(c.a, alpha, Time.deltaTime * 6f);
            sr.color = c;
        }
    }
}