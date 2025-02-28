using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerate : MonoBehaviour
{
    [SerializeField] private Vector2 distance;
    [SerializeField] private GameObject wall;
    [SerializeField] private Vector2 sizeX, sizeY, count;
    void Start()
    {
        int randomCount = (int)Random.Range(count.x, count.y);
        for(int i = 0; i < randomCount; i++)
        {
            wall.transform.localScale = new Vector3(Random.Range(sizeX.x, sizeX.y), Random.Range(sizeY.x, sizeY.y), 1);
            Instantiate(wall, new Vector2(transform.position.x + Random.Range(-distance.x, distance.x), transform.position.y + Random.Range(-distance.y, distance.y)), Quaternion.Euler(0,0,Random.Range(0,360)), transform);
        }
    }
}
