using UnityEngine;

public class rotate : MonoBehaviour
{
    [SerializeField]public float targetAngle = -90f;
    [SerializeField]public float duration = 0.5f; // durasi rotasi
    private float time = 0f;

    [SerializeField]private float startAngle;

    void Start()
    {
        startAngle = transform.eulerAngles.z;
    }

    void Update()
    {
        if (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Easing in-out (awal lambat, akhir lambat)
            t = Mathf.SmoothStep(0f, 1f, t);

            float angle = Mathf.LerpAngle(startAngle, targetAngle, t);

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
