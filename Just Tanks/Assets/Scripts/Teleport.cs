using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Vector2 distance;
    [SerializeField] private int jump;
    private Transform player;
    private void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Transform>();
    }
    private void Update()
    {
        if(player)
        {
            if (player.position.x - transform.position.x > distance.x) transform.position = new Vector2(transform.position.x + jump, transform.position.y);
            if (transform.position.x - player.position.x > distance.x) transform.position = new Vector2(transform.position.x - jump, transform.position.y);
            if (player.position.y - transform.position.y > distance.y) transform.position = new Vector2(transform.position.x, transform.position.y + jump);
            if (transform.position.y - player.position.y > distance.y) transform.position = new Vector2(transform.position.x, transform.position.y - jump);
        }
    }
}
