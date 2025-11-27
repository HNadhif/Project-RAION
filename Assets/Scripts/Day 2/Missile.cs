using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;
    [SerializeField]private float lifetime = 5f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject explosionAnimationPrefab;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // spawn circle di posisi missile
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            GameObject explosionAnim = Instantiate(explosionAnimationPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = explosionAnim.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.left * 13f; 
            }

            // kill missile
            Destroy(gameObject);
        }
    }
}
