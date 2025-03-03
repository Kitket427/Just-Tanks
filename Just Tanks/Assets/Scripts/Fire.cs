using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnObj;
    [SerializeField] private float reloadTime, randomAngle;
    private bool reload;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask layerMask;
    public void Shoot()
    {
        if (reload == false && !Physics2D.OverlapCircle(transform.position, 0.3f, layerMask))
        {
            foreach (var item in spawnObj)
            {
                Instantiate(item, transform.position, Quaternion.Euler(0,0,transform.eulerAngles.z + Random.Range(-randomAngle, randomAngle)));
            }
            if (anim) anim.Play("Fire");
            Invoke(nameof(Reload), reloadTime);
            reload = true;
        }
    }
    void Reload()
    {
        reload = false;
    }
}
