using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 pos = transform.position;
            pos.z = -10;
            cam.transform.position = pos;
        }
    }
}
