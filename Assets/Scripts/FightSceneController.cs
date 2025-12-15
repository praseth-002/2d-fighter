using UnityEngine;
using UnityEngine.UI;

public class FightSceneController : MonoBehaviour
{
    [Header("Spawns")]
    public Transform player1Spawn;
    public Transform player2Spawn;

    [Header("Stage (UI Image)")]
    public Image stageBackground;

    private GameObject player1;
    private GameObject player2;

    void Start()
    {
        SpawnStage();
        SpawnPlayers();

        // Sanity logs (remove later)
        Debug.Log("P1: " + MatchConfig.player1Character.characterName);
        Debug.Log("P2: " + MatchConfig.player2Character.characterName);
        Debug.Log("Stage: " + MatchConfig.stage.stageName);
    }

    void SpawnStage()
    {
        if (stageBackground != null && MatchConfig.stage != null)
        {
            stageBackground.sprite = MatchConfig.stage.background;
        }
        else
        {
            Debug.LogWarning("Stage background or MatchConfig.stage is missing");
        }
    }

    void SpawnPlayers()
    {
        // Spawn Player 1
        player1 = Instantiate(
            MatchConfig.player1Character.characterPrefab,
            player1Spawn.position,
            Quaternion.identity
        );

        // Spawn Player 2
        player2 = Instantiate(
            MatchConfig.player2Character.characterPrefab,
            player2Spawn.position,
            Quaternion.identity
        );

        SetupPlayer(player1, isPlayer2: false);
        SetupPlayer(player2, isPlayer2: true);

        FaceEachOther(player1, player2);

        PlayerHealthUI healthUI = FindObjectOfType<PlayerHealthUI>();
        if (healthUI != null)
        {
            healthUI.BindPlayers(player1, player2);
        }
        Debug.Log("HealthUI bound to players");

    }

    void SetupPlayer(GameObject player, bool isPlayer2)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.Initialize(isPlayer2);
            movement.opponent = isPlayer2 ? player1.transform : player2.transform;
        }
    }

    void FaceEachOther(GameObject p1, GameObject p2)
    {
        Vector3 scale1 = p1.transform.localScale;
        Vector3 scale2 = p2.transform.localScale;

        // Player 1 faces right
        scale1.x = Mathf.Abs(scale1.x);
        p1.transform.localScale = scale1;

        // Player 2 faces left
        scale2.x = -Mathf.Abs(scale2.x);
        p2.transform.localScale = scale2;
    }


}
