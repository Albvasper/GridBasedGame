using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Handles player input and snaps it to the grid.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap collisionTilemap;
    /// <summary>
    /// Time it takes to finish moving to another tile.
    /// </summary>
    [SerializeField]
    private float moveDuration = 0.05f;
    /// <summary>
    /// Time between moves when holding.
    /// </summary>
    [SerializeField]
    private float moveInterval = 0.08f;
    /// <summary>
    /// 
    /// </summary>
    private const float InputThreshold = 0.01f;
    private PlayerMovement controls;
    private bool isMoving;

    private void Awake()
    {
        controls = new PlayerMovement();
        SnapToGrid();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        if (isMoving) return;
        // Handle player input
        Vector2 input = controls.Main.Movement.ReadValue<Vector2>();
        // Check if there is any input
        if (input.sqrMagnitude > InputThreshold)
        {
            Vector2 direction = GetCardinalDirection(input);
            StartCoroutine(HandleMovement(direction));
        }
    }

    private IEnumerator HandleMovement(Vector2 direction)
    {
        isMoving = true;

        // Continuous movement while holding a key
        while (true)
        {
            Vector2 currentInput = controls.Main.Movement.ReadValue<Vector2>();

            // Stop if released or direction changed
            if (currentInput.sqrMagnitude < 0.01f || GetCardinalDirection(currentInput) != direction)
            {
                isMoving = false;
                yield break;
            }

            yield return MoveToTile(direction);
            yield return new WaitForSeconds(moveInterval);
        }
    }

    private IEnumerator MoveToTile(Vector2 direction)
    {
        Vector3 targetPos = transform.position + (Vector3)direction;
        targetPos = SnapToGridCenter(targetPos);

        if (!CanMove(targetPos))
            yield break;

        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            // Smoothstep for slightly less linear feel
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
    }

    private Vector2 GetCardinalDirection(Vector2 input)
    {
        return Mathf.Abs(input.x) > Mathf.Abs(input.y)
            ? new Vector2(Mathf.Sign(input.x), 0)
            : new Vector2(0, Mathf.Sign(input.y));
    }

    private Vector3 SnapToGridCenter(Vector3 position)
    {
        return new Vector3(
            Mathf.Floor(position.x) + 0.5f,
            Mathf.Floor(position.y) + 0.5f,
            position.z
        );
    }

    private void SnapToGrid()
    {
        transform.position = SnapToGridCenter(transform.position);
    }

    private bool CanMove(Vector3 targetWorldPos)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(targetWorldPos);
        return groundTilemap.HasTile(gridPosition) && !collisionTilemap.HasTile(gridPosition);
    }
}