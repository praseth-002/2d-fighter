using UnityEngine;
using UnityEngine.UI;

public class MainMenuSelection : MonoBehaviour
{
    [Header("UI")]
    public RectTransform highlight;   // highlight frame or cursor
    public RectTransform[] options;   // button RectTransforms

    [Header("Input")]
    public bool vertical = true;      // true = up/down, false = left/right

    private int currentIndex = 0;

    void Start()
    {
        MoveHighlight();
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
            Button btn = options[currentIndex].GetComponent<Button>();
            if (btn != null)
                btn.onClick.Invoke();
        }
    }

    void Move(int direction)
    {
        currentIndex += direction;
        currentIndex = Mathf.Clamp(currentIndex, 0, options.Length - 1);
        MoveHighlight();
    }

    void MoveHighlight()
    {
        highlight.position = options[currentIndex].position;
    }
}
