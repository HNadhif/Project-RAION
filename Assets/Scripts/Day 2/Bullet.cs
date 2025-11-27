using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;
    private float lifetime = 5f;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Set bullet direction
    /// </summary>
    /// <param name="newDirection">Direction vector</param>
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
    
    /// <summary>
    /// Set bullet speed
    /// </summary>
    /// <param name="newSpeed">Speed value</param>
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If this is a player bullet and hits an enemy
        if (gameObject.CompareTag("PlayerBullet") && other.CompareTag("Enemy"))
        {
            
            // Destroy enemy
            Destroy(other.gameObject);
            
            // Destroy bullet
            Destroy(gameObject);
        }
        // If this is an enemy bullet and hits player
        else if (gameObject.CompareTag("EnemyBullet") && other.CompareTag("Player"))
        {
            // Cek apakah player sedang immune (dash)
            Movements player = other.GetComponent<Movements>();
            if (player != null && player.IsImmune)
            {
                // Player sedang dash → peluru menembus → jangan hancurkan
                return;
            }

            // Player tidak dash → hancurkan peluru
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Alternative collision detection if using Collision instead of Trigger
        // Handle collision the same way as trigger
        OnTriggerEnter2D(other.collider);
    }
    
    /// <summary>
    /// Destroy bullet when it goes off screen
    /// </summary>
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}