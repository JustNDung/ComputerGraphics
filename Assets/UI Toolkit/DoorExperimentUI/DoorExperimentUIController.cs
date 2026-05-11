using UnityEngine;
using UnityEngine.UIElements;

public class DoorExperimentUIController : MonoBehaviour
{
    // private MarkerPanelController _hingeLogic;
    // private MarkerPanelController _handleLogic;
    //
    // // Thêm biến quản lý Vector lực
    // private ForceVectorController _forceVector;
    //
    // [Header("Dữ liệu Test")]
    // public Vector2 currentForce = new Vector2(50f, 0f); // Cho một giá trị mặc định để dễ test
    // public string hingeStatus = "Ổn định";
    // public string handleStatus = "Sẵn sàng";
    //
    // [Header("Cài đặt Vector UI")]
    // public float forceVisualScale = 1f; // Hệ số tỷ lệ: 1 đơn vị lực = bao nhiêu pixel trên UI
    // public Color forceColor = Color.red; // Chỉnh màu lực trực tiếp từ Inspector
    //
    // private void OnEnable()
    // {
    //     var root = GetComponent<UIDocument>().rootVisualElement;
    //
    //     // Khởi tạo logic cho cụm Bản lề
    //     var hingeEl = root.Q<VisualElement>("HingeInfoPanel");
    //     if (hingeEl != null) _hingeLogic = new MarkerPanelController(hingeEl);
    //
    //     // Khởi tạo logic cho cụm Tay nắm
    //     var handleEl = root.Q<VisualElement>("HandleInfoPanel");
    //     if (handleEl != null) _handleLogic = new MarkerPanelController(handleEl);
    //     
    //     // Khởi tạo logic cho Vector lực
    //     // Chỉ cần truyền root vào, ForceVectorController sẽ tự query ID "force-vector-container"
    //     if (root != null) _forceVector = new ForceVectorController(root);
    // }
    //
    // private void Update()
    // {
    //     _hingeLogic?.UpdateStatus(hingeStatus);
    //     _handleLogic?.UpdateStatus(handleStatus);
    //
    //     // Cập nhật UI Vector lực dựa trên thông số currentForce từ Inspector
    //     _forceVector?.UpdateVectorFromForce(currentForce, forceVisualScale, forceColor);
    //     
    //     // THÊM: Hiển thị độ lớn của lực (Làm tròn 1 chữ số thập phân nếu cần)
    //     float currentMagnitude = Mathf.Round(currentForce.magnitude * 10f) / 10f;
    //     _forceVector?.SetLabelText($"F = {currentMagnitude}");
    // }
}