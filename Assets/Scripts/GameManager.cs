using UnityEngine;
using TMPro;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject modeSelectionPanel;
    [SerializeField] private GameObject mainPanel;
    public GameObject gameUI;
    [SerializeField] private GameObject gameOverPanel;

    [Header("UI Elements")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI ammoText; 

    private int enemyKillCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ShowNameInputPanel();
    }

    private void ShowNameInputPanel()
    {
        namePanel.SetActive(true);
        modeSelectionPanel.SetActive(false);
        mainPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameUI.SetActive(false);
    }

    public void OnNameEntered()
    {
        string playerName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName)) return;

        modeSelectionPanel.SetActive(true);
        namePanel.SetActive(false);
    }

    public void OnSinglePlayerClicked()
    {
        modeSelectionPanel.SetActive(false);
        SinglePlayerManager.Instance.StartGame();
    }

    public void IncreaseKillCount()
    {
        enemyKillCount++;
    }

    public void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";
        }
    }

    public void OnMultiplayerClicked()
    {
        modeSelectionPanel.SetActive(false);
        MultiplayerManager.Instance.StartMultiplayerGame();
    }

    public void OnPlayerDied()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = $"Вы проиграли!\nУничтожено ботов: {enemyKillCount}";
    }

    public void OnExitClicked()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
