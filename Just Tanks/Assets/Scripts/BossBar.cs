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
    private void Update()
    {
        if(time == 1) hpDamage = Mathf.MoveTowards(hpDamage, hpCurrent, 0.7f * Time.deltaTime);
        bars[1].fillAmount = hpDamage;
        if (hpCurrent > hpDamage) hpDamage = hpCurrent;
        if (ostSystem && hpCurrent <= 0)
        {
            Invoke(nameof(Restart), 7f * Time.timeScale);
            Invoke(nameof(GameOver), 4f * Time.timeScale);
            ostSystem.GameOver();
        }
        if(hpDamage <= 0)
        {
            for (int i = 3; i < bars.Length; i++)
            {
                if (bars[i].color.a > 0) bars[i].color = new Color(bars[i].color.r, bars[i].color.g, bars[i].color.b, bars[i].color.a - Time.deltaTime / 2f / Time.timeScale);
            }
            foreach (var item in texts)
            {
                if (item.color.a > 0) item.color = new Color(item.color.r, item.color.g, item.color.b, item.color.a - Time.deltaTime / 2f / Time.timeScale);
            }
            if(texts.Length>0)texts[10].GetComponent<Pointsystem>().enabled = false;
        }
    }
    public void Damage(float hp, float hpmax)
    {
        bars[0].fillAmount = hp / hpmax;
        hpCurrent = bars[0].fillAmount;
        time = 0;
        CancelInvoke();
        Invoke(nameof(Timing), 0.7f);
    }
    void Timing()
    {
        time = 1;
    }
    public void BossBattle()
    {
        hpDamage = hpCurrent = 1;
        bars[0].fillAmount = 1;
        bars[1].fillAmount = 1;
        for (int i = 3; i < bars.Length; i++)
        {
            bars[i].color = new Color(bars[i].color.r, bars[i].color.g, bars[i].color.b, 1);
        }
    }
    void GameOver()
    {
        bars[2].GetComponent<Animator>().Play("EndGame");
    }
    void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
