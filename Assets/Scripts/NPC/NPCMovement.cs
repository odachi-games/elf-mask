using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitTime = 2f;

    private readonly int moving = Animator.StringToHash("Moving");
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
        animator.SetBool(moving, IsMoving);

        if (IsMoving)
        {
            UpdateMoveValues(nextPos);

            transform.position = Vector3.MoveTowards(
                transform.position,
                nextPos,
                moveSpeed * Time.deltaTime
            );
        }

        if (distance <= 0.1f)
        {
            IsMoving = false;
            animator.SetBool(moving, false);
            
            isWaiting = true;
            waitTimer = 0f;

            currentPointIndex = (currentPointIndex + 1) % waypoint.Points.Length;
            
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
        Vector2 dir = ((Vector2)nextPos - (Vector2)transform.position).normalized;

        if (dir.magnitude < 0.1f) return;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            MoveDirection = dir.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            MoveDirection = dir.y > 0 ? Vector2.up : Vector2.down;
        }

        animator.SetFloat(moveX, MoveDirection.x);
        animator.SetFloat(moveY, MoveDirection.y);
    }

    public void LookAtDirection(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        MoveDirection = dir.normalized;

        animator.SetFloat(moveX, MoveDirection.x);
        animator.SetFloat(moveY, MoveDirection.y);
    }
}
