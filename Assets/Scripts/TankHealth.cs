using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TankHealth : MonoBehaviourPun
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
        Debug.Log($"Танк восстановил {amount} HP, текущее HP: {currentHealth}");
    }
    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        GameManager.Instance.OnPlayerDied();
        PhotonNetwork.Destroy(gameObject);
    }
}
