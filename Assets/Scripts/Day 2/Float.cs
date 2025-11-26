using UnityEngine;

public class Float : MonoBehaviour
{
    [Header("Movement Settings")]
    public float amplitude = 50f; // jarak naik-turun
    public float speed = 1f;      // kecepatan gerak

    private Vector3 startPos;

    void Start()
    {
        // simpan posisi awal UI
        startPos = transform.localPosition;
    }

    void Update()
    {
        // hitung offset Y menggunakan Sin untuk gerakan smooth in-out
        float offsetY = Mathf.Sin(Time.time * speed * Mathf.PI * 2) * amplitude;
        transform.localPosition = startPos + new Vector3(0, offsetY, 0);
    }
}
