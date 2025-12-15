using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    [Header("Round Settings")]
    public int roundsToWin = 2;          // Best of 3 by default
    public float endDelay = 1.5f;
    public float betweenRoundDelay = 1.5f;

    private int p1RoundsWon = 0;
    private int p2RoundsWon = 0;

    private bool roundOver = false;

    private PlayerHealth player1;
    private PlayerHealth player2;

    private Vector3 p1StartPos;
    private Vector3 p2StartPos;

    private int currentRound = 1;
private RoundUI roundUI;

    private RoundWinUI winUI;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Called by FightSceneController AFTER spawning players
    public void RegisterPlayers(PlayerHealth p1, PlayerHealth p2)
    {
        player1 = p1;
        player2 = p2;

        p1StartPos = p1.transform.position;
        p2StartPos = p2.transform.position;

        roundUI = FindObjectOfType<RoundUI>(true);

if (roundUI != null)
{
    roundUI.ShowRound(currentRound);
}

winUI = FindObjectOfType<RoundWinUI>(true);

if (winUI != null)
{
    winUI.ResetMarkers();
}

    }

    public void OnPlayerDeath(PlayerHealth deadPlayer)
    {
        if (roundOver) return;
        roundOver = true;

        // Decide winner
        if (deadPlayer == player1)
            p2RoundsWon++;
        else
            p1RoundsWon++;

        Debug.Log($"Round over | P1: {p1RoundsWon} - P2: {p2RoundsWon}");

        Time.timeScale = 0.5f;

        if (p1RoundsWon >= roundsToWin || p2RoundsWon >= roundsToWin)
        {
            Invoke(nameof(EndMatch), endDelay);
        }
        else
        {
            Invoke(nameof(StartNextRound), endDelay);
        }

        if (winUI != null)
{
    winUI.UpdateWins(p1RoundsWon, p2RoundsWon);
}
    }

    private void StartNextRound()
    {
        currentRound++;

if (roundUI != null)
{
    roundUI.ShowRound(currentRound);
}
        Time.timeScale = 1f;

        ResetPlayer(player1, p1StartPos);
        ResetPlayer(player2, p2StartPos);

        roundOver = false;

        Debug.Log("Next round start");
    }

    private void ResetPlayer(PlayerHealth player, Vector3 startPos)
    {
        player.transform.position = startPos;

        player.ResetHealth(); // we’ll add this (small, safe)
        player.ResetState();  // optional but recommended
    }

    private void EndMatch()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ResultScene");
    }
}
