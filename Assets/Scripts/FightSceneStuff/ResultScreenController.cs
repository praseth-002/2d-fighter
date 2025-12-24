using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultScreenController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI resultText;

    [Header("Buttons")]
    public RectTransform highlight;
    public RectTransform[] options; // Menu, Reselect, Rematch

    private int currentIndex = 0;

    void Start()
    {
        SetupResultText();
        MoveHighlight();
    }

    void Update()
    {
        // Navigation
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(1);
            MenuAudioManager.Instance.PlayMove();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(-1);
            MenuAudioManager.Instance.PlayMove();
        }

        // Confirm
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MenuAudioManager.Instance.PlayConfirm();
            Confirm();
        }
    }

    void SetupResultText()
    {
        if (MatchConfig.matchWinner == MatchWinner.Player1)
            resultText.text = "PLAYER 1 WINS";
        else
            resultText.text = "PLAYER 2 WINS";
    }

    void Move(int dir)
    {
        currentIndex = Mathf.Clamp(currentIndex + dir, 0, options.Length - 1);
        MoveHighlight();
    }

    void MoveHighlight()
    {
        highlight.position = options[currentIndex].position;
    }

    void Confirm()
    {
        switch (currentIndex)
        {
            case 0: // Rematch
                SceneManager.LoadScene("FightScene");
                break;
            case 1: // Reselect
                SceneManager.LoadScene("SelectCharacterScene");
                break;
            case 2: // Main Menu
                SceneManager.LoadScene("MainMenu");
                break;


        }
    }
}
