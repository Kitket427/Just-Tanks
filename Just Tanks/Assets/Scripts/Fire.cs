using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject spawnObj, effectSpawn;
    [SerializeField] private GameObject[] addSpawnObj;
    public int countObj;
    [SerializeField] private float reloadTime, randomAngle, adaptiveSpdAnim;
    private bool reload;
    public float multiplierPoints;
    [SerializeField] private Animator anim;
    [SerializeField] private bool adaptiveSpeed;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Collider2D shooter;

    private void Start()
    {
        multiplierPoints = 1;
        if(adaptiveSpdAnim == 0) adaptiveSpdAnim = reloadTime;
    }
    public void Shoot()
    {
        if (reload == false && !Physics2D.OverlapCircle(transform.position, 0.3f, layerMask))
        {
            if (countObj == 0)
            {
                var bullet = spawnObj;
                bullet.GetComponent<Bullet>().shooter = shooter;
                Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(-randomAngle, randomAngle)));
            }
            else
            {
                var addSpawn = addSpawnObj[countObj - 1];
                var addedBullet = addSpawn.GetComponentsInChildren<Bullet>();
                foreach (var item in addedBullet)
                {
                    item.GetComponent<Bullet>().shooter = shooter;
                }
                Instantiate(addSpawn, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(-randomAngle, randomAngle)));
            }
            if(effectSpawn)Instantiate(effectSpawn, transform.position, transform.rotation);
            if (anim)
            {
                if (adaptiveSpeed && adaptiveSpdAnim > reloadTime) anim.speed = adaptiveSpdAnim / reloadTime;
                else anim.speed = 1;
                anim.Play("Fire");
            }
            Invoke(nameof(Reload), reloadTime / multiplierPoints);
            reload = true;
        }
    }
    void Reload()
    {
        reload = false;
    }
    public void Bonus(float reloadTime, float randomAngle, int countObj)
    {
        this.reloadTime *= reloadTime;
        this.randomAngle *= randomAngle;
        this.countObj += countObj;
    }
}
