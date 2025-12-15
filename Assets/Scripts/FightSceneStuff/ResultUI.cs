using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public void Rematch()
    {
        SceneManager.LoadScene("FightScene");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
