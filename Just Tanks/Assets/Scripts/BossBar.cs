using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BossBar : MonoBehaviour
{
    private float hpDamage, hpCurrent, time;
    [SerializeField] private Image[] bars;
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
