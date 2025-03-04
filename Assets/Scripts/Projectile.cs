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
        Debug.Log($" Пуля запущена. IsMine: {photonView.IsMine}");
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($" Проверка коллизии с {other.gameObject.name}. IsMine: {photonView.IsMine}");

        PhotonView targetPhotonView = other.GetComponentInParent<PhotonView>();

        if (targetPhotonView == null)
        {
            Debug.LogError($" У объекта {other.gameObject.name} и его родителей нет PhotonView!");
            return;
        }

        Debug.Log($" Найден PhotonView у {other.gameObject.name}, ViewID: {targetPhotonView.ViewID}");

        if (other.CompareTag("Tank"))
        {
            Debug.Log($" Попадание в танк {other.gameObject.name}!");

            targetPhotonView.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damage);
        }

        PhotonNetwork.Destroy(gameObject);
    }



}
