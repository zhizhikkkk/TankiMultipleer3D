using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public static MultiplayerManager Instance;

    [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject gameUI;
    public GameObject gameOverPanel;

    [Header("Multiplayer Settings")]
    public GameObject playerTankPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartMultiplayerGame()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnCreateRoomClicked()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            string roomName = "Room_" + Random.Range(1000, 9999);
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 4 });
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
        Debug.Log("Подключено к Photon Master Server!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string newRoom = "Room_" + Random.Range(1000, 9999);
        PhotonNetwork.CreateRoom(newRoom, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Вошли в комнату: {PhotonNetwork.CurrentRoom.Name}");
        gameUI.SetActive(true);

        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("TankSpawned"))
        {
            GameObject tank = PhotonNetwork.Instantiate(playerTankPrefab.name, GetSpawnPosition(), Quaternion.identity);
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties["TankSpawned"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }

    public void OnPlayerDied()
    {
        gameOverPanel.SetActive(true);
        StartCoroutine(LeaveRoomAfterDelay(3f));
    }

    private IEnumerator LeaveRoomAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Игрок покинул комнату.");
        PhotonNetwork.LocalPlayer.CustomProperties["TankSpawned"] = false;
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
