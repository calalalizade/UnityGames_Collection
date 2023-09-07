using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [HideInInspector]
    public enum GameOutcome
    {
        None,
        Win,
        Lose,
        Draw
    }

    private const int SIZE = 3;
    public int[,] grid = new int[3, 3];

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.GetInstance();
    }

    public void ResetGrid()
    {
        for (int row = 0; row < SIZE; row++)
        {
            for (int col = 0; col < SIZE; col++)
            {
                grid[row, col] = 0;
            }
        }
    }

    public bool IsCellOccupied(int row, int col)
    {
        return grid[row, col] != 0;
    }

    public void UpdateGrid(int row, int col, int move)
    {
        grid[row, col] = move;
    }

    public GameOutcome CheckGameOutcome(int[,] currentGrid)
    {
        // Check rows and columns for a win
        for (int i = 0; i < 3; i++)
        {
            // Check rows
            if (currentGrid[i, 0] == currentGrid[i, 1] && currentGrid[i, 1] == currentGrid[i, 2])
            {
                if (currentGrid[i, 0] == 2)
                    return GameOutcome.Lose;
                else if (currentGrid[i, 0] == 1)
                    return GameOutcome.Win;
            }

            // Check columns
            if (currentGrid[0, i] == currentGrid[1, i] && currentGrid[1, i] == currentGrid[2, i])
            {
                if (currentGrid[0, i] == 2)
                    return GameOutcome.Lose;
                else if (currentGrid[0, i] == 1)
                    return GameOutcome.Win;
            }
        }

        // Check diagonals
        if (currentGrid[0, 0] == currentGrid[1, 1] && currentGrid[1, 1] == currentGrid[2, 2])
        {
            if (currentGrid[0, 0] == 2)
                return GameOutcome.Lose;
            else if (currentGrid[0, 0] == 1)
                return GameOutcome.Win;
        }
        if (currentGrid[0, 2] == currentGrid[1, 1] && currentGrid[1, 1] == currentGrid[2, 0])
        {
            if (currentGrid[0, 2] == 2)
                return GameOutcome.Lose;
            else if (currentGrid[0, 2] == 1)
                return GameOutcome.Win;
        }

        // Check for a draw
        bool isGridFull = true;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (currentGrid[x, y] == 0)
                {
                    isGridFull = false;
                    break;
                }
            }
        }

        if (isGridFull)
            return GameOutcome.Draw;

        // If none of the above conditions are met, return "None"
        return GameOutcome.None;
    }

    #region Unbeatable Bot (Minimax Algorithm)
    private int alpha = int.MinValue;
    private int beta = int.MaxValue;

    public void MakeAIMove()
    {
        int bestScore = int.MinValue;
        int bestMoveX = -1;
        int bestMoveY = -1;

        // Loop through all empty cells
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (grid[x, y] == 0)
                {
                    // Try placing "O" in the empty cell
                    grid[x, y] = 2;

                    // Calculate the score for this move using the Minimax algorithm
                    int score = Minimax(grid, 0, false, alpha, beta);

                    // Undo the move
                    grid[x, y] = 0;

                    // If this move has a higher score, update the best move
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMoveX = x;
                        bestMoveY = y;
                    }

                    alpha = Mathf.Max(alpha, bestScore);
                    if (beta <= alpha)
                        break;
                }
            }
        }

        // Make the best move
        if (bestMoveX != -1 && bestMoveY != -1)
        {
            grid[bestMoveX, bestMoveY] = 2;
            CheckGameOutcome(grid);
            int index = bestMoveX * 3 + bestMoveY;
            gameManager.UpdateCellSprite(index);
        }
    }

    // Implement the Minimax recursive function
    private int Minimax(int[,] currentGrid, int depth, bool isMaximizing, int alpha, int beta)
    {
        GameOutcome outcome = CheckGameOutcome(currentGrid);

        if (outcome != GameOutcome.None)
        {
            if (outcome == GameOutcome.Win)
                return -1;
            else if (outcome == GameOutcome.Lose)
                return 1;
            else
                return 0;
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentGrid[x, y] == 0)
                    {
                        currentGrid[x, y] = 2;
                        int score = Minimax(currentGrid, depth + 1, false, alpha, beta);
                        currentGrid[x, y] = 0;
                        bestScore = Mathf.Max(score, bestScore);

                        alpha = Mathf.Max(alpha, bestScore);
                        if (beta <= alpha)
                            break;
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentGrid[x, y] == 0)
                    {
                        currentGrid[x, y] = 1; // Assuming player is "X"
                        int score = Minimax(currentGrid, depth + 1, true, alpha, beta);
                        currentGrid[x, y] = 0;
                        bestScore = Mathf.Min(score, bestScore);

                        beta = Mathf.Min(beta, bestScore);
                        if (beta <= alpha)
                            break;
                    }
                }
            }
            return bestScore;
        }
    }
    #endregion
}
