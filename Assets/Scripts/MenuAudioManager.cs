using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public static MenuAudioManager Instance;

    public AudioSource audioSource;

    [Header("Menu SFX")]
    public AudioClip moveSFX;
    public AudioClip confirmSFX;
    public AudioClip cancelSFX;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayMove()
    {
        audioSource.PlayOneShot(moveSFX);
    }

    public void PlayConfirm()
    {
        audioSource.PlayOneShot(confirmSFX);
    }

    public void PlayCancel()
    {
        audioSource.PlayOneShot(cancelSFX);
    }
}
