using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnAround : MonoBehaviour
{
    private bool facingRight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (facingRight)
            {
                transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
                facingRight = false;
            }
            else if (!facingRight)
            {
                transform.parent.localRotation = Quaternion.Euler(0, 180, 0);
                facingRight = true;
            }
        }
    }
}
