using UnityEngine;
using TMPro;

public class Movements : MonoBehaviour
{
    // ... (Semua variabel movement & shooting Anda tetap sama, saya sembunyikan biar ringkas) ...
    
    [Header("Movement Settings")]
    public float speed = 5f;
    // ... Variable lain tetap ada ...

    [Header("Components")]
    private Rigidbody2D rb;
    private Health playerHealth; // Referensi ke script Health

    // ... Variable Shooting tetap ada ...
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
    [SerializeField] private TextMeshProUGUI missileText;
    
    // ... Variable Logic ...
    private Vector2 movement;
    private Vector2 lastPos;
    private Vector2 currentVelocity;
    public int killCount = 0;
    private int missileCount = 0;
    private float nextFireTime = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<Health>(); // AMBIL REFERENSI HEALTH
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
        // ... (Logika missile count Anda tetap sama) ...
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
        // ...
    }

    // ... (Fungsi HandleMovementInput, HandleShootingInput, Shoot, ShootMissile tetap sama) ...
    private void HandleMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.sqrMagnitude > 1f) movement = movement.normalized;
    }

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
    
    private void Shoot()
    {
        if (bulletPrefab == null) return;
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        spawnPosition.x += 0.5f;
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null) bulletRb.linearVelocity = Vector2.right * bulletSpeed;
        else {
             Bullet bulletScript = bullet.GetComponent<Bullet>();
             if (bulletScript != null) { bulletScript.SetDirection(Vector3.right); bulletScript.SetSpeed(bulletSpeed); }
        }
        bullet.transform.rotation = Quaternion.Euler(0, 0, -90);
        bullet.tag = "PlayerBullet";
    }

    private void ShootMissile()
    {
        missileCount--;
        UpdateMissileUI();
        if (missilePrefab == null) return;
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        spawnPosition.y -= 0.5f;
        GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D missileRb = missile.GetComponent<Rigidbody2D>();
        if (missileRb != null) missileRb.linearVelocity = new Vector2(currentVelocity.x/2, 0);
        missile.tag = "PlayerBullet";
    }

    private void ShootMissile1()
    {
        missileCount--;
        UpdateMissileUI();
        if (missilePrefab1 == null) return;
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        spawnPosition.x += 0.7f;
        GameObject missile = Instantiate(missilePrefab1, spawnPosition, Quaternion.identity);
        Rigidbody2D missileRb = missile.GetComponent<Rigidbody2D>();
        if (missileRb != null) missileRb.linearVelocity = Vector2.right * (bulletSpeed + 5f);
        missile.tag = "PlayerBullet";
    }
    
    private void UpdateMissileUI()
    {
        if (missileText != null) missileText.text = "Missile: " + missileCount + " / " + missileMax;
    }

    // --- BAGIAN PENTING YANG DIUBAH ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek tabrakan dengan Musuh ATAU Peluru Musuh
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet"))
        {
            // Panggil fungsi damage di script Health
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            // Jika yang menabrak adalah peluru, hancurkan pelurunya
            if (other.CompareTag("EnemyBullet"))
            {
                Destroy(other.gameObject);
            }
            
            // OPSI: Jika menabrak musuh, apakah musuhnya hancur juga?
            // Jika ya, uncomment baris di bawah:
            // if (other.CompareTag("Enemy")) Destroy(other.gameObject);
        }
    }
}