using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rico : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        Vector2 newDirection = Vector2.Reflect(transform.right, normal);
        transform.parent.right = newDirection;
    }
}
