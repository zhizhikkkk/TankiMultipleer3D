using Photon.Pun;
using UnityEngine;
using System.Collections;

public class PlayerColorManager : MonoBehaviourPunCallbacks
{
    public static PlayerColorManager Instance;

    private static readonly Color[] availableColors = {
        Color.blue, Color.gray, Color.green, Color.yellow, Color.cyan, Color.magenta
    };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AssignUniqueColor(GameObject tank)
    {
        PhotonView tankPhotonView = tank.GetComponent<PhotonView>();

        if (tankPhotonView == null)
        {
            Debug.LogError(" PhotonView не найден у танка!");
            return;
        }

        if (!tankPhotonView.IsMine)
        {
            Debug.LogWarning(" Этот игрок не владеет танком, пропускаем установку цвета.");
            return;
        }

        Color assignedColor = GetUniqueColor();
        Debug.Log($" Назначен цвет: {assignedColor}");

        StartCoroutine(WaitAndSetColor(tankPhotonView, assignedColor));
    }

    private IEnumerator WaitAndSetColor(PhotonView tankPhotonView, Color assignedColor)
    {
        yield return new WaitForSeconds(0.1f); 

        tankPhotonView.RPC("SetColorRPC", RpcTarget.AllBuffered, assignedColor.r, assignedColor.g, assignedColor.b);
    }

    private Color GetUniqueColor()
    {
        bool[] usedColors = new bool[availableColors.Length];

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("TankColor"))
            {
                float[] takenColorData = (float[])player.CustomProperties["TankColor"];
                Color takenColor = new Color(takenColorData[0], takenColorData[1], takenColorData[2]);

                for (int i = 0; i < availableColors.Length; i++)
                {
                    if (availableColors[i] == takenColor) usedColors[i] = true;
                }
            }
        }

        for (int i = 0; i < availableColors.Length; i++)
        {
            if (!usedColors[i])
            {
                float[] colorData = new float[] { availableColors[i].r, availableColors[i].g, availableColors[i].b };

                ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
                playerProperties["TankColor"] = colorData;
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

                return availableColors[i];
            }
        }

        return Color.white;
    }
}
