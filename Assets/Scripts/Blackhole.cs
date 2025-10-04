using TMPro;

using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public TextMeshPro moneyText;

    private float moneyVal = 0;

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Matter"))
        {
            MatterOld matterScript = collider.gameObject.GetComponent<MatterOld>();
            AddMoney(matterScript.value);
            UpdateMoneyText();
            Destroy(matterScript.gameObject);
        }
    }

    public float GetBalance() { return moneyVal; }

    private void AddMoney(float amount)
    {
        moneyVal += amount;
    }

    public void RemoveMoney(float amount)
    {
        moneyVal -= amount;
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "Money: " + System.Math.Round(moneyVal, 2);
    }
}
