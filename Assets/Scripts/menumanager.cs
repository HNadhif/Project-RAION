using UnityEngine;
using UnityEngine.SceneManagement; // Penting: Tambahkan ini!

public class menumanager : MonoBehaviour
{
    // Pastikan nama scene utama game Anda (scene yang ada pesawatnya) sudah benar
    public string Day2 = "Day 2";

    // Fungsi ini akan dipanggil saat tombol diklik
    public void MulaiPermainan()
    {
        // Memuat scene yang namanya disimpan di variabel namaSceneGameUtama
        SceneManager.LoadScene(Day2);
    }

    // Fungsi opsional untuk keluar dari game (hanya berfungsi di build)
    public void KeluarPermainan()
    {
        Application.Quit();
        Debug.Log("Keluar Game"); // Log ini akan muncul di editor
    }
}