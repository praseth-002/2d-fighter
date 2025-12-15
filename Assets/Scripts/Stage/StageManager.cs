using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("UI")]
    public Image backgroundPreview;

    [Header("Stages")]
    public StageData[] stages;

    private int currentIndex = 0;

    void Start()
    {
        if (stages == null || stages.Length == 0)
        {
            Debug.LogError("StageManager: No stages assigned!");
            return;
        }

        UpdatePreview(0);
    }

    public void SelectStage(int index)
    {
        if (index < 0 || index >= stages.Length)
        {
            Debug.LogError("StageManager: Invalid stage index " + index);
            return;
        }

        currentIndex = index;
        UpdatePreview(index);
    }

    void UpdatePreview(int index)
    {
        backgroundPreview.sprite = stages[index].background;
    }

    public void ConfirmStage()
    {
        if (stages == null || stages.Length == 0)
        {
            Debug.LogError("StageManager: Cannot confirm stage, none assigned.");
            return;
        }

        MatchConfig.stage = stages[currentIndex];
        Debug.Log("Stage selected: " + MatchConfig.stage.stageName);

        SceneManager.LoadScene("FightScene");
    }
}
