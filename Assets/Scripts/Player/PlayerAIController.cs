using UnityEngine;

public class PlayerAIController : MonoBehaviour
{
    private PlayerMovement movement;
    private Transform opponent;

    private float decisionTimer;

    private void Awake()
    {
        Debug.Log("PlayerAIController active on " + gameObject.name);
        movement = GetComponent<PlayerMovement>();
        opponent = movement.opponent;

    }

    private void Update()
    {
        if (!opponent) return;

        decisionTimer -= Time.deltaTime;
        if (decisionTimer > 0f) return;

        decisionTimer = Random.Range(0.6f, 1.2f);

        float dir = Mathf.Sign(opponent.position.x - transform.position.x);
        int choice = Random.Range(0, 4);

        switch (choice)
        {
            case 0:
                movement.AI_Move(dir);
                break;

            case 1:
                movement.AI_StopMove();
                movement.AI_Punch();
                break;

            case 2:
                movement.AI_StopMove();
                movement.AI_Kick();
                break;

            case 3:
                movement.AI_Block(true);
                Invoke(nameof(ReleaseBlock), 0.4f);
                break;
        }
    }

    private void ReleaseBlock()
    {
        if (movement != null)
            movement.AI_Block(false);
    }
}
