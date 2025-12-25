using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnPvPPressed()
    {
        MatchConfig.gameMode = GameMode.PvP;
        SceneManager.LoadScene("SelectCharacterScene");
    }

    public void OnPvCPUPressed()
    {
        MatchConfig.gameMode = GameMode.PvCPU;
        SceneManager.LoadScene("SelectCharacterScene");
    }

    public void OnInstructionPressed()
    {
        SceneManager.LoadScene("InstructionScene");
    }
}