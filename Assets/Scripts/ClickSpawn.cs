using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ClickSpawn : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject upgradeMenu;
    public Button upgradeRateBTN;
    public Button upgradeQTYBTN;
    public Button upgradeValueBTN;
    public Blackhole blackholeScript;

    private Collider2D spawnAreaCollider;
    private int spawnQuantity = 1;
    private float spawnQTYCost = 20;
    private float spawnQTYscale = 2.4f;
    private int spawnQTYMax = 5;
    private float spawnRate = 0.5f;
    private float spawnRateMax = 20;
    private float spawnCooldown = 0;

    private float spawnUpgradeCost = 5;
    private float spawnRateScale = 1.3f;
    private float spawnCostScale = 2.2f;

    private float matterValue = 1;
    private float matterValueScale = 3.8f;
    private float matterValueMax = 25;
    private float matterValueCost = 35;
    private float matterValueCostScale = 2.6f;




    void Awake()
    {
        spawnAreaCollider = GetComponent<Collider2D>();
        spawnCooldown = 1f / spawnRate;
        upgradeMenu.SetActive(false);
    }

    void Update()
    {
        if (spawnCooldown <= 0f && Mouse.current.leftButton.IsPressed())
        {
            SpawnObject();
            spawnCooldown = 1f / spawnRate;
        }
        else
        {
            spawnCooldown -= Time.deltaTime;
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            OpenUpgradeMenu();
        }

        if (upgradeMenu.activeSelf && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!IsPointerOverUIObject())
            {
                upgradeMenu.SetActive(false);
            }
        }
    }

    public void IncreaseMatterValue()
    {
        if (blackholeScript.GetBalance() < matterValueCost)
        {
            return;
        }
        else
        {
            blackholeScript.RemoveMoney(matterValueCost);
            matterValue *= matterValueScale;
            matterValueCost *= matterValueCostScale;
            if (matterValue >= matterValueMax)
            {
                matterValue = matterValueMax;
                upgradeValueBTN.interactable = false;
            }
        }
    }
    public void IncreaseSpawnRate()
    {
        if (blackholeScript.GetBalance() < spawnUpgradeCost)
        {
            return;
        }
        else
        {
            blackholeScript.RemoveMoney(spawnUpgradeCost);
            spawnRate *= spawnRateScale;
            spawnUpgradeCost *= spawnCostScale;
            if (spawnRate >= spawnRateMax)
            {
                upgradeRateBTN.interactable = false;
            }
        }
    }

    public void IncreaseSpawnQTY()
    {
        if (blackholeScript.GetBalance() < spawnQTYCost)
        {
            return;
        }
        else
        {
            blackholeScript.RemoveMoney(spawnQTYCost);
            spawnQuantity += 1;
            spawnQTYCost *= spawnQTYscale;
            if (spawnQuantity >= spawnQTYMax)
            {
                upgradeQTYBTN.interactable = false;
            }
        }

    }


    private bool IsPointerOverUIObject()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    void OpenUpgradeMenu()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (spawnAreaCollider != null && spawnAreaCollider.OverlapPoint(worldPos))
        {
            Vector2 offset = new Vector2(-1, 1);
            Vector2 menuPos = (Vector2)worldPos - offset;
            upgradeMenu.transform.position = menuPos;
            upgradeMenu.SetActive(true);
        }
        else return;

    }

    void SpawnObject()
    {
        for (int i = 0; i < spawnQuantity; i++)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 spawnPos = (Vector2)worldPos;
            float randZ = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(0, 0, randZ);

            if (spawnAreaCollider != null && spawnAreaCollider.OverlapPoint(spawnPos))
            {
                GameObject newMatter = Instantiate(objectToSpawn, new Vector2(spawnPos.x, spawnPos.y), rotation);
                newMatter.GetComponent<Matter>().SetValue(matterValue);
            }
        }
    }
}
