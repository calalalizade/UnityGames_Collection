using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTween : MonoBehaviour
{
    [SerializeField] float tweenTime;

    void Start()
    {
        // Tween();
    }

    public void Tween()
    {
        LeanTween.cancel(gameObject);

        transform.localScale = Vector3.one;

        LeanTween.scale(gameObject, Vector3.one * 1.2f, tweenTime).setEasePunch();
    }
}
