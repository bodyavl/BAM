using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private float stamina = 5f;

    private bool isAlive = true;

    public void OnBirdCollision()
    {
        if (isAlive)
        {
            if (GetComponent<Collider2D>() != null)
            {
                if (GetComponent<Collider2D>().enabled)
                {
                    if (stamina <= 0)
                    {
                        Die();
                    }
                }
            }
        }
    }

    private void Die()
    {
        isAlive = false;
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > stamina)
        {
            Destroy(gameObject);
        }
    }
}
