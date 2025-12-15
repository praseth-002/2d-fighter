using UnityEngine;
using UnityEngine.UI;

public class RoundWinUI : MonoBehaviour
{
    [Header("Markers")]
    public Image[] p1Markers;
    public Image[] p2Markers;

    [Header("Colors")]
    public Color inactiveColor = Color.red;
    public Color activeColor = Color.green;

    private void Start()
    {
        ResetMarkers();
    }

    public void UpdateWins(int p1Wins, int p2Wins)
    {
        for (int i = 0; i < p1Markers.Length; i++)
            p1Markers[i].color = i < p1Wins ? activeColor : inactiveColor;

        for (int i = 0; i < p2Markers.Length; i++)
            p2Markers[i].color = i < p2Wins ? activeColor : inactiveColor;
    }

    public void ResetMarkers()
    {
        UpdateWins(0, 0);
    }
}
