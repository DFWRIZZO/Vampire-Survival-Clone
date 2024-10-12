using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private float lifeTime = 1f;  // How long the indicator stays visible
    private float floatSpeed = 2f;  // Speed at which the indicator floats upwards

    void Start()
    {
        Destroy(gameObject, lifeTime);  // Destroy the indicator after the specified lifetime
    }

    void Update()
    {
        // Make the indicator float upwards over time
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Optional: Add fade-out effect here if using a TextMeshPro component
    }
}
