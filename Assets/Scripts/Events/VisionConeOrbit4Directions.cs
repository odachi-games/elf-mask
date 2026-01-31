using UnityEngine;

public class VisionConeOrbit4Directions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform npc;
    [SerializeField] private NPCMovement npcMovement;

    [Header("Vision")]
    [SerializeField] private float radius = 2f;
    [SerializeField] private float changeInterval = 1f;

    public Vector2 CurrentDirection { get; private set; }

    private Vector2[] scanDirections =
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    private int currentIndex;
    private float timer;

    void Update()
    {
        if (!npcMovement.IsMoving)
        {
            npcMovement.LookAtDirection(CurrentDirection);
        }

        if (!npc || !npcMovement) return;

        Vector2 dir;

        if (!npcMovement.IsMoving)
        {
            timer += Time.deltaTime;

            if (timer >= changeInterval)
            {
                timer = 0f;
                currentIndex = (currentIndex + 1) % scanDirections.Length;
            }

            dir = scanDirections[currentIndex];
        }
        else
        {
            timer = 0f;
            dir = npcMovement.MoveDirection;

            if (dir == Vector2.zero)
                dir = Vector2.down;
        }
        
        CurrentDirection = dir.normalized;

        transform.position = (Vector2)npc.position + dir * radius;
        transform.right = dir;
    }
}
