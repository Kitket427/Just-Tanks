using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
struct BonusesChoise
{
    public bool active;
    public TextTranslation up;
    public TextTranslation but;
    public TextTranslation down;
    public float speedAim;
    public float reloadTime;
    public float randomAngle;
    public int countObj;
    public float hp;
    public float healingAfter;
    public float healingTime;
    public float speedMove;
}
public class Bonuses : MonoBehaviour
{
    private int language = 1;
    [SerializeField] private BonusesChoise[] bonuses;
    [SerializeField] private Text[] leftChoise, rightChoise;
    private int choiseA, choiseB, choiseC, summ;
    private bool selectedRight, activeSelect;
    [SerializeField] private Player player;
    [SerializeField] private Aim aim;
    [SerializeField] private HitPoints hitPoints;
    [SerializeField] private Fire fire;
    [SerializeField] private float x;
    private void Start()
    {
        Invoke(nameof(PlayerFind), 1f);
    }
    private void PlayerFind()
    {
        player = FindObjectOfType<Player>();
        hitPoints = player.GetComponent<HitPoints>();
        aim = player.GetComponentInChildren<Aim>();
        fire = player.GetComponentInChildren<Fire>();
    }
    public void StartChoise()
    {
        summ = 0;
        for (int i = 0; i < bonuses.Length; i++)
        {
            if (bonuses[i].active) summ++;
        }
        if (summ > 0)
        {
            choiseA = Random.Range(0, bonuses.Length);
            leftChoise[0].gameObject.SetActive(true);
            while (bonuses[choiseA].active == false)
            {
                if (choiseA < bonuses.Length - 1) choiseA++;
                else choiseA = 0;
            }
            bonuses[choiseA].active = false;
            leftChoise[0].text = bonuses[choiseA].but.text[language];
            leftChoise[1].text = bonuses[choiseA].up.text[language];
            leftChoise[2].text = bonuses[choiseA].down.text[language];
            if (summ > 1)
            {
                choiseB = Random.Range(0, bonuses.Length);
                rightChoise[0].gameObject.SetActive(true);
                while (bonuses[choiseB].active == false)
                {
                    if (choiseB < bonuses.Length - 1) choiseB++;
                    else choiseB = 0;
                }
                bonuses[choiseA].active = true;
                rightChoise[0].text = bonuses[choiseB].but.text[language];
                rightChoise[1].text = bonuses[choiseB].up.text[language];
                rightChoise[2].text = bonuses[choiseB].down.text[language];
            }
            activeSelect = true;
        }
    }
    private void Update()
    {
        if (activeSelect)
        {
            x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            if (x >= 0 && summ > 1)
            {
                selectedRight = true;
                rightChoise[0].rectTransform.localScale = new Vector3(0.4f, 0.4f);
                leftChoise[0].rectTransform.localScale = new Vector3(0.2f, 0.2f);
            }
            else
            {
                selectedRight = false;
                leftChoise[0].rectTransform.localScale = new Vector3(0.4f, 0.4f);
                rightChoise[0].rectTransform.localScale = new Vector3(0.2f, 0.2f);
            }
        }
    }
    public void EndChoise()
    {
        if (summ > 0)
        {
            activeSelect = false;
            if (selectedRight)
            {
                choiseC = choiseB;
                leftChoise[0].gameObject.SetActive(false);
            }
            else
            {
                choiseC = choiseA;
                rightChoise[0].gameObject.SetActive(false);
            }
            player.Bonus(bonuses[choiseC].speedMove);
            aim.Bonus(bonuses[choiseC].speedAim);
            fire.Bonus(bonuses[choiseC].reloadTime, bonuses[choiseC].randomAngle, bonuses[choiseC].countObj);
            hitPoints.Bonus(bonuses[choiseC].hp, bonuses[choiseC].healingAfter, bonuses[choiseC].healingTime);
            bonuses[choiseC].active = false;
            Invoke(nameof(End), 2f);
        }
    }
    void End()
    {
        leftChoise[0].gameObject.SetActive(false);
        rightChoise[0].gameObject.SetActive(false);
    }
}
