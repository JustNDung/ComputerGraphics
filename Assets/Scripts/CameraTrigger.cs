using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    public CinemachineCamera experimentCam;
    public CinemachineCamera exploreCam;

    [Header("Cinematic Settings")]
    public float zoomDelay = 0.25f; // delay trước khi zoom
    public MonoBehaviour playerController; // script điều khiển player

    private bool _isPlayerInside = false;
    private bool _isFocused = false;
    private bool _isTransitioning = false;

    void Update()
    {
        if (_isPlayerInside && !_isTransitioning && Input.GetKeyDown(KeyCode.F))
        {
            if (!_isFocused)
                StartCoroutine(FocusWithDelay());
            else
                ExitCamera();
        }
    }

    IEnumerator FocusWithDelay()
    {
        _isTransitioning = true;

        // 👉 delay nhẹ để tạo cinematic feel
        yield return new WaitForSeconds(zoomDelay);

        // 👉 chuyển camera
        experimentCam.Priority = 20;
        exploreCam.Priority = 10;

        // 👉 khóa player (tuỳ chọn)
        if (playerController != null)
            playerController.enabled = false;

        _isFocused = true;
        _isTransitioning = false;

        Debug.Log("Zoom vào thí nghiệm 🎬");
    }

    void ExitCamera()
    {
        _isTransitioning = true;

        // 👉 trả camera về explore
        experimentCam.Priority = 10;
        exploreCam.Priority = 20;

        // 👉 mở lại player
        if (playerController != null)
            playerController.enabled = true;

        _isFocused = false;

        // delay nhỏ để tránh spam F
        StartCoroutine(ResetTransition());
        
        Debug.Log("Thoát camera 🎮");
    }

    IEnumerator ResetTransition()
    {
        yield return new WaitForSeconds(0.2f);
        _isTransitioning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;

            // TODO: Hiện UI [F] Interact
            Debug.Log("Nhấn F để tương tác");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;

            // Nếu đang focus mà đi ra thì auto thoát
            if (_isFocused)
                ExitCamera();

            // TODO: Ẩn UI
        }
    }
}