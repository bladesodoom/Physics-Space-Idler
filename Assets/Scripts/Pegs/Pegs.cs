using UnityEngine;

public class Pegs : MonoBehaviour
{
    private float posZRotate;
    private float negZRotate;
    private float deltaZRotate;

    public float value = 1f;

    void Start()
    {
        posZRotate = Random.Range(15f, 50f);
        negZRotate = Random.Range(-50f, -15f);

        deltaZRotate = posZRotate + negZRotate;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, deltaZRotate * Time.deltaTime);
    }
}
