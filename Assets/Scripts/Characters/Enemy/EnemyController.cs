using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Handles enemy movement and behavior inside the grid.
/// </summary>
public class EnemyController : GridBasedObject
{
    [Header("Behavior Settings")]
    [Range(0f, 4f)][SerializeField] private float movementCooldown = .5f;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool verticalMovement = true;

    private Vector2 direction = Vector2.down;

    private void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (canMove && verticalMovement)
        {
            yield return new WaitForSeconds(movementCooldown);
            // Get next position
            Vector3 targetPos = transform.position + (Vector3)direction;
            targetPos = SnapToGridCenter(targetPos);

            // Check if next position is valid
            if (IsNextTileAvailable(targetPos))
            {
                // if position is valid move to the next tile
                Rb.MovePosition(targetPos);
            }
            else
            {
                // if not, change directions
                direction = -direction;
            }
        }
    }
}
