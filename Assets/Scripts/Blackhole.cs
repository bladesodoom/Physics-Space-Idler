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
            Matter matterScript = collider.gameObject.GetComponent<Matter>();
            AddMoney(matterScript.value);
            UpdateMoneyText();
            Destroy(matterScript.gameObject);
            Debug.Log("Matter Value: " + matterScript.value);
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
