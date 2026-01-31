using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VisionConeOrbit4Directions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform npc;
    [SerializeField] private NPCMovement npcMovement;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Vision Settings")]
    [SerializeField] private float radius = 2f;
    [SerializeField] private float changeInterval = 1f;

    [Header("Line Offset")]
    [SerializeField] private Vector3 startOffset;
    [SerializeField] private Vector3 endOffset;

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

    void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        SetupDefaultLineMaterial();
        lineRenderer.positionCount = 2;
    }

    private void SetupDefaultLineMaterial()
    {
        if (lineRenderer.sharedMaterial == null)
        {
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = new Color(1f, 0f, 0f, 0f);
    }

    void Update()
    {
        if (!npc || !npcMovement) return;

        HandleDirection();
        UpdateTransformPosition();
        UpdateLineRenderer();
    }

    private void HandleDirection()
    {
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
            if (dir == Vector2.zero) dir = Vector2.down;
        }

        CurrentDirection = dir;
        npcMovement.LookAtDirection(CurrentDirection);
    }

    private void UpdateTransformPosition()
    {
        transform.position = (Vector2)npc.position + CurrentDirection * radius;
        transform.right = CurrentDirection;
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer == null) return;

        lineRenderer.SetPosition(0, npc.position + startOffset);
        lineRenderer.SetPosition(1, transform.position + endOffset);
    }
}