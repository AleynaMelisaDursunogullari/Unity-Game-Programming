
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_Door : MonoBehaviour
{
    public string level22; // Bir sonraki sahnenin adı

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Çarpan nesnenin tag'i "Player" ise ve kapının tag'i "next_level" ise sahneyi değiştir
        if (collision.CompareTag("Player") && gameObject.CompareTag("next_level"))
        {
            Debug.Log("Kapıya çarpıldı, sahne değişiyor...");
            GameManager.Instance.ChangeScene(level22);
        }
    }
}
