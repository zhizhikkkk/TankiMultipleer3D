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
            Debug.Log("������������ � Photon...");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("�������� ����������� � Photon Master Server!");
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        string roomName = "Room_" + Random.Range(1000, 9999);
        RoomOptions options = new RoomOptions { MaxPlayers = 6 };
        PhotonNetwork.CreateRoom(roomName, options);
        Debug.Log($"������� �������: {roomName}");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"������� {PhotonNetwork.CurrentRoom.Name} ������� �������!");
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("���� ��������� �������...");
        }
        else
        {
            Debug.Log("�� � �����, �� ����� ������������!");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("�� ������� ��������� ������, ������� �����...");
        CreateRoom();
    }
}
