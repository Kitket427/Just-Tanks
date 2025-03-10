using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField] private Vector2 distance;
    void Start()
    {
        transform.position = new Vector2(Random.Range(-distance.x, distance.x), Random.Range(-distance.y, distance.y));
    }
}
