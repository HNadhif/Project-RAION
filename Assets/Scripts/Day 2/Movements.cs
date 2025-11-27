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

    [Header("Dash Settings")]
    [Tooltip("Dash speed in units/second")]
    public float dashSpeed = 12f;
    [Tooltip("Dash duration in seconds")]
    public float dashDuration = 0.18f;
    [Tooltip("Dash cooldown in seconds")]
    public float dashCooldown = 1f;
    [Tooltip("Key to trigger dash")]
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("Shooting Settings")]
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
    public bool IsImmune => isImmune;
    private Vector2 lastPos;
    private Vector2 currentVelocity;
    public int killCount = 0;
    private int missileCount = 0;

    // Dash state
    private bool isDashing = false;
    private bool isImmune = false;
    private float dashTimeRemaining = 0f;
    private float dashCooldownRemaining = 0f;
    private Vector2 dashDirection = Vector2.right;

    // Renderers for opacity changes during dash
    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;


    private float nextFireTime = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastPos = rb.position;
        UpdateMissileUI();

        // Cache all SpriteRenderers on this object and children so we can change opacity during dash
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        if (spriteRenderers != null && spriteRenderers.Length > 0)
        {
            originalColors = new Color[spriteRenderers.Length];
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                originalColors[i] = spriteRenderers[i].color;
            }
        }
    }

    void Update()
    {
        HandleMovementInput();
        HandleShootingInput();
        HandleDashInput();

        // Dash / cooldown timers
        if (dashCooldownRemaining > 0f)
        {
            float before = dashCooldownRemaining;

            dashCooldownRemaining = Mathf.Max(0f, dashCooldownRemaining - Time.deltaTime);

            // detect the exact moment cooldown hits 0
            if (before > 0f && dashCooldownRemaining == 0f)
            {
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.SetDashUIOpacity(1f);
                }
            }
        }

        if (isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;
            if (dashTimeRemaining <= 0f)
            {
                isDashing = false;
                isImmune = false;
                dashCooldownRemaining = dashCooldown;
                RestoreOriginalOpacity();
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.SetDashUIOpacity(0.5f);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
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
    /// Handle dash input and state
    /// </summary>
    private void HandleDashInput()
    {
        if (Input.GetKeyDown(dashKey) && !isDashing && dashCooldownRemaining <= 0f)
        {
            // Use current movement direction if available, otherwise dash to the right
            if (movement.sqrMagnitude > 0.01f)
                dashDirection = movement.normalized;
            else
                dashDirection = Vector2.right;

            isDashing = true;
            isImmune = true;
            dashTimeRemaining = dashDuration;
            // Set semi-transparent while dashing
            SetOpacity(0.5f);
            // cooldown will start when dash ends
        }
    }

    /// <summary>
    /// Set opacity for all cached sprite renderers
    /// </summary>
    private void SetOpacity(float alpha)
    {
        if (spriteRenderers == null)
            return;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] == null) continue;
            Color c = spriteRenderers[i].color;
            c.a = Mathf.Clamp01(alpha);
            spriteRenderers[i].color = c;
        }
    }

    /// <summary>
    /// Restore original colors (including original alpha) for all sprite renderers
    /// </summary>
    private void RestoreOriginalOpacity()
    {
        if (spriteRenderers == null || originalColors == null)
            return;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] == null) continue;
            spriteRenderers[i].color = originalColors[i];
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
        // Ignore hits while dashing (immune)
        if (isImmune)
            return;

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
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.UpdateMissileUI(missileCount, missileMax);
        }
    }
}
