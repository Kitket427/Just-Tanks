using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public bool richoshet, fire, dam;
    public float speed, time, timeEffect;
    [SerializeField] private GameObject effect, ex;
    [SerializeField] private GameObject[] bullet;
    [SerializeField] private Transform posEffect;
    [SerializeField] private int ricount;
    public int breakouts;
    private void Start()
    {
        if (time > 0) Invoke(nameof(Dead), time);
        if (!richoshet && GetComponentInChildren<Rico>()) GetComponentInChildren<Rico>().enabled = false;
        Invoke(nameof(Active), 0.1f);
        if (fire) Invoke(nameof(Fire), 0.2f);
    }
    void Active()
    {
        dam = true;
    }
    void Fire()
    {
        foreach (var item in bullet)
        {
            Instantiate(item, transform.position, Quaternion.Euler(0,0,Random.Range(0, 360)));
        }
        Invoke(nameof(Fire), 0.2f);
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed);
        if (effect && speed != 0 && timeEffect >= 0.03f / speed)
        {
            Instantiate(effect, posEffect.position, transform.rotation);
            timeEffect = 0;
        }
        timeEffect += Time.fixedDeltaTime;
    }
    void Dead()
    {
        Instantiate(ex, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ITakeDamage takeDamage = collision.GetComponent<ITakeDamage>();
        if (takeDamage != null && dam)
        {
            takeDamage.TakeDamage(damage);
            if (speed != 0)
            {
                if (breakouts <= 0) Dead();
                else
                {
                    breakouts--;
                    CancelInvoke();
                    Invoke(nameof(Dead), time);
                }
            }
        }
        if (speed != 0)
        {
            if ((!richoshet || ricount <= 0)) Dead();
            else
            {
                dam = true;
                ricount--;
                CancelInvoke();
                Invoke(nameof(Dead), time);
            }
        }
    }
}
