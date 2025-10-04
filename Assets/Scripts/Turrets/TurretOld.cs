using UnityEngine;
using UnityEngine.UI;

public class TurretOld : MonoBehaviour
{
    public GameObject matterPrefab;
    public Transform firePoint;
    public Button upgradeFireRateBTN;
    public Button upgradeMatterValueBTN;

    private float fireRate = 0.5f;
    private float firePower = 0.5f;
    private float fireRateMax = 30;
    private float fireRateScale = 1.36f;
    private float fireRateCost = 70;
    private float fireRateCostScale = 1.28f;
    private float fireCooldown;
    private float matterValue = 1f;
    private float matterValueMax = 15f;
    private float matterValueScale = 1.1f;
    private float matterValueCost = 50f;
    private float matterValueCostScale = 1.6f;

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = 1f / fireRate;
        }
    }

    public void IncreaseFireRate()
    {
        fireRate *= fireRateScale;
        fireRateCost *= fireRateCostScale;
        if (fireRate >= fireRateMax)
        {
            fireRate = fireRateMax;
            upgradeFireRateBTN.interactable = false;
        }
    }

    public void IncreaseMatterValue()
    {
        matterValue *= matterValueScale;
        matterValueCost *= matterValueCostScale;
        if (matterValue >= matterValueMax)
        {
            matterValue = matterValueMax;
            upgradeMatterValueBTN.interactable = false;
        }
    }

    void Fire()
    {
        if (matterPrefab != null && firePoint != null)
        {
            GameObject spawned = Instantiate(matterPrefab, firePoint.position, firePoint.rotation);
            spawned.GetComponent<MatterOld>().SetValue(matterValue);

            Rigidbody2D rb = spawned.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(firePoint.up * firePower, ForceMode2D.Impulse);
            }
        }
    }
}
