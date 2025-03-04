using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Подключаемся к Photon...");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Успешное подключение к Photon Master Server!");
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        string roomName = "Room_" + Random.Range(1000, 9999);
        RoomOptions options = new RoomOptions { MaxPlayers = 6 };
        PhotonNetwork.CreateRoom(roomName, options);
        Debug.Log($"Создаем комнату: {roomName}");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"Комната {PhotonNetwork.CurrentRoom.Name} успешно создана!");
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("Ищем случайную комнату...");
        }
        else
        {
            Debug.Log("Не в лобби, не можем подключиться!");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Не найдено доступных комнат, создаем новую...");
        CreateRoom();
    }
}
