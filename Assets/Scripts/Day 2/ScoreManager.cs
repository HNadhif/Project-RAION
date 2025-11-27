using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    [SerializeField] private int score = 0;
    [SerializeField] private int enemyKillScore = 1000;
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText; // For game over screen
    [SerializeField] private TextMeshProUGUI missileText;
    [SerializeField] private GameObject dashUI;
    
    // Singleton instance
    public static ScoreManager Instance { get; private set; }
    
    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        UpdateScoreUI();
    }
    
    /// Add score when enemy is destroyed
    public void AddEnemyKillScore()
    {
        score += enemyKillScore;
        UpdateScoreUI();
        Debug.Log("kill");
    }
    
    /// Get current score
    public int GetScore()
    {
        return score;
    }
    
    /// Reset score to zero
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }
    
    /// Update score UI text
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + score.ToString();
        }
    }
    
    /// Called when game is over to update final score display
    public void OnGameOver()
    {
        UpdateScoreUI();
    }
    
    /// Update missile UI text
    public void UpdateMissileUI(int currentCount, int maxCount)
    {
        if (missileText != null)
        {
            missileText.text = "(Q/E) Missile: " + currentCount + " / " + maxCount;
        }
    }

    public void SetDashUIOpacity(float alpha)
    {
        if (dashUI == null) return;

        // Coba ambil Image (UI)
        var img = dashUI.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
        {
            Color c = img.color;
            c.a = Mathf.Clamp01(alpha);
            img.color = c;
        }

        // Jika pakai SpriteRenderer (kalau dashUI bukan UI Canvas)
        var sr = dashUI.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Clamp01(alpha);
            sr.color = c;
        }
    }
}