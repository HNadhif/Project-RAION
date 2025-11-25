using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float fireRate = 2f;
    private float fireCountdown = 0f;
    private Vector3 lastDirection = Vector3.up;
    private int facing = 1;

    public GameObject enemyBulletPrefab;
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
    }

    void Shoot()
    {
        if (enemyBulletPrefab != null)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y -= 0.3f;

            GameObject bullet = Instantiate(enemyBulletPrefab, spawnPosition, Quaternion.identity);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector3.left * 10f;
            }

            bullet.transform.rotation = Quaternion.Euler(0, 0, -90);
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