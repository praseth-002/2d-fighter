using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayerDied(PlayerHealth deadPlayer)
    {
        Debug.Log(deadPlayer.name + " lost the round!");
        StartCoroutine(ReloadScene());
    }

    private System.Collections.IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
