using UnityEngine;
using Photon.Pun;

public class TankController : MonoBehaviourPun
{
    public float speed = 5f;
    public float rotationSpeed = 100f;
    public Transform turret;
    public Transform firePoint;
    public GameObject projectilePrefab;

    private Rigidbody rb;
    private float fireRate = 0.5f;
    private float nextFireTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (!photonView.IsMine)
        {
            enabled = false;
        }
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float rotate = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        rb.MovePosition(rb.position + transform.forward * move);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotate, 0));
        RotateTurret();
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }
    void Fire()
    {
        if (!PhotonNetwork.InRoom) return;

        Quaternion rotatedFireRotation = firePoint.rotation * Quaternion.Euler(90, 0, 0);

        GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, firePoint.position, rotatedFireRotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = projectile.transform.forward * 20f;
    }


    void RotateTurret()
    {
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, turret.position);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);
            targetPoint.y = turret.position.y;

            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - turret.position);

            turret.rotation = Quaternion.Lerp(turret.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }



}
