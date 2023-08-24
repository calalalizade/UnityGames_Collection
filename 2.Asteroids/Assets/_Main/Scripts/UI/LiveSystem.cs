using UnityEngine;

public class LiveSystem : MonoBehaviour
{
    public void UpdateUI(int _lives)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < _lives; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
