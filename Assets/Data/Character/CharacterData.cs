using UnityEngine;

[CreateAssetMenu(menuName = "Fighter/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public GameObject characterPrefab;
}