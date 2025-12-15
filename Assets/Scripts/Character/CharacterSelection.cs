using UnityEngine;
using System.Collections.Generic;

public class CharacterSelection : MonoBehaviour
{
    public RectTransform highlightFrame;
    public RectTransform gridParent;
    public int columns = 4; // Number of columns in your grid

    private List<RectTransform> slots = new List<RectTransform>();
    private int currentIndex = 0;

    void Start()
    {
        // Populate flat list of slots
        for (int i = 0; i < gridParent.childCount; i++)
            slots.Add(gridParent.GetChild(i).GetComponent<RectTransform>());

        // Set initial highlight position
        highlightFrame.position = slots[currentIndex].position;
    }

    void Update()
    {
        int total = slots.Count;

        // Left/Right
        if (Input.GetKeyDown(KeyCode.RightArrow))
            currentIndex = Mathf.Min(currentIndex + 1, total - 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentIndex = Mathf.Max(currentIndex - 1, 0);

        // Up/Down
        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentIndex = Mathf.Max(currentIndex - columns, 0);
            // currentIndex -= columns;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentIndex = Mathf.Min(currentIndex + columns, total - 1);
            // currentIndex += columns;

        // Move highlight frame
        highlightFrame.position = slots[currentIndex].position;

        // Select
        if (Input.GetKeyDown(KeyCode.Return))
            Debug.Log("Selected: " + currentIndex + " -> " + slots[currentIndex].name);
    }
}
