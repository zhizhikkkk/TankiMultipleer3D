using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TankHealth : MonoBehaviourPun
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
    }

    [PunRPC]
    public void TakeDamageRPC(int damage, int attackerID)
    {
        currentHealth -= damage;
        UpdateHealthUI();
        Debug.Log($" {gameObject.name} получил {damage} урона! Текущее HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die(attackerID);
        }
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }


    void Die(int attackerID)
    {
        Debug.Log($"Танк {gameObject.name} уничтожен!");

        PhotonView attacker = PhotonView.Find(attackerID);
        if (attacker != null && attacker.Owner != null)
        {
            int currentScore = (int)attacker.Owner.CustomProperties["Score"];
            ExitGames.Client.Photon.Hashtable newScore = new ExitGames.Client.Photon.Hashtable();
            newScore["Score"] = currentScore + 10; 
            attacker.Owner.SetCustomProperties(newScore);
        }

        if (photonView.IsMine)
        {
            GameManager.Instance.OnPlayerDied();
            PhotonNetwork.Destroy(gameObject);
        }
    }

}
