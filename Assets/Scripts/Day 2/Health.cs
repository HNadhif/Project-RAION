using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 3;
    public int currentHealth;
    public float immunityDuration = 2f; 
    
    [Header("UI References")]
    public GameObject[] hearts; 
    
    // Kita kembali pakai cara ini, karena paling aman
    public GameObject gameOverCanvas; 

    private bool isImmune = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateHeartUI();
        
        // Pastikan Game Over mati dulu saat game mulai
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("HEI! Kotak Game Over Canvas di Player BELUM DIISI!");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isImmune) return;
        currentHealth -= damage;
        UpdateHeartUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BecomeImmuneRoutine());
        }
    }

    void UpdateHeartUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth) hearts[i].SetActive(true);
            else hearts[i].SetActive(false);
        }
    }

    IEnumerator BecomeImmuneRoutine()
    {
        isImmune = true;
        float timer = 0;
        while (timer < immunityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; 
            yield return new WaitForSeconds(0.1f); 
            timer += 0.1f;
        }
        spriteRenderer.enabled = true; 
        isImmune = false;
    }

    void Die()
    {
        // Matikan player
        GetComponent<Movements>().enabled = false;
        Time.timeScale = 0; 
        
        // NYALAKAN GAME OVER
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnGameOver();
        }
    }
}