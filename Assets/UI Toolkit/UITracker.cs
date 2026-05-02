using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic; 

public class UITracker : MonoBehaviour
{
    [System.Serializable]
    public class AnnotationData
    {
        public string labelName;    
        public string lineName;     
        public Transform target;    
        
        [HideInInspector] public VisualElement box;
        [HideInInspector] public VisualElement line;
        [HideInInspector] public float opacity = 0f;
    }

    [Header("Danh sách các chú thích")]
    public List<AnnotationData> annotations = new List<AnnotationData>();

    [Header("Cài đặt chung")]
    public UIDocument uiDoc;
    public float fadeSpeed = 5f;
    public Vector2 offset = new Vector2(150f, -100f); 

    [Header("Cài đặt Đường kẻ (Line)")]
    public Color lineColor = Color.black; // Tùy chỉnh màu sắc
    public float lineThickness = 4f;      // Tùy chỉnh độ dày (size)

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (uiDoc == null || uiDoc.rootVisualElement == null) return;

        var root = uiDoc.rootVisualElement;

        foreach (var item in annotations)
        {
            item.box = root.Q<VisualElement>(item.labelName);
            item.line = root.Q<VisualElement>(item.lineName);

            if (item.box != null) item.box.style.opacity = 0f;
            if (item.line != null) item.line.style.opacity = 0f;
        }
    }

    void LateUpdate()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null || uiDoc == null) return;

        foreach (var item in annotations)
        {
            UpdateSingleAnnotation(item);
        }
    }

    void UpdateSingleAnnotation(AnnotationData item)
    {
        if (item.target == null || item.box == null || item.line == null) return;
        if (item.box.panel == null) return;

        Vector3 camToTarget = item.target.position - cam.transform.position;
        bool isVisible = Vector3.Dot(cam.transform.forward, camToTarget) > 0;

        float targetOpacity = isVisible ? 1f : 0f;
        item.opacity = Mathf.MoveTowards(item.opacity, targetOpacity, Time.deltaTime * fadeSpeed);

        item.box.style.opacity = item.opacity;
        item.line.style.opacity = item.opacity;

        if (item.opacity > 0)
        {
            item.box.style.display = DisplayStyle.Flex;
            item.line.style.display = DisplayStyle.Flex;

            Vector2 startPos = RuntimePanelUtils.CameraTransformWorldToPanel(item.box.panel, item.target.position, cam);
            Vector2 endPos = startPos + offset;

            item.box.style.left = endPos.x;
            item.box.style.top = endPos.y - 20f;

            Vector2 dir = endPos - startPos;
            float distance = dir.magnitude;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            item.line.style.position = Position.Absolute;
            item.line.style.left = startPos.x;
            item.line.style.top = startPos.y;
            item.line.style.width = distance;
            
            // ÉP MÀU VÀ ĐỘ DÀY TỪ BIẾN ĐÃ KHAI BÁO
            item.line.style.height = lineThickness;
            item.line.style.backgroundColor = new StyleColor(lineColor);

            item.line.style.transformOrigin = new StyleTransformOrigin(new TransformOrigin(new Length(0, LengthUnit.Percent), new Length(50, LengthUnit.Percent)));
            item.line.style.rotate = new StyleRotate(new Rotate(new Angle(angle, AngleUnit.Degree)));
        }
        else
        {
            item.box.style.display = DisplayStyle.None;
            item.line.style.display = DisplayStyle.None;
        }
    }
}