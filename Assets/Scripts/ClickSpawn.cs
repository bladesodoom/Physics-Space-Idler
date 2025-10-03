using TMPro;

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

    public TextMeshProUGUI rateCostText;
    public TextMeshProUGUI rateChangeText;

    public TextMeshProUGUI quantityCostText;
    public TextMeshProUGUI quantityChangeText;

    public TextMeshProUGUI valueCostText;
    public TextMeshProUGUI valueChangeText;

    private Collider2D spawnAreaCollider;

    private int spawnQuantity = 1;
    private float spawnQTYCost = 20;
    private float spawnQTYscale = 2.4f;
    private int spawnQTYMax = 5;

    private float spawnRate = 0.5f;
    private float spawnRateMax = 20;
    private float spawnCooldown = 0;
    private float spawnRateCost = 5;
    private float spawnRateScale = 1.3f;
    private float spawnCostScale = 2.2f;

    private float matterValue = 1;
    private float matterValueScale = 3.8f;
    private float matterValueMax = 25;
    private float matterValueCost = 35;
    private float matterValueCostScale = 2.6f;

    /* TODO 
        - Make upgrade buttons say "MAX" showing the last upgrade value of the quantity, rate etc
        - Make second to last upgrade show the max value on the right side of the ">"
    */


    void Awake()
    {
        spawnAreaCollider = GetComponent<Collider2D>();
        spawnCooldown = 1f / spawnRate;
        upgradeMenu.SetActive(false);
        upgradeQTYBTN.interactable = false;
        upgradeRateBTN.interactable = false;
        upgradeValueBTN.interactable = false;
        UpdateText();
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
        CompareBalanceCost();
    }

    private void CompareBalanceCost()
    {
        float currentBalance = blackholeScript.GetBalance();

        if (currentBalance >= spawnRateCost)
        {
            upgradeRateBTN.interactable = true;
        }
        else if (currentBalance >= spawnQTYCost)
        {
            upgradeQTYBTN.interactable = true;
        }
        else if (currentBalance >= matterValueCost)
        {
            upgradeValueBTN.interactable = true;
        }
        else
        {
            upgradeRateBTN.interactable = false;
            upgradeValueBTN.interactable = false;
            upgradeQTYBTN.interactable = false;
        }
    }
    private void UpdateText()
    {
        float newRateText = (float)System.Math.Round(spawnRate * spawnRateScale, 2);

        rateCostText.text = "Cost: " + System.Math.Round(spawnRateCost).ToString();
        rateChangeText.text = System.Math.Round(spawnRate, 2).ToString() + " > " + newRateText.ToString();

        int newQTYText = spawnQuantity + 1;

        quantityCostText.text = System.Math.Round(spawnQTYCost, 2).ToString();
        quantityChangeText.text = spawnQuantity.ToString() + " > " + newQTYText.ToString();

        float newValueText = (float)System.Math.Round(matterValue * matterValueScale, 2);

        valueCostText.text = "Cost: " + matterValueCost.ToString();
        valueChangeText.text = matterValue.ToString() + " > " + newValueText.ToString();
    }

    public void IncreaseMatterValue()
    {
        if (blackholeScript.GetBalance() < matterValueCost) { return; }
        blackholeScript.RemoveMoney(matterValueCost);
        matterValue *= matterValueScale;
        matterValueCost *= matterValueCostScale;
        UpdateText();
        if (matterValue >= matterValueMax)
        {
            matterValue = matterValueMax;
            upgradeValueBTN.gameObject.SetActive(false);
        }

    }
    public void IncreaseSpawnRate()
    {
        if (blackholeScript.GetBalance() < spawnRateCost) { return; }
        blackholeScript.RemoveMoney(spawnRateCost);
        spawnRate *= spawnRateScale;
        spawnRateCost *= spawnCostScale;
        UpdateText();
        if (spawnRate >= spawnRateMax)
        {
            upgradeRateBTN.gameObject.SetActive(false);
        }
    }

    public void IncreaseSpawnQTY()
    {
        if (blackholeScript.GetBalance() < spawnQTYCost) { return; }
        blackholeScript.RemoveMoney(spawnQTYCost);
        spawnQuantity += 1;
        spawnQTYCost *= spawnQTYscale;
        UpdateText();
        if (spawnQuantity >= spawnQTYMax)
        {
            upgradeQTYBTN.gameObject.SetActive(false);
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
