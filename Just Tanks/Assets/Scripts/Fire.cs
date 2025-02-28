using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnObj;
    public float reloadTime;
    private bool reload;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask layerMask;
    public void Shoot()
    {
        if (reload == false && !Physics2D.OverlapCircle(transform.position, 0.3f, layerMask))
        {
            foreach (var item in spawnObj)
            {
                Instantiate(item, transform.position, transform.rotation);
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
