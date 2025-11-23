using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void onClick()
    {
        // Reset score when restarting
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
