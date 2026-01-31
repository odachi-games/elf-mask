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

        CurrentDirection = dir;

        npcMovement.LookAtDirection(CurrentDirection);

        transform.position = (Vector2)npc.position + CurrentDirection * radius;
        transform.right = CurrentDirection;

        if (!npcMovement.IsMoving)
        {
            string dirName = CurrentDirection switch
            {
                { x: 0, y: 1 } => "UP",
                { x: 1, y: 0 } => "RIGHT",
                { x: 0, y: -1 } => "DOWN",
                { x: -1, y: 0 } => "LEFT",
                _ => $"({CurrentDirection.x:F1}, {CurrentDirection.y:F1})"
            };

            Debug.Log($"[VISION] Parado → Olhando para: {dirName} | Direção: {CurrentDirection}", npc);
            
            Debug.DrawRay(npc.position, (Vector3)CurrentDirection * radius, Color.red);
        }
    }
}