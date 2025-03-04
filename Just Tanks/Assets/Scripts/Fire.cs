using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject spawnObj, effectSpawn;
    [SerializeField] private GameObject[] addSpawnObj;
    public int countObj;
    [SerializeField] private float reloadTime, randomAngle;
    private bool reload;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask layerMask;
    public void Shoot()
    {
        if (reload == false && !Physics2D.OverlapCircle(transform.position, 0.3f, layerMask))
        {
            if(countObj == 0) Instantiate(spawnObj, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(-randomAngle, randomAngle)));
            else Instantiate(addSpawnObj[countObj-1], transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(-randomAngle, randomAngle)));
            Instantiate(effectSpawn, transform.position, transform.rotation);
            if (anim) anim.Play("Fire");
            Invoke(nameof(Reload), reloadTime);
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
