using UnityEngine;
using System;

public class DragRotateModel : MonoBehaviour
{
    [Header("Camera")]
    public Camera targetCamera;
    public Transform focusTarget;
    public float focusDistance = 3f;
    public float cameraMoveSpeed = 8f;

    [Header("Dark Background")]
    public CanvasGroup darkOverlay;
    public float darkAlpha = 0.55f;
    public float overlayFadeSpeed = 8f;

    [Header("Rotate")]
    public float rotateSpeed = 0.4f;

    private bool isDragging = false;
    private bool isFocusing = false;

    private Vector3 lastMousePosition;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private Vector3 targetCameraPosition;
    private Quaternion targetCameraRotation;

    [Header("Reset Rotation")]
    public bool resetRotationOnRelease = true;
    public float resetRotationSpeed = 6f;

    private Quaternion originalModelRotation;
    private bool isResettingRotation = false;

    [Header("Hide Objects When Focus")]
    public GameObject[] objectsToHideOnFocus;

    private bool[] originalActiveStates;

    void Start()
    {
        
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (focusTarget == null)
        {
            focusTarget = transform;
        }

        if (darkOverlay != null)
        {
            darkOverlay.alpha = 0f;
        }
        originalModelRotation = transform.rotation;

        originalActiveStates = new bool[objectsToHideOnFocus.Length];

        for (int i = 0; i < objectsToHideOnFocus.Length; i++)
        {
            if (objectsToHideOnFocus[i] != null)
            {
                originalActiveStates[i] = objectsToHideOnFocus[i].activeSelf;
            }
        }
    }

    void Update()
    {
        if (targetCamera == null) return;

        HandleMouseInput();
        HandleCameraFocus();
        HandleDarkOverlay();
        HandleResetRotation();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (IsMouseOnThisModel())
            {
                StartFocusMode();

                isDragging = true;
                lastMousePosition = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
            StopFocusMode();

            if (resetRotationOnRelease)
            {
                isResettingRotation = true;
            }
        }

        if (isDragging && Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            float rotateY = -delta.x * rotateSpeed;

            // Chỉ xoay quanh trục Y ở tâm
            transform.Rotate(0f, rotateY, 0f, Space.World);
        }
    }

    void StartFocusMode()
    {
        isFocusing = true;

        SetObjectsToHideVisible(false);

        originalCameraPosition = targetCamera.transform.position;
        originalCameraRotation = targetCamera.transform.rotation;

        Vector3 focusPoint = focusTarget.position;

        Vector3 directionFromModelToCamera =
            (targetCamera.transform.position - focusPoint).normalized;

        targetCameraPosition =
            focusPoint + directionFromModelToCamera * focusDistance;

        targetCameraRotation =
            Quaternion.LookRotation(focusPoint - targetCameraPosition, Vector3.up);
    }

    void StopFocusMode()
    {
        isFocusing = false;
        SetObjectsToHideVisible(true);
    }

    void HandleCameraFocus()
    {
        if (isFocusing)
        {
            targetCamera.transform.position = Vector3.Lerp(
                targetCamera.transform.position,
                targetCameraPosition,
                Time.deltaTime * cameraMoveSpeed
            );

            targetCamera.transform.rotation = Quaternion.Slerp(
                targetCamera.transform.rotation,
                targetCameraRotation,
                Time.deltaTime * cameraMoveSpeed
            );
        }
        else
        {
            targetCamera.transform.position = Vector3.Lerp(
                targetCamera.transform.position,
                originalCameraPosition,
                Time.deltaTime * cameraMoveSpeed
            );

            targetCamera.transform.rotation = Quaternion.Slerp(
                targetCamera.transform.rotation,
                originalCameraRotation,
                Time.deltaTime * cameraMoveSpeed
            );
        }
    }

    void HandleDarkOverlay()
    {
        if (darkOverlay == null) return;

        float targetAlpha = isFocusing ? darkAlpha : 0f;

        darkOverlay.alpha = Mathf.Lerp(
            darkOverlay.alpha,
            targetAlpha,
            Time.deltaTime * overlayFadeSpeed
        );
    }

    bool IsMouseOnThisModel()
    {
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(
            ray,
            1000f,
            ~0,
            QueryTriggerInteraction.Ignore
        );

        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                return true;
            }
        }

        return false;
    }

    void HandleResetRotation()
    {
        if (!isResettingRotation) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            originalModelRotation,
            Time.deltaTime * resetRotationSpeed
        );

        if (Quaternion.Angle(transform.rotation, originalModelRotation) < 0.1f)
        {
            transform.rotation = originalModelRotation;
            isResettingRotation = false;
        }
    }

    void SetObjectsToHideVisible(bool visible)
    {
        for (int i = 0; i < objectsToHideOnFocus.Length; i++)
        {
            if (objectsToHideOnFocus[i] == null) continue;

            if (visible)
            {
                objectsToHideOnFocus[i].SetActive(originalActiveStates[i]);
            }
            else
            {
                objectsToHideOnFocus[i].SetActive(false);
            }
        }
    }
}