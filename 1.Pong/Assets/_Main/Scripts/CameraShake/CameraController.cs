using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float duration = 0.15f;
    [SerializeField] float initialShakeAmount = 0.05f;
    [SerializeField] float maxShakeAmount = 0.2f;

    private float currentShakeAmount = 0.0f;
    private Transform cameraTransform;
    private Vector3 originalPosition;
    private Coroutine cameraMoveCoroutine;

    private void Awake()
    {
        currentShakeAmount = initialShakeAmount;
        cameraTransform = GetComponent<Transform>();
        originalPosition = cameraTransform.position;
    }

    public void ShakeCamera(Vector3 direction)
    {
        if (cameraMoveCoroutine != null)
        {
            StopCoroutine(cameraMoveCoroutine);
        }

        cameraMoveCoroutine = StartCoroutine(MoveCamera(direction));
    }

    public void IncreaseIntensity(float _amount)
    {
        // Calculate the new shake amount by adding the scaled _amount to the initial value.
        // Dividing by 50 is used to control the scaling factor.
        currentShakeAmount = initialShakeAmount + _amount / 50;

        // Bound shake amount within limits
        currentShakeAmount = Mathf.Clamp(currentShakeAmount, initialShakeAmount, maxShakeAmount);
    }

    private IEnumerator MoveCamera(Vector3 direction)
    {
        float elapsed = 0f;

        Vector3 targetPosition = originalPosition + direction * currentShakeAmount;

        // Move camera towards the target position over the specified duration.
        while (elapsed < duration)
        {
            cameraTransform.position = Vector3.Lerp(originalPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Return the camera to the original position.
        while (elapsed < duration)
        {
            cameraTransform.position = Vector3.Lerp(targetPosition, originalPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera is back at the original position.
        cameraTransform.position = originalPosition;
    }
}
