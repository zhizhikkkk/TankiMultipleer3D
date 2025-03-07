using UnityEngine;

public class TankController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 100f;
    public Transform turret;
    public Transform firePoint;
    public GameObject projectilePrefab;

    public int maxAmmo = 10;
    private int currentAmmo;

    private float fireRate = 0.5f;
    private float nextFireTime = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
        GameManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);

    }

    void Update()
    {
        MoveTank();
        RotateTurret();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void MoveTank()
    {
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float rotate = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        transform.Translate(Vector3.forward * move);
        transform.Rotate(Vector3.up * rotate);
    }

    void RotateTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, turret.position);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);
            targetPoint.y = turret.position.y;
            turret.rotation = Quaternion.Lerp(turret.rotation, Quaternion.LookRotation(targetPoint - turret.position), Time.deltaTime * 10f);
        }
    }

    void Fire()
    {
        if (currentAmmo <= 0) return;

        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        currentAmmo--;
        GameManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);
    }

    public void RefillAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        GameManager.Instance.UpdateAmmoUI(currentAmmo, maxAmmo);
    }

   
}
