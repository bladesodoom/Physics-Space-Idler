using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ClickSpawn : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject upgradeMenu;

    public Blackhole blackholeScript;

    private Collider2D spawnAreaCollider;
    private int spawnQuantity = 1;
    private float spawnRate = 1.0f;
    private float spawnCooldown = 0;

    private float spawnUpgradeCost = 5;
    private float spawnRateScale = 1.3f;
    private float spawnCostScale = 1.2f;


    void Awake()
    {
        spawnAreaCollider = GetComponent<Collider2D>();
        spawnCooldown = 1f / spawnRate;
        upgradeMenu.SetActive(false);
    }

    void Update()
    {
        if (spawnCooldown <= 0f && Mouse.current.leftButton.wasPressedThisFrame)
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
            Vector2 menuPos = (Vector2)worldPos;
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
                Instantiate(objectToSpawn, new Vector2(spawnPos.x, spawnPos.y), rotation);
            }
        }
    }
}
