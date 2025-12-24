using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public enum SoundType
    {
        Punch,
        Kick,
        Jump,
        Hurt,
        Block,
        Dash,
        Death,
        Walk
    }

    public AudioSource audioSource;

    public AudioClip punch;
    public AudioClip kick;
    public AudioClip jump;
    public AudioClip hurt;
    public AudioClip block;
    public AudioClip dash;
    public AudioClip death;
    public AudioClip walk;

    public void PlaySound(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.Punch:
                audioSource.PlayOneShot(punch);
                break;

            case SoundType.Kick:
                audioSource.PlayOneShot(kick);
                break;

            case SoundType.Jump:
                audioSource.PlayOneShot(jump);
                break;

            case SoundType.Hurt:
                audioSource.PlayOneShot(hurt);
                break;

            case SoundType.Block:
                audioSource.PlayOneShot(block);
                break;
            
            case SoundType.Dash:
                audioSource.PlayOneShot(dash);
                break;

            case SoundType.Death:
                audioSource.PlayOneShot(death);
                break;
            
            case SoundType.Walk:
            audioSource.PlayOneShot(walk);
            break;
        }
    }

    
    public void PlaySoundFromEvent(int soundIndex)
    {
        PlaySound((SoundType)soundIndex);
    }
}
