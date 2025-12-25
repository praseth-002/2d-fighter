using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MenuAudioManager.Instance.PlayConfirm();
            SceneManager.LoadScene("MainMenu");

        }
    }
}
