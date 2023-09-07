using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    #region Outcome Text Colors
    private Color32 winTextColor = new(40, 32, 90, 255);
    private Color32 loseTextColor = new(249, 36, 100, 255);
    private Color32 drawTextColor = new(40, 32, 90, 160);
    #endregion

    [Header("References")]
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private Sprite crossSprite;
    [SerializeField] private Sprite noughtSprite;
    [SerializeField] private GameObject gameOutcomeScreen;
    [SerializeField] private TMP_Text outcomeText;
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private Image[] Cells;

    [HideInInspector] public bool playerTurn = true;

    private bool isGameEnd = false;

    private void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isGameEnd)
        {
            ResetGame();
        }
    }

    public void HandlePlayerMove(Image image, int rowIndex, int colIndex)
    {
        if (!playerTurn || gameBoard.IsCellOccupied(rowIndex, colIndex) || isGameEnd) return;

        image.sprite = crossSprite;
        gameBoard.UpdateGrid(rowIndex, colIndex, 1);

        gameBoard.CheckGameOutcome(gameBoard.grid);
        UpdateOutcome();

        playerTurn = false;
        UpdateTurnUi();
        Invoke(nameof(HandleComputerMove), 1.5f);
    }

    private void HandleComputerMove()
    {
        if (!playerTurn)
        {
            gameBoard.MakeAIMove();
            UpdateOutcome();

            playerTurn = true;
            UpdateTurnUi();
        }
    }

    public void UpdateCellSprite(int index)
    {
        Cells[index].sprite = noughtSprite;
    }

    public void UpdateOutcome()
    {
        switch (gameBoard.CheckGameOutcome(gameBoard.grid))
        {
            case GameBoard.GameOutcome.Win:
                UpdateOutcomeUI("You win!", winTextColor);
                isGameEnd = true;
                break;
            case GameBoard.GameOutcome.Lose:
                UpdateOutcomeUI("You lose!", loseTextColor);
                isGameEnd = true;
                break;
            case GameBoard.GameOutcome.Draw:
                UpdateOutcomeUI("Draw", drawTextColor);
                isGameEnd = true;
                break;
        }
    }

    private void UpdateOutcomeUI(string _outcomeText, Color32 _color)
    {
        turnText.gameObject.SetActive(false);
        gameOutcomeScreen.SetActive(true);

        outcomeText.text = _outcomeText;
        outcomeText.color = _color;
    }

    private void UpdateTurnUi()
    {
        turnText.text = playerTurn ? "Your Turn" : "Please Wait...";
    }

    private void ResetGame()
    {
        isGameEnd = false;
        playerTurn = true;
        gameBoard.ResetGrid();
        gameOutcomeScreen.SetActive(false);
        turnText.gameObject.SetActive(true);
        UpdateTurnUi();
        foreach (Image cell in Cells)
        {
            cell.sprite = null;
        }
        StopAllCoroutines();
    }
}
