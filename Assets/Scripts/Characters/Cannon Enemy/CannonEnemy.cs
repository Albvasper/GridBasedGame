using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

/// <summary>
/// Enemy that shoots bullets in one direction.
/// </summary>
public class CannonEnemy : MonoBehaviour
{
    // TODO: Object pooling
    //private List<GameObject> bullets;
    [Header("Shooting Settings")]
    [Range(0f, 3f)][SerializeField] private float shootingRate = 0.3f;
    [SerializeField] private bool canShoot = true;

    [Header("Components")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (canShoot)
        {
            yield return new WaitForSeconds(shootingRate);
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<BulletEnemy>().groundTilemap = groundTilemap;
            bullet.GetComponent<BulletEnemy>().collisionTilemap = collisionTilemap;
        }
    }
}
