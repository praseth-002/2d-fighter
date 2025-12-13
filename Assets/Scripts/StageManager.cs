using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public Image backgroundPreview;
    public Sprite[] stageBackgrounds;

    private void Start()
{
    int selectedStage = PlayerPrefs.GetInt("SelectedStage", 0); // default 0
    backgroundPreview.sprite = stageBackgrounds[selectedStage];
}


    public void SelectStage(int index)
    {
        backgroundPreview.sprite = stageBackgrounds[index];
        PlayerPrefs.SetInt("SelectedStage", index);
    }
}
