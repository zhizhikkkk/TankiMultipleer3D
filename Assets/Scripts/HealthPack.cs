using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField]private int healAmount = 30;

    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tank"))
        {
            TankHealth tankHealth = other.GetComponentInParent<TankHealth>();
            if (tankHealth != null)
            {
                tankHealth.Heal(healAmount); 
                Destroy(gameObject); 
            }
        }
    }
}
