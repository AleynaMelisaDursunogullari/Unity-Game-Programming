using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TMP_Text coinText; // TextMesh Pro referansı
    public int coinCount = 0; // Toplam coin sayısı
    public Transform player; // Oyuncunun Transform'u
    public Vector3 offset;
    private CinemachineVirtualCamera virtualCamera; // Cinemachine Virtual Camera referansı
    public GameObject childObject; // Oyuncunun kafasındaki obje (örneğin şapka)

    private Vector3 startingPosition = new Vector3(-5f, -1f, 0f); // Oyuncunun başlama konumu

    void Awake()
    {
         
        // Oyun başladığında coin sayısını sıfırlamak
    

    // Mevcut coin sayısını yükle
    

    // Singleton yapılandırması
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // Sahne değişiminde yok edilmez
    }
    else
    {
        Destroy(gameObject); // Zaten bir GameManager varsa yenisini yok et
        return;
    }

    // TextMesh Pro ayarlarını yap
    SetupCoinText();
    }

    private void SetupCoinText()
    {
        if (coinText != null) // coinText bağlı mı kontrol et
        {
            // UI pozisyonu, boyut ayarı
            RectTransform rectTransform = coinText.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(40f, 650f); // UI pozisyonunu ayarla
                rectTransform.sizeDelta = new Vector2(200f, 200f); // Genişlik ve yükseklik
            }

            // Yazı tipi boyutu ve stili
            coinText.fontSize = 36; // Yazı tipi boyutu
            coinText.alignment = TextAlignmentOptions.Center; // Ortala
            coinText.color = Color.yellow; // Yazı rengi
        }
        else
        {
            Debug.LogWarning("coinText referansı atanmamış! Bu normal bir durum olabilir.");
        }
    }

    public void ChangeScene(string sceneName)
    {
        // Sahne değişmeden önce coin sayısını kaydedin
        PlayerPrefs.SetInt("CoinCount", coinCount);
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetPlayerPosition());

        if (virtualCamera == null)
        {
            GameObject cameraObject = GameObject.FindWithTag("VirtualCamera");
            if (cameraObject != null)
            {
                virtualCamera = cameraObject.GetComponent<CinemachineVirtualCamera>();
            }
        }

        if (virtualCamera != null && player != null)
        {
            virtualCamera.Follow = player;
            virtualCamera.LookAt = player;
        }

        UpdateCoinText();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator SetPlayerPosition()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        if (player != null)
        {
            yield return new WaitForSeconds(0.1f); // Sahne yüklendikten sonra konum ayarlaması yapalım
            player.position = startingPosition;
        }
        else
        {
            Debug.LogError("Oyuncu objesi bulunamadı!");
        }
    }

    public void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = ": " + coinCount; // Coin sayısını güncelle
        }
    }

    public void CollectCoin(GameObject coinObject)
    {
        // Geçerli coin tag'ine sahip nesneleri kontrol et
        if (coinObject.CompareTag("gold"))
        {
            IncreaseCoin(1); // Coin sayısını arttır
        }
        else
        {
            Debug.LogWarning("Geçersiz tag: " + coinObject.tag + " (Coin sayısı artmadı)");
        }
    }

    // Coin sayısını arttırmak için fonksiyon
    public void IncreaseCoin(int value)
    {
        coinCount += value;
        UpdateCoinText(); // UI'yı güncelle
        Debug.Log("Coin Count: " + coinCount);
    }
}
