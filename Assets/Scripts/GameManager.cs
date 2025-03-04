using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject namePanel;
    public GameObject mainPanel;
    public GameObject gameOverPanel;
    public GameObject gameUI;

    [Header("UI Elements")]
    public TMP_InputField nameInput;
    public TextMeshProUGUI gameOverText;

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
        mainPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameUI.SetActive(false);
    }

    public void OnNameEntered()
    {
        string playerName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName)) return;

        PhotonNetwork.NickName = playerName;

        namePanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void OnCreateRoomClicked()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.CreateRoom("Room_" + Random.Range(1000, 9999), new RoomOptions { MaxPlayers = 4 });
        }

        mainPanel.SetActive(false);
    }

    public void OnJoinRoomClicked()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }

        mainPanel.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("���������� � Photon Master Server!");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        gameUI.SetActive(true);

        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Score"))
        {
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["Score"] = 0;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }
    }


    private Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }

    public void OnPlayerDied()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "�� ���������!";

        ShowScoreTable(); 
    }
    void ShowScoreTable()
    {
        string scoreText = "������� �����:\n";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int score = player.CustomProperties.ContainsKey("Score") ? (int)player.CustomProperties["Score"] : 0;
            scoreText += $"{player.NickName}: {score} �����\n";
        }

        gameOverText.text += "\n" + scoreText;
    }


    private IEnumerator LeaveRoomAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {

        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties["TankSpawned"] = false;
        playerProperties["Score"] = 0; 
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        gameUI.SetActive(false);
        gameOverPanel.SetActive(false);
        mainPanel.SetActive(true);
    }



    public void OnExitClicked()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }


}
