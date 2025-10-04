using UnityEngine;

public class MatterOld : MonoBehaviour
{
    public float value = 1f;

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Peg"))
        {
            PegOld pegScript = collider.gameObject.GetComponent<PegOld>();
            AddVal(pegScript.value);
        }
    }

    public void SetValue(float value) { this.value = value; }
    private void AddVal(float amount) { value += amount; }
}
