using UnityEngine;

/// Simple scrolling background that moves continuously to the left
/// Handles infinite looping by repositioning when off-screen
public class ScrollingBackground : MonoBehaviour
{
    [Header("Scrolling Settings")]
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private bool autoScroll = true;
    
    [Header("Looping Settings")]
    [SerializeField] private float loopDistance = 10f; // Distance to travel before looping
    
    private float startPositionX;
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        // Save initial X position
        startPositionX = transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Debug.Log($"{gameObject.name} - Start X: {startPositionX}, Loop Distance: {loopDistance}");
    }
    
    private void Update()
    {
        if (autoScroll)
        {
            Scroll();
        }
    }
    
    private void Scroll()
    {
        // Move to the left
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        
        // Check if traveled beyond loop distance
        if (transform.position.x < startPositionX - loopDistance)
        {
            // Teleport forward by loop distance
            Vector3 pos = transform.position;
            pos.x += loopDistance;
            transform.position = pos;
        }
    }
    
    /// Set scroll speed at runtime
    public void SetScrollSpeed(float speed)
    {
        scrollSpeed = speed;
    }
    
    /// Enable or disable scrolling
    public void SetAutoScroll(bool enabled)
    {
        autoScroll = enabled;
    }
    
    /// Set the loop distance (how far to travel before teleporting back)
    public void SetLoopDistance(float distance)
    {
        loopDistance = distance;
    }
}