using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages grid position for any game entity & object that is attached to the grid system.
/// </summary>
public abstract class GridBasedObject : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] public Tilemap groundTilemap;
    [SerializeField] public Tilemap collisionTilemap;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SnapToGrid();
    }

    protected void SnapToGrid()
    {
        rb.MovePosition(SnapToGridCenter(transform.position));
    }

    protected Vector3 SnapToGridCenter(Vector3 position)
    {
        const float tileCenter = 0.5f;
        return new Vector3(
            Mathf.Floor(position.x) + tileCenter,
            Mathf.Floor(position.y) + tileCenter,
            position.z
        );
    }

    /// <summary>
    /// Check if the targeted tile is a wall or is ground.
    /// </summary>
    /// <param name="targetWorldPos">Tile world position.</param>
    /// <returns>Is tile is ground or wall.</returns>
    protected bool IsNextTileAvailable(Vector3 targetWorldPos)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(targetWorldPos);
        return groundTilemap.HasTile(gridPosition) && !collisionTilemap.HasTile(gridPosition);
    }
}
