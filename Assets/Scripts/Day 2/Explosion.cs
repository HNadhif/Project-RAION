using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float duration = 0.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
