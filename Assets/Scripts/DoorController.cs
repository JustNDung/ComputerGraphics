using UnityEngine;
using UnityEngine.InputSystem; // nếu bạn dùng Input System mới

public class DoorController : MonoBehaviour
{
    private Animator _animator;

    private bool _isPlayerInside = false;
    private bool _isOpen = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Nhấn E để mở cửa khi đang ở trong vùng
        if (_isPlayerInside && !_isOpen && Keyboard.current.fKey.wasPressedThisFrame)
        {
            OpenDoor();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        _isOpen = true;
        _animator.SetBool("isOpen", true);
    }

    void CloseDoor()
    {
        _isOpen = false;
        _animator.SetBool("isOpen", false);
    }
}