using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthController : MonoBehaviour
{
    private float _maxHealth = 100f; // Maksimum sağlık
    public float _currentHealth; // Mevcut sağlık
    [SerializeField] private Image _healthBarFill; // Sağlık barı UI'si
    [SerializeField] private float _damageAmount = 10f; // Hasar miktarı
    [SerializeField] private Transform _respawnPoint; // Yeniden doğma noktası
    //[SerializeField] private TextMeshProUGUI _healthText; // Sağlık metni
    [SerializeField] private Transform player; // Oyuncu referansı

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _currentHealth = _maxHealth; // Başlangıçta sağlık tam dolu
        UpdateHealthBar(); // Sağlık barını güncelle
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            TakeDamage(_damageAmount);
        }
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount; // Hasar uygula
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth); // Sağlığı sınırla
        UpdateHealthBar(); // Sağlık barını güncelle

        if (_currentHealth == 0)
        {
            Die();
            // Ölüm işlevini çağır
        }
    }

    private void UpdateHealthBar()
    {
        _healthBarFill.fillAmount = _currentHealth / _maxHealth; // Sağlık barını güncelle

        // Sağlık barının rengini güncelle
        if (_healthBarFill != null)
        {
            if (_currentHealth / _maxHealth > 0.5f) // Sağlık %50'den fazla
            {
                _healthBarFill.color = Color.green; // Yeşil renk
            }
            else if (_currentHealth / _maxHealth > 0.2f) // Sağlık %20-%50 arası
            {
                _healthBarFill.color = Color.yellow; // Sarı renk
            }
            else // Sağlık %20'den az
            {
                _healthBarFill.color = Color.red; // Kırmızı renk
            }
        }
    }

    private void Die()
    {
        Debug.Log("Karakter öldü!");
        StartCoroutine(Respawn(0.5f)); // Yeniden doğma süresini belirleyin
    }

    IEnumerator Respawn(float duration)
    {
        if (_respawnPoint != null)
        {
            yield return new WaitForSeconds(duration);
            transform.position = _respawnPoint.position; // Yeniden doğma noktasına taşın
        }
        else
        {
            Debug.LogWarning("Respawn noktası atanmadı!");
        }

        _currentHealth = _maxHealth; // Sağlığı sıfırla
        UpdateHealthBar(); // Sağlık barını yenile
    }

    public void UpdateCheckpoint(Transform checkpoint)
    {
        _respawnPoint = checkpoint; // Respawn noktasını Transform olarak güncelle
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    private void Update()
    {
        // Oyuncunun pozisyonunu kontrol et -50 den aşağı olursa respawn çalışır
        if (player != null && player.position.y < -50f) // Oyuncu mevcutsa ve düşme durumu varsa
        {
            Respawn(0.5f); // Belirli bir süre ile yeniden doğma işlemi başlat
        }
    }
    public void addhealth()
    {
        _currentHealth +=40f;
        UpdateHealthBar();
    }
}
