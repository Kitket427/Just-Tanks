using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
struct Trigger
{
    public GameObject obj;
    public bool active;
}
public class HitPoints : MonoBehaviour, ITakeDamage
{
    [SerializeField] private BossBar bossBar;
    [SerializeField] private float hp, hpMax, healingAfter, healingTime;
    [SerializeField] private SpriteRenderer[] sprites;
    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject objDead, effect;
    [SerializeField] private Trigger[] triggers;
    [SerializeField] private Color color;
    private bool isDead;
    [SerializeField] private bool player, bossColor, enemiesCounter;
    void Start()
    {
        if (bossBar && !player) bossBar.BossBattle();
        if (player) bossBar.Damage(hp, hpMax);
        hpMax = hp;
        foreach (var item in sprites)
        {
            if (item.color != Color.black)item.color = color;
        }
    }
    void Update()
    {
        if (bossColor)
        {
            foreach (var item in sprites)
            {
                if (item.color != Color.black) item.color = color;
            }
        }
    }
    void ITakeDamage.TakeDamage(float damage)
    {
        if(player) FindObjectOfType<CameraShake>().TriggerShake(0.1f, damage/7f, damage / 7f);
        hp -= damage;
        if (hp <= 0 && isDead == false)
        {
            Dead();
            isDead = true;
        }
        if (damage > 0)
        {
            CancelInvoke(nameof(ReturnMat));
            foreach (var item in sprites)
            {
                item.material = materials[0];
            }
            Invoke(nameof(ReturnMat), 0.03f);
        }
        if (bossBar) bossBar.Damage(hp, hpMax);
        if (healingAfter > 0)
        {
            CancelInvoke(nameof(Healing));
            Invoke(nameof(Healing), healingAfter);

        }
    }
    void Healing()
    {
        hp++;
        if(hp < hpMax) Invoke(nameof(Healing), 1f/(hpMax / healingTime));
        if (bossBar) bossBar.Damage(hp, hpMax);
    }
    void ReturnMat()
    {
        foreach (var item in sprites)
        {
            item.material = materials[1];
        }
    }
    void Dead()
    {
        if (bossBar) bossBar.Damage(0, hpMax);
        Instantiate(effect, transform.position, transform.rotation);
        foreach (var item in triggers)
        {
            item.obj.SetActive(item.active);
        }
        FindObjectOfType<CameraShake>().TriggerShake(0.12f, 7f, 7f);
        if (enemiesCounter) FindObjectOfType<Spawner>().EnemyDown();
        Destroy(objDead);
    }
    public void Bonus(float hp, float healingAfter, float healingTime)
    {
        this.hp *= hp;
        hpMax *= hp;
        this.healingAfter *= healingAfter;
        this.healingTime *= healingTime;
    }
}
