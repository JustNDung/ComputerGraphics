using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Header("Cấu hình đòn bẩy")]
    public Transform pivotPoint;        // Điểm tựa O
    public Transform weightO1;          // Vật O1 (trái)
    public Transform weightO2;          // Vật O2 (phải)

    [Header("Lực tác động")]
    public float forceF1 = 10f;         // Lực F1
    public float forceF2 = 5f;          // Lực F2

    [Header("Khoảng cách tay đòn")]
    public float armLength1 = 2.5f;     // d1: O1 đến O
    public float armLength2 = 2.5f;     // d2: O2 đến O

    private Rigidbody rb;
    private bool isBalanced;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CheckBalance();
    }

    void FixedUpdate()
    {
        ApplyTorque();
    }

    void ApplyTorque()
    {
        // Momen lực: M = F × d
        float torque1 = forceF1 * armLength1;  // Momen bên trái (quay xuống)
        float torque2 = forceF2 * armLength2;  // Momen bên phải (quay xuống)

        float netTorque = torque1 - torque2;

        // Áp lực dọc theo trục Z
        rb.AddTorque(Vector3.forward * (-netTorque * 0.1f));
    }

    void CheckBalance()
    {
        float M1 = forceF1 * armLength1;
        float M2 = forceF2 * armLength2;

        isBalanced = Mathf.Approximately(M1, M2);

        // if (isBalanced)
        //     Debug.Log("✅ Đòn bẩy CÂN BẰNG! M1 = M2 = " + M1);
        // else
        //     Debug.Log($"⚖️ Mất cân bằng: M1={M1}, M2={M2}. " +
        //               $"Bên {'(M1>M2 ? "trái" : "phải")'} nặng hơn.");
    }

    // Gọi khi thay đổi thông số trong Inspector
    void OnValidate()
    {
        CheckBalance();
    }

    // Hiển thị debug
    void OnDrawGizmos()
    {
        if (pivotPoint == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pivotPoint.position, 0.1f);
        
        // Vẽ vector lực F1
        Gizmos.color = Color.blue;
        if (weightO1 != null)
            Gizmos.DrawRay(weightO1.position, Vector3.down * forceF1 * 0.1f);
        
        // Vẽ vector lực F2    
        Gizmos.color = Color.green;
        if (weightO2 != null)
            Gizmos.DrawRay(weightO2.position, Vector3.down * forceF2 * 0.1f);
    }

//     // Thêm vào LeverController.cs
// [Header("UI hiển thị")]
// public LineRenderer forceArrow1;
// public LineRenderer forceArrow2;
// public TextMesh balanceText;

// void UpdateVisuals()
// {
//     // Cập nhật mũi tên lực
//     if (forceArrow1 != null)
//     {
//         forceArrow1.SetPosition(0, weightO1.position);
//         forceArrow1.SetPosition(1, weightO1.position + Vector3.down * forceF1 * 0.05f);
//     }
    
//     // Hiển thị trạng thái
//     if (balanceText != null)
//         balanceText.text = isBalanced ? "CÂN BẰNG ✓" : "MẤT CÂN BẰNG";
// }
}

