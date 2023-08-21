using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] BallBehavior ball;
    [SerializeField] PlayerController player;
    [SerializeField] AiController computer;
    [SerializeField] TMP_Text playerScoreText;
    [SerializeField] TMP_Text computerScoreText;
    [SerializeField] GameObject startMenu;

    private int playerScore = 0;
    private int computerScore = 0;

    void Start()
    {
        Time.timeScale = 0;
    }

    public void PlayerScoresPoint()
    {
        playerScore++;
        playerScoreText.text = playerScore.ToString();
        playerScoreText.GetComponent<ScoreTween>().Tween();

        StartNewRound();
    }

    public void ComputerScoresPoint()
    {
        computerScore++;
        computerScoreText.text = computerScore.ToString();
        computerScoreText.GetComponent<ScoreTween>().Tween();


        StartNewRound();
    }

    private void StartNewRound()
    {
        ball.ResetBall();
        player.ResetPaddle();
        computer.ResetPaddle();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        startMenu.SetActive(false);
    }
}
