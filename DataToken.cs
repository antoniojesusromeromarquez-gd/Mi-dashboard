using UnityEngine;

public class DataToken : MonoBehaviour
{
    public enum DataType { Red, Blue, Green }
    public DataType type;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el dato toca el suelo, se destruye
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("¡Dato perdido! Se ha corrompido al tocar el suelo.");
            Destroy(gameObject);
        }
    }
}
