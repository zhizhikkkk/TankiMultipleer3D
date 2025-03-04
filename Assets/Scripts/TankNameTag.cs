using Photon.Pun;
using TMPro;
using UnityEngine;

public class TankNameTag : MonoBehaviourPun
{
    public TextMeshProUGUI nicknameText; 

    void Start()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SetNicknameRPC", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
    }

    [PunRPC]
    void SetNicknameRPC(string nickname)
    {
        if (nicknameText != null)
        {
            nicknameText.text = nickname;
        }
    }
}
