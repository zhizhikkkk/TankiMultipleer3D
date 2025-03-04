using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviourPun
{
    public float speed = 20f;
    public float lifetime = 3f;
    public int damage = 25;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position; 
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Udar");
        if (!photonView.IsMine) return;

        if (other.CompareTag("Tank"))
        {
            Debug.Log($"Попадание в танк {other.gameObject.name}!");
            other.GetComponent<TankHealth>().TakeDamage(damage);
            PhotonNetwork.Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Debug.Log("Пуля ударилась о стену!");
           
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
