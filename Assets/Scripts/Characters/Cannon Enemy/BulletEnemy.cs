using UnityEngine;
using System.Collections;

/// <summary>
/// Enemy that moves in one direction until it crashes with a wall.
/// </summary>
public class BulletEnemy : GridBasedObject
{
    [Header("Settings")]
    [Range(1, 5)][SerializeField] private int damage = 1;
    [Range(0f, 1f)][SerializeField] private float movementCooldown = 0.3f;

    [Header("Components")]
    [SerializeField] private LayerMask playerLayerMask;

    private Vector2 direction = Vector2.left;

    private void Start()
    {
        StartCoroutine(Move());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check layer mask
        if (!IsInLayerMask(other.gameObject.layer, playerLayerMask))
            return;
        // Check if its damagable
        IDamageable target = other.GetComponent<IDamageable>();
        if (target == null)
            return;
        target.TakeDamage(damage);
    }

    private IEnumerator Move()
    {
        while (true)
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
                // if not it crashed and destroy it
                Destroy(gameObject);
            }
        }
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return ((1 << layer) & layerMask) != 0;
    }
}