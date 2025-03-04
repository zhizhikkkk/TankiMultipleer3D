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
        Debug.Log($" ���� ��������. IsMine: {photonView.IsMine}");
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($" �������� �������� � {other.gameObject.name}. IsMine: {photonView.IsMine}");

        PhotonView targetPhotonView = other.GetComponentInParent<PhotonView>();

        if (targetPhotonView == null)
        {
            Debug.LogError($" � ������� {other.gameObject.name} � ��� ��������� ��� PhotonView!");
            return;
        }

        Debug.Log($" ������ PhotonView � {other.gameObject.name}, ViewID: {targetPhotonView.ViewID}");

        if (other.CompareTag("Tank"))
        {
            Debug.Log($" ��������� � ���� {other.gameObject.name}!");

            targetPhotonView.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damage);
        }

        PhotonNetwork.Destroy(gameObject);
    }



}
