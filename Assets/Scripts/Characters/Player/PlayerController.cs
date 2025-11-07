using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(BoxCollider2D))]

/// <summary>
/// Handles player input and snaps it to the grid.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private const float InputThreshold = 0.01f;

    [Header("Movement Settings")]
    [Tooltip("Time it takes to finish moving to another tile.")]
    [Range(0f, 1f)][SerializeField] private float moveDuration = 0.05f;
    [Tooltip("Time between moves when holding.")]
    [Range(0f, 1f)][SerializeField] private float moveInterval = 0.08f;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackArea;
    [Range(1f, 3f)][SerializeField] private int damage = 1;
    [Range(0f, 1f)][SerializeField] private float attackCooldown = 0.5f;
    [Range(0f, 1f)][SerializeField] private float attackDuration = 0.25f;
    [Range(0f, 4f)][SerializeField] private float attackRangeX = 1f;
    [Range(0f, 4f)][SerializeField] private float attackRangeY = 1f;
    [SerializeField] private LayerMask enemyLayerMask;
    
    [Header("Tilemaps")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;

    private Rigidbody2D rb;
    private bool isMoving;
    private Vector2 moveInput;
    private Vector2 currentDirection = Vector2.down;
    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SnapToGrid();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackArea.position, new Vector3(attackRangeX, attackRangeY, 1f));
    }

    /// <summary>
    /// This gets called by PlayerInput component via Unity Events
    /// </summary>
    /// <param name="context">Input action</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        // Always update the input value
        moveInput = context.ReadValue<Vector2>();
        
        // Update direction when input is detected
        if (moveInput.sqrMagnitude > InputThreshold)
        {
            Vector2 direction = GetCardinalDirection(moveInput);
            currentDirection = direction;
            UpdateAttackAreaPosition();
        }

        // Only try to start movement if not already moving
        if (!isMoving && moveInput.sqrMagnitude > InputThreshold)
        {
            StartCoroutine(HandleMovement());
        }
        // If input is released, clear it
        else if (context.canceled)
        {
            moveInput = Vector2.zero;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void OnAttack()
    {
        if (Time.time >= nextAttackTime && !isAttacking)
            StartCoroutine(Attack());
    }

    private IEnumerator HandleMovement()
    {
        isMoving = true;
        
        while (true)
        {
            // Check if input is still held
            if (moveInput.sqrMagnitude < InputThreshold)
            {
                isMoving = false;
                yield break;
            }
            
            // Check if direction changed
            Vector2 newDirection = GetCardinalDirection(moveInput);
            if (newDirection != currentDirection)
            {
                currentDirection = newDirection;
                UpdateAttackAreaPosition();
            }
            
            yield return MoveToTile(currentDirection);
            yield return new WaitForSeconds(moveInterval);
        }
    }

    private IEnumerator MoveToTile(Vector2 direction)
    {
        Vector3 targetPos = transform.position + (Vector3)direction;
        targetPos = SnapToGridCenter(targetPos);

        if (!IsNextTileAvailable(targetPos))
            yield break;

        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            t = t * t * (3f - 2f * t);

            rb.MovePosition(Vector3.Lerp(startPos, targetPos, t));
            yield return null;
        }
        rb.MovePosition(targetPos);
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        // Deal Damage
        Collider2D[] enemiesToDamage =
        Physics2D.OverlapBoxAll (attackArea.position, new Vector2(attackRangeX, attackRangeY), 0f, enemyLayerMask);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            Debug.Log(enemiesToDamage[i].name);
            // TODO: Cache get component!
            enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(damage);
        }

        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    private void UpdateAttackAreaPosition()
    {
        Vector3 attackAreaPosition = transform.position + (Vector3)currentDirection;
        attackArea.transform.position = attackAreaPosition;

    }
    private Vector2 GetCardinalDirection(Vector2 input)
    {
        return Mathf.Abs(input.x) > Mathf.Abs(input.y)
            ? new Vector2(Mathf.Sign(input.x), 0)
            : new Vector2(0, Mathf.Sign(input.y));
    }

    private Vector3 SnapToGridCenter(Vector3 position)
    {
        const float tileCenter = 0.5f;
        return new Vector3(
            Mathf.Floor(position.x) + tileCenter,
            Mathf.Floor(position.y) + tileCenter,
            position.z
        );
    }
    
    private void SnapToGrid()
    {
        rb.MovePosition(SnapToGridCenter(transform.position));
    }
    
    
    private bool IsNextTileAvailable(Vector3 targetWorldPos)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(targetWorldPos);
        return groundTilemap.HasTile(gridPosition) && !collisionTilemap.HasTile(gridPosition);
    }
}