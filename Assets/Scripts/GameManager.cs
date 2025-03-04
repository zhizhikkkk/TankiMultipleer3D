using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance; 

    public GameObject gameOverPanel; 
    public TextMeshProUGUI gameOverText;
    public GameObject mainPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GameOver(bool isWinner)
    {
        gameOverPanel.SetActive(true); 
        gameOverText.text = isWinner ? "Ты победил!" : "Ты проиграл!";

        Invoke("LeaveGame", 3f);
    }

    void LeaveGame()
    {
        mainPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        
    }
}
