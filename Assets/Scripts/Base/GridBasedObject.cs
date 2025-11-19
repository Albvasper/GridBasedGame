using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]

/// <summary>
/// Manages grid position for any game entity & object that is attached to the grid system.
/// </summary>
public abstract class GridBasedObject : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap GroundTilemap;
    public Tilemap CollisionTilemap;

    public Rigidbody2D Rb { get; private set; }

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    protected void Onable()
    {
        SnapToGrid();
    }

    protected void SnapToGrid()
    {
        Rb.MovePosition(SnapToGridCenter(transform.position));
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
        Vector3Int gridPosition = GroundTilemap.WorldToCell(targetWorldPos);
        return GroundTilemap.HasTile(gridPosition) && !CollisionTilemap.HasTile(gridPosition);
    }
}
