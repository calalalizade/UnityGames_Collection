using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] float destroyAfter;

    private void OnEnable()
    {
        Destroy(gameObject, destroyAfter);
    }

}
