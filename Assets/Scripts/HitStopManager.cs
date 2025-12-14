using UnityEngine;
using System.Collections;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance;

    private int hitStopCounter = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void DoHitStop(float duration)
    {
        StartCoroutine(HitStopRoutine(duration));
    }

    private IEnumerator HitStopRoutine(float duration)
    {
        hitStopCounter++;

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        hitStopCounter--;

        if (hitStopCounter <= 0)
        {
            hitStopCounter = 0;
            Time.timeScale = 1f;
        }
    }
}
