using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounce : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction = Vector3.zero;
    private CameraController cameraController;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (Camera.main != null)
            {
                cameraController.ShakeCamera(direction);
            }
        }
    }
}
