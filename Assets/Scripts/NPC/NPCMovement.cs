using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitTime = 2f; // 🔥 tempo parado no waypoint

    private readonly int moveX = Animator.StringToHash("MoveX");
    private readonly int moveY = Animator.StringToHash("MoveY");

    private Waypoint waypoint;
    private Animator animator;
    private Vector3 previousPos;
    private int currentPointIndex;

    private float waitTimer;
    private bool isWaiting;

    public bool IsMoving { get; private set; }
    public Vector2 MoveDirection { get; private set; }
    

    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
        animator = GetComponent<Animator>();
        previousPos = transform.position;
    }

    private void Update()
    {
        if (isWaiting)
        {
            HandleWaiting();
            return;
        }

        Vector3 nextPos = waypoint.GetPosition(currentPointIndex);

        float distance = Vector3.Distance(transform.position, nextPos);
        IsMoving = distance > 0.05f;

        UpdateMoveValues(nextPos);

        transform.position = Vector3.MoveTowards(
            transform.position,
            nextPos,
            moveSpeed * Time.deltaTime
        );

        if (distance <= 0.2f)
        {
            // chegou → para
            IsMoving = false;
            MoveDirection = Vector2.zero;
            isWaiting = true;
            waitTimer = 0f;

            previousPos = nextPos;
            currentPointIndex = (currentPointIndex + 1) % waypoint.Points.Length;

            // animação idle
            animator.SetFloat(moveX, 0);
            animator.SetFloat(moveY, 0);
        }
    }

    private void HandleWaiting()
    {
        waitTimer += Time.deltaTime;

        if (waitTimer >= waitTime)
        {
            isWaiting = false;
        }
    }

    private void UpdateMoveValues(Vector3 nextPos)
    {
        Vector2 dir = Vector2.zero;

        if (previousPos.x < nextPos.x) dir = Vector2.right;
        else if (previousPos.x > nextPos.x) dir = Vector2.left;
        else if (previousPos.y < nextPos.y) dir = Vector2.up;
        else if (previousPos.y > nextPos.y) dir = Vector2.down;

        MoveDirection = dir;

        animator.SetFloat(moveX, dir.x);
        animator.SetFloat(moveY, dir.y);
    }

    public void LookAtDirection(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        MoveDirection = dir.normalized;

        animator.SetFloat(moveX, MoveDirection.x);
        animator.SetFloat(moveY, MoveDirection.y);
    }
}
