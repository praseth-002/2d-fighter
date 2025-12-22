using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [Header("UI")]
    public RectTransform highlightFrame;
    public RectTransform[] characterIcons;   // images / buttons in order

    [Header("Characters")]
    public CharacterData[] characters;

    [Header("Preview Anchors (WORLD SPACE)")]
    public Transform previewAnchorP1;
    public Transform previewAnchorP2;

    private int currentIndex = 0;
    private int lastPreviewIndex = -1;
    private bool selectingPlayer1 = true;

    private GameObject previewP1;
    private GameObject previewP2;

    void Start()
    {
        if (characterIcons.Length == 0 || characters.Length == 0)
        {
            Debug.LogError("CharacterSelection: Missing icons or characters!");
            return;
        }

        MoveHighlight();
        UpdatePreview(currentIndex);
        lastPreviewIndex = currentIndex;
    }

    void Update()
    {
        bool moved = false;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = Mathf.Min(currentIndex + 1, characterIcons.Length - 1);
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = Mathf.Max(currentIndex - 1, 0);
            moved = true;
        }

        if (moved)
        {
            MoveHighlight();
        }

        if (currentIndex != lastPreviewIndex)
        {
            UpdatePreview(currentIndex);
            lastPreviewIndex = currentIndex;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSelection();
        }
    }

    // ================= UI =================

    void MoveHighlight()
    {
        highlightFrame.position = characterIcons[currentIndex].position;
    }

    // ================= SELECTION =================

    void ConfirmSelection()
    {
        CharacterData selected = characters[currentIndex];

        if (selectingPlayer1)
        {
            MatchConfig.player1Character = selected;
            selectingPlayer1 = false;

            Debug.Log("P1 selected: " + selected.characterName);

            // if (MatchConfig.gameMode == GameMode.PvCPU)
            // {
            //     MatchConfig.player2Character = characters[0];
            //     SpawnPreviewP2(MatchConfig.player2Character);

            //     Debug.Log("CPU auto-selected: " + MatchConfig.player2Character.characterName);
            //     SceneManager.LoadScene("SelectStage");
            // }
            if (MatchConfig.gameMode == GameMode.PvCPU)
            {
                int cpuIndex;

                do
                {
                    cpuIndex = Random.Range(0, characters.Length);
                }
                while (characters[cpuIndex] == MatchConfig.player1Character);

                MatchConfig.player2Character = characters[cpuIndex];
                SpawnPreviewP2(MatchConfig.player2Character);

                Debug.Log("CPU auto-selected: " + MatchConfig.player2Character.characterName);
                SceneManager.LoadScene("SelectStage");
            }

        }
        else
        {
            MatchConfig.player2Character = selected;
            Debug.Log("P2 selected: " + selected.characterName);
            SceneManager.LoadScene("SelectStage");
        }
    }

    // ================= PREVIEW =================

    void UpdatePreview(int index)
    {
        if (selectingPlayer1)
            SpawnPreviewP1(characters[index]);
        else
            SpawnPreviewP2(characters[index]);
    }

    void SpawnPreviewP1(CharacterData data)
    {
        if (previewP1 != null)
            Destroy(previewP1);

        previewP1 = Instantiate(
            data.characterPrefab,
            previewAnchorP1.position,
            Quaternion.identity
        );

        DisableGameplay(previewP1);
        ForceIdle(previewP1);
    }

    void SpawnPreviewP2(CharacterData data)
    {
        if (previewP2 != null)
            Destroy(previewP2);

        previewP2 = Instantiate(
            data.characterPrefab,
            previewAnchorP2.position,
            Quaternion.identity
        );

        DisableGameplay(previewP2);
        ForceIdle(previewP2);
    }

    // ================= UTIL =================

    void DisableGameplay(GameObject preview)
    {
        PlayerMovement pm = preview.GetComponent<PlayerMovement>();
        if (pm != null)
            pm.enabled = false;

        Rigidbody2D rb = preview.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.simulated = false;
    }

    void ForceIdle(GameObject preview)
    {
        Animator anim = preview.GetComponentInChildren<Animator>();
        if (anim == null) return;

        anim.Play("PlayerIdle", 0, 0f);
        anim.Update(0f);
    }
}


// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections.Generic;

// public class CharacterSelection : MonoBehaviour
// {
//     [Header("Grid Navigation")]
//     public RectTransform highlightFrame;
//     public RectTransform gridParent;
//     public int columns = 4;

//     [Header("Characters")]
//     public CharacterData[] characters;

//     [Header("Preview Anchors (WORLD SPACE)")]
//     public Transform previewAnchorP1;
//     public Transform previewAnchorP2;

//     private List<RectTransform> slots = new List<RectTransform>();
//     private int currentIndex = 0;
//     private int lastPreviewIndex = -1;

//     private bool selectingPlayer1 = true;

//     private GameObject previewP1;
//     private GameObject previewP2;

//     void Start()
//     {
//         // Cache grid slots
//         for (int i = 0; i < gridParent.childCount; i++)
//         {
//             RectTransform rt = gridParent.GetChild(i).GetComponent<RectTransform>();
//             if (rt != null)
//                 slots.Add(rt);
//         }

//         // Initial highlight + preview
//         highlightFrame.position = slots[currentIndex].position;
//         // highlightFrame.anchoredPosition = slots[currentIndex].anchoredPosition;

//         UpdatePreview(currentIndex);
//         lastPreviewIndex = currentIndex;
//     }

//     void Update()
//     {
//         int total = slots.Count;
//         bool moved = false;

//         // Horizontal
//         if (Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             currentIndex = Mathf.Min(currentIndex + 1, total - 1);
//             moved = true;
//         }
//         if (Input.GetKeyDown(KeyCode.LeftArrow))
//         {
//             currentIndex = Mathf.Max(currentIndex - 1, 0);
//             moved = true;
//         }

//         // Vertical
//         if (Input.GetKeyDown(KeyCode.UpArrow))
//         {
//             currentIndex = Mathf.Max(currentIndex - columns, 0);
//             moved = true;
//         }
//         if (Input.GetKeyDown(KeyCode.DownArrow))
//         {
//             currentIndex = Mathf.Min(currentIndex + columns, total - 1);
//             moved = true;
//         }

//         // Move highlight
//         if (moved)
//             highlightFrame.position = slots[currentIndex].position;
//             // highlightFrame.anchoredPosition = slots[currentIndex].anchoredPosition;


//         // Update preview ONLY when index changes
//         if (currentIndex != lastPreviewIndex)
//         {
//             UpdatePreview(currentIndex);
//             lastPreviewIndex = currentIndex;
//         }

//         // Confirm
//         if (Input.GetKeyDown(KeyCode.Return))
//         {
//             ConfirmSelection();
//         }
//     }

//     // ================= SELECTION =================

//     void ConfirmSelection()
//     {
//         CharacterData selected = characters[currentIndex];

//         if (selectingPlayer1)
//         {
//             MatchConfig.player1Character = selected;
//             selectingPlayer1 = false;

//             Debug.Log("P1 selected: " + selected.characterName);

//             // PvCPU: auto pick CPU + preview
//             if (MatchConfig.gameMode == GameMode.PvCPU)
//             {
//                 MatchConfig.player2Character = characters[0];
//                 Debug.Log("CPU auto-selected: " + MatchConfig.player2Character.characterName);

//                 SpawnPreviewP2(MatchConfig.player2Character);
//                 SceneManager.LoadScene("SelectStage");
//             }
//         }
//         else
//         {
//             MatchConfig.player2Character = selected;
//             Debug.Log("P2 selected: " + selected.characterName);
//             SceneManager.LoadScene("SelectStage");
//         }
//     }

//     // ================= PREVIEW =================

//     void UpdatePreview(int index)
//     {
//         CharacterData data = characters[index];

//         if (selectingPlayer1)
//         {
//             SpawnPreviewP1(data);
//         }
//         else
//         {
//             SpawnPreviewP2(data);
//         }
//     }

//     void SpawnPreviewP1(CharacterData data)
//     {
//         if (previewP1 != null)
//             Destroy(previewP1);

//         previewP1 = Instantiate(
//             data.characterPrefab,
//             previewAnchorP1.position,
//             Quaternion.identity
//         );

//         DisableGameplay(previewP1);
//         ForceIdle(previewP1);
//     }

//     void SpawnPreviewP2(CharacterData data)
//     {
//         if (previewP2 != null)
//             Destroy(previewP2);

//         previewP2 = Instantiate(
//             data.characterPrefab,
//             previewAnchorP2.position,
//             Quaternion.identity
//         );

//         DisableGameplay(previewP2);
//         ForceIdle(previewP2);
//     }

//     // ================= UTIL =================

//     void DisableGameplay(GameObject preview)
//     {
//         // Disable player input / movement
//         PlayerMovement pm = preview.GetComponent<PlayerMovement>();
//         if (pm != null)
//             pm.enabled = false;

//         // Disable physics
//         Rigidbody2D rb = preview.GetComponent<Rigidbody2D>();
//         if (rb != null)
//             rb.simulated = false;

//         // DO NOT disable Animator
//     }

//     void ForceIdle(GameObject preview)
//     {
//         Animator anim = preview.GetComponentInChildren<Animator>();
//         if (anim == null) return;

//         // Explicit naming convention
//         anim.Play("PlayerIdle", 0, 0f);
//         anim.Update(0f);
//     }
// }
