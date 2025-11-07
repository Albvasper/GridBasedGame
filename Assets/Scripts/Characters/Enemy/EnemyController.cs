using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System;
using Unity.VisualScripting;

/// <summary>
/// Handles enemy movement and behavior inside the grid.
/// </summary>
public class EnemyController : MonoBehaviour
{
    [Header("Behavior Settings")]
    [Range(0f, 4f)][SerializeField] private float movementCooldown = .5f;
    [Range(0f, 3f)][SerializeField] private float movementSpeed = 1f;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool verticalMovement = true;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;

    private Vector2 currentDirection = Vector2.down;

    private void Awake()
    {
        SnapToGrid();
    }

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
            Vector3 targetPos = transform.position + (Vector3)currentDirection;
            targetPos = SnapToGridCenter(targetPos);

            // Check if next position is valid
            if (IsNextTileAvailable(targetPos))
            {
                // if position is valid move to the next tile
                transform.position = targetPos;
            }
            else
            {
                // if not, change directions
                currentDirection = -currentDirection;
            }
        }
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
        transform.position = SnapToGridCenter(transform.position);
    }

    private bool IsNextTileAvailable(Vector3 targetWorldPos)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(targetWorldPos);
        return groundTilemap.HasTile(gridPosition) && !collisionTilemap.HasTile(gridPosition);
    }
}
