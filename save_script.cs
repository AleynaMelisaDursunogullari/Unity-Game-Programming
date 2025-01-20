using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // Dosya işlemleri için gerekli
using UnityEngine.UI;  // UI elemanlarını kullanabilmek için gerekli
using UnityEngine.SceneManagement;

public class save_script : MonoBehaviour
{
    private string saveLocation;
    private bool isLoadTrue = false;
    public GameObject gameController; // GameController GameObject'i

    [System.Serializable]
    public class SaveData
    {
        public Vector3 playerPosition; // Oyuncunun pozisyonu
        public int coinCount;
         public string sceneName; // Sahne ismi
    }

    public GameObject menuPanel;   // Menü Paneli (Save/Load menüsü)
    public Button saveButton;
    public Button continueButton;     // Save butonu

    private bool isMenuOpen = false;

    void Start()
    {
        if (gameController != null)
        {
            DontDestroyOnLoad(gameController);
        }
        else
        {
            Debug.LogError("GameController objesi bulunamadı!");
        }

        // Menü başlangıçta gizli olmalı
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Menu Panel referansı atanmadı!");
        }

        // Butonlara işlevsellik ekleyin
        saveButton.onClick.AddListener(SaveGame); // Save butonuna tıklandığında SaveGame fonksiyonu çalışacak
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        continueButton.onClick.AddListener(ContinueGame);

        Debug.Log("Kayıt dosyasının yolu: " + saveLocation);
    }

    void Update()
    {
        // Escape tuşuna basıldığında menüyü aç/kapat
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        if (menuPanel == null)
        {
            Debug.LogError("Menu Panel referansı atanmadı!");
            return;
        }

        isMenuOpen = !isMenuOpen; // Menü durumu tersine çevriliyor

        menuPanel.SetActive(isMenuOpen); // Menü görünürlüğünü değiştir

        if (isMenuOpen)
        {
            Time.timeScale = 0f; // Menü açıldığında oyunu durdur
        }
        else
        {
            Time.timeScale = 1f; // Menü kapandığında oyunu devam ettir
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        isMenuOpen = false;
    }
    
    public void ExitGame()
    {
        Debug.Log("Oyun kapatılıyor...");
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Unity Editörü üzerinden oyunu durdurur
        #else
        Application.Quit(); // Derlenmiş sürümde uygulamayı kapatır
        #endif
    }
    public void SaveGame()
    {
        // Kaydedilecek veriyi oluştur
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            coinCount = GameManager.Instance.coinCount,
             sceneName = SceneManager.GetActiveScene().name // Aktif sahne ismini alıyoruz
        };

        // Veriyi JSON formatına çevir ve dosyaya yaz
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
        Debug.Log($"Oyun kaydedildi: {saveLocation}");
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                player.transform.position = saveData.playerPosition;
            }
            else
            {
                Debug.LogError("Player bulunamadı!");
            }

            GameManager.Instance.coinCount = saveData.coinCount;
            Debug.Log("Oyun başarıyla yüklendi.");
        }
        else
        {
            Debug.LogWarning("Kayıt dosyası bulunamadı, yeni bir kayıt oluşturuluyor...");
            SaveGame();
        }
    }
}
