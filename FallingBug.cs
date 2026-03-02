using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBug : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el bug cae al suelo y nadie lo atrapa desaparece 
        if(collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
