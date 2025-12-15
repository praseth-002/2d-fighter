// using UnityEngine;
// using TMPro;

// public class RoundUI : MonoBehaviour
// {
//     public TextMeshProUGUI roundText;
//     public float displayDuration = 1.2f;

//     public void ShowRound(int roundNumber)
//     {
//         roundText.text = $"ROUND {roundNumber}";
//         gameObject.SetActive(true);

//         CancelInvoke();
//         Invoke(nameof(Hide), displayDuration);
//     }

//     private void Hide()
//     {
//         gameObject.SetActive(false);
//     }
// }

using UnityEngine;
using TMPro;

public class RoundUI : MonoBehaviour
{
    public TextMeshProUGUI roundText;
    public float displayDuration = 1.2f;

    public void ShowRound(int roundNumber)
    {
        if (roundNumber >= 3)
            roundText.text = "FINAL ROUND";
        else
            roundText.text = $"ROUND {roundNumber}";

        gameObject.SetActive(true);

        CancelInvoke();
        Invoke(nameof(Hide), displayDuration);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
