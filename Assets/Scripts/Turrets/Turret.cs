using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    public float maxSwing = 80f;
    public float turnSpeed = 10f;

    public GameObject matterPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float firePower = 0.5f;

    private float fireCooldown;
    private float angleDirection = 1f;
    private float currentOffset = 0f;
    private float initialAngle;

    void Start()
    {
        float rawAngle = transform.localEulerAngles.z;
        if (rawAngle > 180f) rawAngle -= 360f;

        float[] allowedAngles = { -90f, 90f, 180f };
        initialAngle = allowedAngles[0];
        float minDiff = Mathf.Abs(rawAngle - allowedAngles[0]);

        for (int i = 1; i < allowedAngles.Length; i++)
        {
            float diff = Mathf.Abs(rawAngle - allowedAngles[i]);
            if (diff < minDiff)
            {
                minDiff = diff;
                initialAngle = allowedAngles[i];
            }
        }
        transform.localRotation = Quaternion.Euler(0f, 0f, initialAngle);
    }

    void Update()
    {
        Oscillate();

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = 1f / fireRate;
        }
    }

    void Oscillate()
    {
        currentOffset += angleDirection * turnSpeed * Time.deltaTime;

        if (currentOffset > maxSwing)
        {
            currentOffset = maxSwing;
            angleDirection = -1f;
        }
        else if (currentOffset < -maxSwing)
        {
            currentOffset = -maxSwing;
            angleDirection = 1f;
        }

        float finalAngle = initialAngle + currentOffset;
        transform.localRotation = Quaternion.Euler(0f, 0f, finalAngle);
    }

    void Fire()
    {
        if (matterPrefab != null && firePoint != null)
        {
            GameObject spawned = Instantiate(matterPrefab, firePoint.position, firePoint.rotation);

            Rigidbody2D rb = spawned.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(firePoint.up * firePower, ForceMode2D.Impulse);
            }
        }
    }
}
