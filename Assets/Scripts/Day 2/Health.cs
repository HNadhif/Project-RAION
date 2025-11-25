using UnityEngine;
using System.Collections; // Dibutuhkan untuk IEnumerator (Timer)

public class Health : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 3;
    public float currentHealth;
    public float immunityDuration = 2f; // Durasi kebal 2 detik

    [Header("UI Elements")]
    public GameObject[] hearts; // Masukkan 3 gambar hati Anda ke sini di Inspector
    public GameObject gameOverCanvas; // Pindahkan referensi Game Over ke sini

    private bool isImmune = false; // Status apakah player sedang kebal
    private SpriteRenderer spriteRenderer; // Untuk efek berkedip (opsional)

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Fungsi ini dipanggil oleh Movements saat tertabrak
    public void TakeDamage(int damage)
    {
        // 1. Jika sedang kebal, abaikan damage
        if (isImmune) return;

        // 2. Kurangi nyawa
        currentHealth -= damage;

        // 3. Update UI (Matikan gambar hati)
        UpdateHeartUI();

        // 4. Cek apakah mati
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // 5. Jika masih hidup, aktifkan fitur kebal sementara
            StartCoroutine(BecomeImmune());
        }
    }

    void UpdateHeartUI()
    {
        // Logika: Jika health 2, matikan hati ke-3 (index 2).
        // Kita loop semua hati, jika index >= currentHealth, matikan gambarnya.
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }

    IEnumerator BecomeImmune()
    {
        isImmune = true;
        Debug.Log("Player Immune!");

        // (Opsional) Efek visual berkedip agar pemain tahu sedang kebal
        if (spriteRenderer != null) spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); // Jadi transparan

        yield return new WaitForSeconds(immunityDuration); // Tunggu 2 detik

        if (spriteRenderer != null) spriteRenderer.color = Color.white; // Kembali normal
        isImmune = false;
        Debug.Log("Immunity Ended.");
    }

    void Die()
    {
        Time.timeScale = 0; // Stop waktu
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
    }
}