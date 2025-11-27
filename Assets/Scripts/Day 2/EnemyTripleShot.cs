using UnityEngine;

public class EnemyTripleShot : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float fireRate = 2f;
    private float fireCountdown = 0f;
    private Vector3 lastDirection = Vector3.up;
    private int facing = 1;

    public GameObject enemyBulletPrefab;
    public GameObject deathSoundBulletPrefab;
    Rigidbody2D rb;
    private Movements player;
    [SerializeField] private GameObject bulletSound;
    
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
        player = FindObjectOfType<Movements>();
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
            if (bulletSound != null)
            {
                Instantiate(bulletSound, transform.position, Quaternion.identity);
            }
            Vector3 spawnPosition = transform.position;
            
            // Shoot ke tiga arah: kiri (tengah), atas-kiri, bawah-kiri
            ShootBullet(spawnPosition, Vector3.left, 0f);  // Kiri
            ShootBullet(spawnPosition + Vector3.up * 0.5f, new Vector3(-0.866f, 0.5f, 0).normalized, 0f);  // Atas-kiri
            ShootBullet(spawnPosition + Vector3.down * 0.5f, new Vector3(-0.866f, -0.5f, 0).normalized, 0f);  // Bawah-kiri
        }
    }

    void ShootBullet(Vector3 spawnPosition, Vector3 direction, float rotationZ)
    {
        GameObject bullet = Instantiate(enemyBulletPrefab, spawnPosition, Quaternion.identity);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = direction * 10f;
        }

        // Hitung rotasi berdasarkan arah peluru
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
    public void OnEnemyDestroyed()
    {
        player.killCount += 1;
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddEnemyKillScore();
        }
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet") || other.CompareTag("Explosion"))
        {
            if (other.CompareTag("Explosion"))
            {
                OnEnemyDestroyed();
            } else
            {
                if (deathSoundBulletPrefab != null)
                {
                    Instantiate(deathSoundBulletPrefab, transform.position, Quaternion.identity);
                }
                // Destroy the bullet
                Destroy(other.gameObject);
                
                // Destroy enemy and add score
                OnEnemyDestroyed();
            }
        }
    }

}
