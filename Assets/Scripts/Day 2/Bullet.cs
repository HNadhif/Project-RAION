using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;
    private float lifetime = 5f;
    [SerializeField] private GameObject explosionAnimationPrefab;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
    
    /// Set bullet direction
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
    
    /// Set bullet speed
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("PlayerBullet") && other.CompareTag("Enemy"))
        {
            if (explosionAnimationPrefab != null)
            {
                GameObject explosionAnim = Instantiate(explosionAnimationPrefab, transform.position, Quaternion.identity);
                Rigidbody2D rrb = explosionAnim.GetComponent<Rigidbody2D>();
                if (rrb != null)
                {
                    rrb.linearVelocity = Vector2.left * 13f;
                }
                
                Destroy(explosionAnim, 2f);
            }
            // Destroy enemy
            Destroy(other.gameObject);
            
            // Destroy bullet
            Destroy(gameObject);
        }
        else if (gameObject.CompareTag("EnemyBullet") && other.CompareTag("Player"))
        {
            Movements player = other.GetComponent<Movements>();
            if (player != null && player.IsImmune)
            {
                return;
            }

            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        OnTriggerEnter2D(other.collider);
    }
    
    /// Destroy bullet when it goes off screen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}