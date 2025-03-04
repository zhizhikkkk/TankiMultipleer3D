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
    public void TakeDamageRPC(int damage)
    {
        Debug.Log($" {gameObject.name} получил {damage} урона! Текущее HP: {currentHealth}");

        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        Debug.Log($" Танк {gameObject.name} уничтожен!");

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.PlayerList.Length == 1)
            {
                GameManager.Instance.GameOver(true);
            }
            else
            {
                GameManager.Instance.GameOver(false);
            }
        }

        PhotonNetwork.Destroy(gameObject);
    }
}
