using UnityEngine;

public class Matter : MonoBehaviour
{
    public float value = 1f;

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Peg"))
        {
            Pegs pegScript = collider.gameObject.GetComponent<Pegs>();
            AddVal(pegScript.value);
        }
    }

    public void SetValue(float value) { this.value = value; }
    private void AddVal(float amount) { value += amount; }
}
