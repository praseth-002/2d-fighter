using UnityEngine;
using UnityEngine.UI;

public class StageSelectionNavigation : MonoBehaviour
{
    [Header("References")]
    public StageManager stageManager;

    public RectTransform highlight;          // highlight frame / cursor
    public RectTransform[] stageButtons;     // buttons or images representing stages

    [Header("Layout")]
    public bool vertical = false;            // true = up/down, false = left/right

    private int currentIndex = 0;

    void Start()
    {
        MoveHighlight();
        stageManager.SelectStage(currentIndex);
    }

    void Update()
    {
        // Navigation
        if (vertical)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                Move(1);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                Move(-1);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                Move(1);
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                Move(-1);
        }

        // Confirm
        if (Input.GetKeyDown(KeyCode.Return))
        {
            stageManager.ConfirmStage();
        }
    }

    void Move(int direction)
    {
        currentIndex += direction;
        currentIndex = Mathf.Clamp(currentIndex, 0, stageButtons.Length - 1);

        MoveHighlight();
        stageManager.SelectStage(currentIndex);
    }

    void MoveHighlight()
    {
        highlight.position = stageButtons[currentIndex].position;
    }
}
