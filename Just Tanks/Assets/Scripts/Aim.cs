using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
enum TypeOfUnit
{
    player, enemy, friend
}
public class Aim : MonoBehaviour
{
    [SerializeField] private TypeOfUnit type;
    [SerializeField] private float rotateZ, rotate, speed, offset, fireRadius, distance;
    [SerializeField] private Transform target;
    [SerializeField] private Fire fire;
    [SerializeField] private bool gamepad;

    private void Start()
    {
        if (type == TypeOfUnit.player) target.gameObject.SetActive(true);
        else
        {
            transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            if (type == TypeOfUnit.enemy) target = FindObjectOfType<Player>().GetComponent<Transform>();
            else Finder();
        }
        rotate = rotateZ = transform.eulerAngles.z;
        
    }
    void Update()
    {
        if (type == TypeOfUnit.player)
        {
             target.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Time.timeScale != 0)
        {
            if (target)
            {
                Vector3 difference = target.position - transform.position;
                rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + offset;
                transform.rotation = Quaternion.Euler(0, 0, rotate);
                rotate = Mathf.MoveTowardsAngle(transform.eulerAngles.z, rotateZ, speed * Time.deltaTime);
                if (Input.GetKey(KeyCode.Mouse0) && type == TypeOfUnit.player ||
                    type != TypeOfUnit.player && Vector2.Distance(target.position, transform.position) < distance && Mathf.Abs(rotate - rotateZ) < fireRadius)
                {
                    fire.Shoot();
                }
            }
            else
            {
                if (Mathf.Abs(rotate - rotateZ) < fireRadius) rotateZ = Random.Range(0, 360);
                transform.rotation = Quaternion.Euler(0, 0, rotate);
                rotate = Mathf.MoveTowardsAngle(transform.eulerAngles.z, rotateZ, speed * Time.deltaTime);
            }
        }
    }
    void Finder()
    {
        if(FindObjectOfType<EnemyUnit>())
        {
            var enemies = FindObjectsOfType<EnemyUnit>();
            var enemyTarget = FindObjectOfType<EnemyUnit>();
            float dist = 999;
            float minDist;
            foreach (var enemy in enemies)
            {
                minDist = Mathf.Min(dist, Vector2.Distance(enemy.transform.position, transform.position));
                if (minDist < dist)
                {
                    enemyTarget = enemy;
                    dist = minDist;
                }
            }
            target = enemyTarget.GetComponent<Transform>();
        }
        Invoke(nameof(Finder), 1f);
    }
    private void OnDisable()
    {
        if (type == TypeOfUnit.player) target.gameObject.SetActive(false);
    }
}