using UnityEngine;

/// Auto-setup Desert background layers with scrolling
/// Attach to empty GameObject, assign sprites, then click "Setup Background" in Inspector
public class DesertBackgroundSetup : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    
    [Header("Desert Sprites - Assign from Assets/Background/desert")]
    [SerializeField] private Sprite desertSky;
    [SerializeField] private Sprite desertMoon;
    [SerializeField] private Sprite desertCloud;
    [SerializeField] private Sprite desertMountain;
    [SerializeField] private Sprite desertDuneMid;
    [SerializeField] private Sprite desertDuneFront;
    
    [Header("Scrolling Speeds (slower = further back)")]
    [SerializeField] private float skySpeed = 0.2f;
    [SerializeField] private float cloudSpeed = 0.3f;
    [SerializeField] private float mountainSpeed = 1f;
    [SerializeField] private float duneMidSpeed = 1.5f;
    [SerializeField] private float duneFrontSpeed = 2f;
    
    [Header("Auto Setup")]
    [SerializeField] private bool setupOnStart = false;
    
    private void Start()
    {
        if (setupOnStart)
        {
            SetupBackground();
        }
    }
    
    [ContextMenu("Setup Background")]
    public void SetupBackground()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        if (mainCamera == null)
        {
            Debug.LogError("Camera not found!");
            return;
        }
        
        ClearChildren();
        
        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        
        Debug.Log($"Setting up background for camera: {cameraWidth}x{cameraHeight}");
        
        CreateLayer("Sky", desertSky, 0, 0, 10f, cameraHeight * 1.1f, 0f, 0, cameraWidth, false, false);
        
        CreateLayer("Moon", desertMoon, 3, 2, 9f, 2f, 0.1f, 1, cameraWidth, false, false);
        
        CreateLayer("Clouds", desertCloud, 0, 1.5f, 8f, cameraHeight * 0.6f, cloudSpeed, 2, cameraWidth, true, true);
        
        CreateLayer("Mountains", desertMountain, 0, -1f, 7f, cameraHeight * 0.7f, mountainSpeed, 3, cameraWidth, true, false);
        
        CreateLayer("DuneMid", desertDuneMid, 0, -2f, 6f, cameraHeight * 0.7f, duneMidSpeed, 4, cameraWidth, true, false);
        
        CreateLayer("DuneFront", desertDuneFront, 0, -3f, 5f, cameraHeight * 0.7f, duneFrontSpeed, 5, cameraWidth, true, false);
        
        Debug.Log("Desert background setup complete!");
    }
    
    private void CreateLayer(string layerName, Sprite sprite, float x, float y, float z, 
        float height, float scrollSpeed, int sortingOrder, float cameraWidth, bool useDuplicate = true, bool useExtendedLoop = false)
    {
        if (sprite == null)
        {
            Debug.LogWarning($"Sprite for {layerName} not assigned!");
            return;
        }
        
        // Create container for this layer
        GameObject layerContainer = new GameObject(layerName + "_Container");
        layerContainer.transform.SetParent(transform);
        layerContainer.transform.localPosition = new Vector3(x, y, z);
        
        // Calculate scale to match desired height
        float spriteHeight = sprite.bounds.size.y;
        float scale = height / spriteHeight;
        
        // Calculate sprite width in world units after scaling
        float spriteWidth = sprite.bounds.size.x * scale;
        
        // Loop distance depends on useExtendedLoop flag
        // Extended: wait until sprite is completely off-screen before teleporting (for clouds)
        // Standard: teleport as soon as sprite exits left side (for mountains/dunes)
        float loopDistance = useExtendedLoop ? (spriteWidth + cameraWidth) : spriteWidth;
        
        Debug.Log($"{layerName}: scale={scale}, spriteWidth={spriteWidth}, loopDistance={loopDistance}, extended={useExtendedLoop}");
        
        if (scrollSpeed > 0 && useDuplicate)
        {
            // For scrolling layers, create 2 sprites positioned side-by-side
            // Position sprite2 right next to sprite1 (no gap)
            
            GameObject sprite1 = CreateSpriteObject(layerName + "_1", sprite, 0, 0, 0, scale, sortingOrder);
            sprite1.transform.SetParent(layerContainer.transform);
            
            ScrollingBackground scroll1 = sprite1.AddComponent<ScrollingBackground>();
            scroll1.SetScrollSpeed(scrollSpeed);
            scroll1.SetLoopDistance(loopDistance);
            scroll1.SetAutoScroll(true);
            
            // Second sprite positioned exactly at sprite width (seamless connection)
            GameObject sprite2 = CreateSpriteObject(layerName + "_2", sprite, spriteWidth, 0, 0, scale, sortingOrder);
            sprite2.transform.SetParent(layerContainer.transform);
            
            ScrollingBackground scroll2 = sprite2.AddComponent<ScrollingBackground>();
            scroll2.SetScrollSpeed(scrollSpeed);
            scroll2.SetLoopDistance(loopDistance);
            scroll2.SetAutoScroll(true);
        }
        else
        {
            // Static layer (no scrolling)
            GameObject spriteObj = CreateSpriteObject(layerName, sprite, 0, 0, 0, scale, sortingOrder);
            spriteObj.transform.SetParent(layerContainer.transform);
            
            if (scrollSpeed > 0)
            {
                ScrollingBackground scroll = spriteObj.AddComponent<ScrollingBackground>();
                scroll.SetScrollSpeed(scrollSpeed);
                scroll.SetAutoScroll(true);
            }
        }
    }
    
    private GameObject CreateSpriteObject(string name, Sprite sprite, float x, float y, float z, 
        float scale, int sortingOrder)
    {
        GameObject obj = new GameObject(name);
        obj.transform.localPosition = new Vector3(x, y, z);
        obj.transform.localScale = Vector3.one * scale;
        
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingLayerName = "Default";
        sr.sortingOrder = sortingOrder;
        
        return obj;
    }
    
    private void ClearChildren()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            else
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
    
    [ContextMenu("Clear Background")]
    public void ClearBackground()
    {
        ClearChildren();
        Debug.Log("Background cleared!");
    }
}