using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] tanks;
    [SerializeField] private float timeA, timeB, distance;
    private void Start()
    {
        Invoke(nameof(Spawn), Random.Range(timeA, timeB));
    }
    void Spawn()
    {
        int a = Random.Range(0, 2);
        int b = Random.Range(0, 2);
        Instantiate(tanks[Random.Range(0, tanks.Length)], new Vector2(transform.position.x + distance * (-1 + a*2), transform.position.y + distance * (-1 + b * 2)), transform.rotation);
        Invoke(nameof(Spawn), Random.Range(timeA, timeB));
        timeA *= 0.97f;
        timeB *= 0.97f;
    }
}
