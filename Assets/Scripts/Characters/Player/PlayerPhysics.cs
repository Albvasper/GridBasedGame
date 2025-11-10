using UnityEngine;
using System.Collections;

/// <summary>
/// Handles player collisions and physics.
/// </summary>
public class PlayerPhysics : MonoBehaviour
{
    private int enemyLayerMask;
    private PlayerHealth player;

    private void Awake()
    {
        player = GetComponent<PlayerHealth>();
        enemyLayerMask = LayerMask.NameToLayer("Enemy");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == enemyLayerMask)
        {
            player.TakeDamage(1);
        }
    }
}
