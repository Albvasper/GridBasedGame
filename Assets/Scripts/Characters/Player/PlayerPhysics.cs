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

    private void OnCollisionEnter2D(Collision2D other)
    {
        //FIXME: When colliding with enemies or other entities it goes out of the grid until player input is recieved.
        if (other.gameObject.layer == enemyLayerMask)
        {
            player.TakeDamage(1);
        }
    }
}
