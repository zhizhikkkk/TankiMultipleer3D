using Photon.Pun;
using UnityEngine;
using System.Collections;

public class TankSpawner : MonoBehaviourPunCallbacks
{
    public static TankSpawner Instance;
    public GameObject mainPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public override void OnJoinedRoom()
    {
        mainPanel.SetActive(false);
        Vector3 spawnPosition = GetValidSpawnPosition();

        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("TankSpawned") ||
            !(bool)PhotonNetwork.LocalPlayer.CustomProperties["TankSpawned"])
        {
            GameObject tank = PhotonNetwork.Instantiate("Tank", spawnPosition, Quaternion.identity);
            StartCoroutine(WaitForOwnershipAndSetColor(tank));

            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["TankSpawned"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }
        else
        {
            Debug.LogWarning("Этот игрок уже создал танк, не спавним новый.");
        }
    }




    private IEnumerator WaitForOwnershipAndSetColor(GameObject tank)
    {
        PhotonView tankPhotonView = tank.GetComponent<PhotonView>();

        while (tankPhotonView != null && !tankPhotonView.IsMine)
        {
            yield return null;
        }

        if (PlayerColorManager.Instance != null)
        {
            PlayerColorManager.Instance.AssignUniqueColor(tank);
        }
        else
        {
            Debug.LogError(" PlayerColorManager.Instance не найден!");
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

            Collider[] colliders = Physics.OverlapSphere(spawnPos, 1.5f);
            bool isSafe = true;

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Wall") || col.CompareTag("Tank"))
                {
                    isSafe = false;
                    break;
                }
            }

            if (isSafe) return spawnPos;
        }

        Debug.LogWarning(" Не удалось найти безопасное место для спавна, используем центр.");
        return new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
    }
}
