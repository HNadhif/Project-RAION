using UnityEngine;

public class bulletDestroyAudio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, GetComponent<AudioSource>().clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
