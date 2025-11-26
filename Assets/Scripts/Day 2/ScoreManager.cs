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
    
    /// <summary>
    /// Add score when enemy is destroyed
    /// </summary>
    public void AddEnemyKillScore()
    {
        score += enemyKillScore;
        UpdateScoreUI();
        Debug.Log("kill");
    }
    
    /// <summary>
    /// Get current score
    /// </summary>
    /// <returns>Current score</returns>
    public int GetScore()
    {
        return score;
    }
    
    /// <summary>
    /// Reset score to zero
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }
    
    /// <summary>
    /// Update score UI text
    /// </summary>
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
    
    /// <summary>
    /// Called when game is over to update final score display
    /// </summary>
    public void OnGameOver()
    {
        UpdateScoreUI();
    }
    
    /// <summary>
    /// Update missile UI text
    /// </summary>
    /// <param name="currentCount">Current missile count</param>
    /// <param name="maxCount">Maximum missile count</param>
    public void UpdateMissileUI(int currentCount, int maxCount)
    {
        if (missileText != null)
        {
            missileText.text = "Missile: " + currentCount + " / " + maxCount;
        }
    }
}