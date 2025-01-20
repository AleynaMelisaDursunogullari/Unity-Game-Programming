using UnityEngine;
using TMPro;
using System.Collections;

public class TextUpdater : MonoBehaviour
{
    public int score = 0; // Skor değişkeni
    [SerializeField] private TextMeshProUGUI text; // TextMeshPro referansı
    private Transform textTransform; // Text'in transform referansı
    private Transform playerTransform; // Oyuncunun transform referansı
    public HealthController _currentHealth;
    private Vector3 originalPosition; // Text'in orijinal pozisyonu

    private void Start()
    {
        _currentHealth = FindObjectOfType<HealthController>();

        // Text referansı kontrolü
        if (text == null)
        {
            Debug.LogError("Text (TMP) referansı atanmadı! Inspector'dan bağlayın.");
            return;
        }

        // Transform referanslarını al
        playerTransform = transform;
        textTransform = text.transform;

        // Text'in başlangıç pozisyonunu kaydet
        originalPosition = textTransform.localPosition;

        // Yukarı-aşağı hareket için Coroutine başlat
        StartCoroutine(MoveTextUpDown());
    }

    private void Update()
    {
        // Oyuncunun dönüşünü kontrol et
        if (playerTransform != null && textTransform != null)
        {
            // Yazının yönünü sabitle
            Vector3 textScale = textTransform.localScale;

            if (playerTransform.localScale.x < 0)
            {
                // Oyuncu sola bakıyorsa yazının X ölçeğini pozitif yap
                textTransform.localScale = new Vector3(Mathf.Abs(textScale.x), textScale.y);
            }
            else
            {
                // Oyuncu sağa bakıyorsa yazının X ölçeğini pozitif yap
                textTransform.localScale = new Vector3(Mathf.Abs(textScale.x), textScale.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("gold"))
        {
            score += 10; // Skoru artır
            UpdateText(); // Metni güncelle
        }

        if (collision.GetType() == typeof(BoxCollider2D))
        {
            if (collision.CompareTag("Spike"))
            {
                score -= 5;
                UpdateText();
            }
        }

        if (_currentHealth != null && _currentHealth.GetCurrentHealth() <= 0)
        {
            score = 0;
            UpdateText();
        }
    }

    private void UpdateText()
    {
        if (text != null)
        {
            text.text = score.ToString(); // Metni güncelle
        }
    }

    private IEnumerator MoveTextUpDown()
    {
        float moveAmount = 0.2f; // Yukarı aşağı hareket miktarı
        float moveSpeed = 2f;    // Hareket hızı

        while (true)
        {
            // Yukarı hareket
            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                textTransform.localPosition = originalPosition + new Vector3(0, Mathf.Lerp(0, moveAmount, elapsedTime), 0);
                elapsedTime += Time.deltaTime * moveSpeed;
                yield return null;
            }

            // Aşağı hareket
            elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                textTransform.localPosition = originalPosition + new Vector3(0, Mathf.Lerp(moveAmount, 0, elapsedTime), 0);
                elapsedTime += Time.deltaTime * moveSpeed;
                yield return null;
            }
        }
        
    }
}
