// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class RoundManager : MonoBehaviour
// {
//     public static RoundManager Instance;

//     private void Awake()
//     {
//         if (Instance == null) Instance = this;
//         else Destroy(gameObject);
//     }

//     public void PlayerDied(PlayerHealth deadPlayer)
//     {
//         Debug.Log(deadPlayer.name + " lost the round!");
//         StartCoroutine(ReloadScene());
//     }

//     private System.Collections.IEnumerator ReloadScene()
//     {
//         yield return new WaitForSeconds(2f);
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }
// }

using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    [Header("Round Settings")]
    public float endDelay = 1.5f;

    private bool roundOver = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnPlayerDeath(PlayerHealth deadPlayer)
    {
        if (roundOver) return;

        roundOver = true;

        // Freeze time slightly for drama
        Time.timeScale = 0.5f;

        Invoke(nameof(ShowResult), endDelay);
    }

    private void ShowResult()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ResultScene");
    }
}
