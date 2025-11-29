using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class ClickManager : MonoBehaviour
{
    private Camera mainCam;
    public static Vector3 LastClickWorldPos;

    private void OnEnable()
    {
        mainCam = Camera.main;

        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
                HandleClick(Mouse.current.position.ReadValue());

            if (Mouse.current.leftButton.wasReleasedThisFrame)
                HandleRelease(Mouse.current.position.ReadValue());
        }
    }

    private void OnFingerDown(Finger finger)
    {
        HandleClick(finger.screenPosition);
    }

    private void OnFingerUp(Finger finger)
    {
        HandleRelease(finger.screenPosition);
    }

    private void HandleClick(Vector2 screenPos)
    {
        Ray ray = mainCam.ScreenPointToRay(screenPos);
        LastClickWorldPos = ray.origin + ray.direction * 10f; // arbitrary depth

        // --- 3D COLLIDERS ---
        if (Physics.Raycast(ray, out RaycastHit hit3D))
        {
            var clickable = hit3D.collider.GetComponent<ClickableObject>();
            if (clickable != null)
                clickable.HandleClick(clickable.gameObject);
        }

        // --- 2D COLLIDERS ---
        Vector3 worldPos2D = mainCam.ScreenToWorldPoint(screenPos);
        Collider2D[] hits2D = Physics2D.OverlapPointAll(worldPos2D);
        foreach (var col in hits2D)
        {
            var clickable = col.GetComponent<ClickableObject>();
            if (clickable != null)
                clickable.HandleClick(clickable.gameObject);
        }
    }

    private void HandleRelease(Vector2 screenPos)
    {
        Ray ray = mainCam.ScreenPointToRay(screenPos);

        // --- 3D COLLIDERS ---
        if (Physics.Raycast(ray, out RaycastHit hit3D))
        {
            var clickable = hit3D.collider.GetComponent<ClickableObject>();
            if (clickable != null)
                clickable.HandleRelease();
        }

        // --- 2D COLLIDERS ---
        Vector3 worldPos2D = mainCam.ScreenToWorldPoint(screenPos);
        Collider2D[] hits2D = Physics2D.OverlapPointAll(worldPos2D);
        foreach (var col in hits2D)
        {
            var clickable = col.GetComponent<ClickableObject>();
            if (clickable != null)
                clickable.HandleRelease();
        }
    }
}
