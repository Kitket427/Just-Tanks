using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Aim : MonoBehaviour
{
    [SerializeField] private bool player;
    [SerializeField] private float rotateZ, rotate, speed, offset, fireRadius, distance;
    [SerializeField] private Transform target;
    [SerializeField] private Fire fire;
    

    private void Start()
    {
        if (player) target.gameObject.SetActive(true);
        else
        {
            transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            target = FindObjectOfType<Player>().GetComponent<Transform>();
        }
        rotate = rotateZ = transform.eulerAngles.z;
    }
    void Update()
    {
        if (player) target.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (target && Time.timeScale != 0)
        {
            Vector3 difference = target.position - transform.position;
            rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + offset;
            transform.rotation = Quaternion.Euler(0, 0, rotate);
            rotate = Mathf.MoveTowardsAngle(transform.eulerAngles.z, rotateZ, speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.Mouse0) && player ||
                player == false && Vector2.Distance(target.position, transform.position) < distance && Mathf.Abs(rotate - rotateZ) < fireRadius)
            {
                fire.Shoot();
            }
        }
    }
    private void OnDisable()
    {
        if (player) target.gameObject.SetActive(false);
    }
}