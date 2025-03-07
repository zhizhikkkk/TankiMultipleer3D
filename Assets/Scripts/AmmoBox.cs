using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField]private int ammoAmount = 5; 
    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        TankController tank = other.GetComponentInParent<TankController>();

        if (tank != null) 
        {
            tank.RefillAmmo(ammoAmount);
            Destroy(gameObject);
        }
    }
}
