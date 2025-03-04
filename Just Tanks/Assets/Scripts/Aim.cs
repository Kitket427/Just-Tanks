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
    [SerializeField] private LayerMask layerMask;
    private void Start()
    {
        if (type == TypeOfUnit.player) target.gameObject.SetActive(true);
        else
        {
            transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            if (type == TypeOfUnit.enemy)
            {
                if(FindObjectOfType<Player>()) InvokeRepeating(nameof(FinderPl), 1, 1);
            }
            else InvokeRepeating(nameof(FinderEn), 1, 1);
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
    void FinderPl()
    {
        if (FindObjectOfType<Player>())
        {
            var pos = FindObjectOfType<Player>().GetComponent<Transform>();
            Vector3 difference = pos.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, difference, distance, layerMask);
            if(hit && hit.collider.TryGetComponent(out Player player))
            {
                target = pos;
            }
            else
            {
                target = null;
            }
        }
    }
    void FinderEn()
    {
        if(FindObjectOfType<EnemyUnit>())
        {
            var enemies = FindObjectsOfType<EnemyUnit>();
            var enemyTarget = FindObjectOfType<EnemyUnit>();
            enemyTarget = null;
            float dist = distance;
            float minDist;
            foreach (var enemy in enemies)
            {
                Vector3 difference = enemy.transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, difference, distance, layerMask);
                if (hit && hit.collider.TryGetComponent(out EnemyUnit enemyUnit))
                {
                    minDist = Mathf.Min(dist, Vector2.Distance(enemy.transform.position, transform.position));
                    if (minDist < dist)
                    {
                        enemyTarget = enemy;
                        dist = minDist;
                    }
                }
            }
            if(enemyTarget) target = enemyTarget.GetComponent<Transform>();
        }
    }
    private void OnDisable()
    {
        if (type == TypeOfUnit.player) target.gameObject.SetActive(false);
    }
    public void Bonus(float speed)
    {
        this.speed *= speed;
    }
}