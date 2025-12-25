using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    void Awake()
    {
        // If there's already an instance, destroy this one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Keep this object between scenes
    }
}
