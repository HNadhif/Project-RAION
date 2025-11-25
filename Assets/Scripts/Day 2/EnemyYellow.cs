using UnityEngine;

public class EnemyYellow : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float fireRate = 2f;
    private float fireCountdown = 0f;
    private int facing = 1;

    public GameObject enemyBulletPrefab;
    private Transform playerTransform;
    Rigidbody2D rb;
    
    void Update()
    {   
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        fireCountdown -= Time.deltaTime;
        if(fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = fireRate;
        }
    }

    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        float yRot = transform.eulerAngles.y;
        if (Mathf.Approximately(Mathf.DeltaAngle(yRot, 180f), 0f) || transform.localScale.x < 0f)
        {
            facing = -1;
        }
        else
        {
            facing = 1;
        }

        var sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = (facing == 1);
        }

        // Cari player di scene
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    void Shoot()
    {
        if (enemyBulletPrefab != null && playerTransform != null)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y -= 0.3f;

            // Hitung arah ke player
            Vector3 directionToPlayer = (playerTransform.position - spawnPosition).normalized;

            GameObject bullet = Instantiate(enemyBulletPrefab, spawnPosition, Quaternion.identity);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            if (bulletRb != null)
            {
                bulletRb.linearVelocity = directionToPlayer * 10f;
            }

            // Rotasi peluru sesuai arah ke player (adjust untuk horizontal default)
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Called when enemy is destroyed by player bullet
    /// </summary>
    public void OnEnemyDestroyed()
    {
        // Add score when enemy is destroyed
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddEnemyKillScore();
        }
        
        // Optional: Add explosion effect or sound
        // Instantiate(explosionEffect, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If hit by player bullet
        if (other.CompareTag("PlayerBullet") || other.CompareTag("Explosion"))
        {
            if (other.CompareTag("Explosion"))
            {
                OnEnemyDestroyed();
            } else
            {
                // Destroy the bullet
                Destroy(other.gameObject);
                
                // Destroy enemy and add score
                OnEnemyDestroyed();
            }
        }
    }

}
