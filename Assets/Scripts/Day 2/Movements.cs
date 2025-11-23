using UnityEngine;

public class Movements : MonoBehaviour
{
    [Tooltip("Movement speed in units/second")]
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverCanvas;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.sqrMagnitude > 1f)
            movement = movement.normalized;


    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("EnemyBullet"))
        {
            Time.timeScale = 0;
            if(gameOverCanvas != null)
            {
                gameOverCanvas.SetActive(true);
            }
        }
    }

    private void OnCollisionEnter2D(Collider2D other)
    {
        
    }
}
