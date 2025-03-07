using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviourPun
{
    public float speed = 20f;
    public float lifetime = 3f;
    public int damage = 25;
    public GameObject explosionEffect;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
        Debug.Log($"Пуля запущена. IsMine: {photonView.IsMine}");
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Попадание в {other.gameObject.name}. IsMine: {photonView.IsMine}");

        PhotonView targetPhotonView = other.GetComponentInParent<PhotonView>();

        if (targetPhotonView == null)
        {
            Debug.Log($"У объекта {other.gameObject.name} нет PhotonView!");
            return;
        }

        if (other.CompareTag("Tank"))
        {
            targetPhotonView.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damage, photonView.ViewID);
        }

        if (other.CompareTag("Enemy"))
        {
            Transform rootObject = other.transform.root;
            Destroy(rootObject.gameObject);
        }

        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 1f); 
        }

        PhotonNetwork.Destroy(gameObject);
    }
}
