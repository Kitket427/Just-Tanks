using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BossBar : MonoBehaviour
{
    private float hpDamage, hpCurrent, time;
    [SerializeField] private Image[] bars;
    [SerializeField] private Text[] texts;
    [SerializeField] private OstSystem ostSystem;
    private void Start()
    {
        hpDamage = hpCurrent = 1;
    }
    private void Update()
    {
        if(time == 1) hpDamage = Mathf.MoveTowards(hpDamage, hpCurrent, 0.4f * Time.deltaTime);
        bars[1].fillAmount = hpDamage;
        if (hpCurrent > hpDamage) hpDamage = hpCurrent;
        if (hpCurrent <= 0)
        {
            Invoke(nameof(Restart), 7f);
            Invoke(nameof(GameOver), 4f);
            ostSystem.GameOver();
        }
        if(hpDamage <= 0)
        {
            if (bars[2].color.a > 0) bars[2].color = new Color(bars[2].color.r, bars[2].color.g, bars[2].color.b, bars[2].color.a - Time.deltaTime/2f);
            if (bars[3].color.a > 0) bars[3].color = new Color(bars[3].color.r, bars[3].color.g, bars[3].color.b, bars[3].color.a - Time.deltaTime/2f);
            foreach (var item in texts)
            {
                if (item.color.a > 0) item.color = new Color(item.color.r, item.color.g, item.color.b, item.color.a - Time.deltaTime / 2f);
            }
        }
    }
    public void Damage(int hp, int hpmax)
    {
        bars[0].fillAmount = hp * 1f / hpmax * 1f;
        hpCurrent = bars[0].fillAmount;
        time = 0;
        CancelInvoke();
        Invoke(nameof(Timing), 0.7f);
    }
    void Timing()
    {
        time = 1;
    }
    private void OnEnable()
    {
        bars[0].fillAmount = 1;
        bars[1].fillAmount = 1;
    }
    void GameOver()
    {
        bars[2].GetComponent<Animator>().Play("EndGame");
    }
    void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
