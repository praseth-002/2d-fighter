using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public enum SoundType
    {
        Spawn,   // 0
        Punch,   // 1
        Kick,    // 2
        Jump,    // 3
        Dash,    // 4
        Hurt,    // 5
        Death,    // 6
        Block
    }

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Clips")]
    public AudioClip spawn;
    public AudioClip punch;
    public AudioClip kick;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip hurt;
    public AudioClip death;
    public AudioClip block;

    void Awake()
    {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Play once when character spawns
        PlaySound(SoundType.Spawn);
    }

    public void PlaySound(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.Spawn:
                audioSource.PlayOneShot(spawn);
                break;
            case SoundType.Punch:
                audioSource.PlayOneShot(punch);
                break;
            case SoundType.Kick:
                audioSource.PlayOneShot(kick);
                break;
            case SoundType.Jump:
                audioSource.PlayOneShot(jump);
                break;
            case SoundType.Dash:
                audioSource.PlayOneShot(dash);
                break;
            case SoundType.Hurt:
                audioSource.PlayOneShot(hurt);
                break;
            case SoundType.Death:
                audioSource.PlayOneShot(death);
                break;
            case SoundType.Block:
                audioSource.PlayOneShot(block);
                break;
        }
    }

    // Used by Animation Events
    public void PlaySoundFromEvent(int soundIndex)
    {
        PlaySound((SoundType)soundIndex);
    }
}
