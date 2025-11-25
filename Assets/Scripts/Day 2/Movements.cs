using UnityEngine;
using TMPro;

public class Movements : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed in units/second")]
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverCanvas;

    [Header("Shooting Settings")]
    [SerializeField] private TextMeshProUGUI missileText;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject missilePrefab1;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private KeyCode fireKey = KeyCode.Space;
    [SerializeField] private KeyCode missileKey = KeyCode.E;
    [SerializeField] private KeyCode missileKey1 = KeyCode.Q;
    [SerializeField] private int missileMax = 2;
    private Vector2 lastPos;
    private Vector2 currentVelocity;
    public int killCount = 0;
    private int missileCount = 0;


    private float nextFireTime = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastPos = rb.position;
        UpdateMissileUI();
    }

    void Update()
    {
        HandleMovementInput();
        HandleShootingInput();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        Vector2 newPos = rb.position;
        currentVelocity = (newPos - lastPos) / Time.fixedDeltaTime;
        lastPos = newPos;

        if (missileCount < missileMax)
        {
            if (killCount >= 3)
            {
                missileCount++;
                UpdateMissileUI();
                killCount = 0;
            }
        }
        else
        {
            if (killCount > 0)
            {
                killCount = 0;
            }
        }
    }

    /// <summary>
    /// Handle movement input
    /// </summary>
    private void HandleMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.sqrMagnitude > 1f)
            movement = movement.normalized;
    }

    /// <summary>
    /// Handle shooting input
    /// </summary>
    private void HandleShootingInput()
    {
        if (Input.GetKeyDown(fireKey) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        } else if (Input.GetKeyDown(missileKey) && missileCount > 0)
        {
            ShootMissile();
        } else if (Input.GetKeyDown(missileKey1) && missileCount > 0)
        {
            ShootMissile1();
        }
    }

    /// <summary>
    /// Shoot a bullet
    /// </summary>
    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab is not assigned!");
            return;
        }
        
        // Determine fire point
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        
        // Adjust spawn position slightly forward
        spawnPosition.x += 0.5f;
        
        // Create bullet
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        
        // Set bullet direction and speed
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = Vector2.right * bulletSpeed;
        }
        else
        {
            // If no Rigidbody2D, try to get Bullet component and set direction
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(Vector3.right);
                bulletScript.SetSpeed(bulletSpeed);
            }
        }
        bullet.transform.rotation = Quaternion.Euler(0, 0, -90);
        // Set bullet tag
        bullet.tag = "PlayerBullet";
    }

    private void ShootMissile()
    {
        missileCount--;
        UpdateMissileUI();
        if (missilePrefab == null)
        {
            Debug.LogWarning("Missile prefab is not assigned!");
            return;
        }
        
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        
        spawnPosition.y -= 0.5f;
        
        GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D missileRb = missile.GetComponent<Rigidbody2D>();

        if (missileRb != null)
        {
            missileRb.linearVelocity = new Vector2(currentVelocity.x/2, 0);
        }
        
        missile.tag = "PlayerBullet";
    }

    private void ShootMissile1()
    {
        missileCount--;
        UpdateMissileUI();

        if (missilePrefab1 == null)
        {
            Debug.LogWarning("Missile prefab1 is not assigned!");
            return;
        }

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        spawnPosition.x += 0.7f; // offset

        GameObject missile = Instantiate(missilePrefab1, spawnPosition, Quaternion.identity);
        Rigidbody2D missileRb = missile.GetComponent<Rigidbody2D>();

        if (missileRb != null)
        {
            missileRb.linearVelocity = Vector2.right * (bulletSpeed + 5f); // lebih cepat dari peluru biasa
        }

        missile.tag = "PlayerBullet";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("EnemyBullet"))
        {
            Time.timeScale = 0;
            
            // Update final score display
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnGameOver();
            }
            
            if(gameOverCanvas != null)
            {
                gameOverCanvas.SetActive(true);
            }
        }
    }

    private void OnCollisionEnter2D(Collider2D other)
    {
        
    }
    private void UpdateMissileUI()
{
    if (missileText != null)
        missileText.text = "Missile: " + missileCount + " / " + missileMax;
}
}
