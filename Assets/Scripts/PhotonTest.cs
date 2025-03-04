using UnityEngine;
using Photon.Pun;

public class PhotonTest : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("�������� ����������� � Photon!");
    }
}
