using UnityEngine;
using UnityEngine.SceneManagement; // Penting: Tambahkan ini!

public class menumanager : MonoBehaviour
{
    public string Day2 = "Day 2";

    public void MulaiPermainan()
    {
        SceneManager.LoadScene(Day2);
    }

    public void KeluarPermainan()
    {
        Application.Quit();
        Debug.Log("Keluar Game");
    }
}