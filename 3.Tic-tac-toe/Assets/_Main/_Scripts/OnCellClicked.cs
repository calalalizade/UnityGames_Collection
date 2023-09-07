using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnCellClicked : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int rowIndex;
    [SerializeField] private int colIndex;
    private GameManager gameManager;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        gameManager = GameManager.GetInstance();
    }


    public void OnPointerClick(PointerEventData pointerEventData)
    {
        gameManager.HandlePlayerMove(image, rowIndex, colIndex);
    }
}
