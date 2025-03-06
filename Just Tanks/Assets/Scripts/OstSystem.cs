using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OstSystem : MonoBehaviour
{
    [SerializeField] private AudioSource[] ost;
    private AudioSource mainOst;
    private bool[] active;
    private void Start()
    {
        active = new bool[ost.Length];
        InvokeRepeating(nameof(Check), 1f, 1f);
    }
    private void Update()
    {
        for (int i = 0; i < ost.Length; i++)
        {
            if (active[i])
            {
                if (ost[i].volume < 0.2f) ost[i].volume += Time.deltaTime / 3f / Time.timeScale;
                else ost[i].volume = 0.2f;
            }
            else
            {
                if (ost[i].volume > 0) ost[i].volume -= Time.deltaTime / 3f / Time.timeScale;
                else ost[i].volume = 0;
            }
        }
        if(mainOst)
        {
            if (mainOst.pitch > 0) mainOst.pitch -= Time.deltaTime / 6f / Time.timeScale;
            else mainOst.pitch = 0;
            for (int i = 0; i < ost.Length; i++)
            {
                if (ost[i].pitch     > 0) ost[i].pitch -= Time.deltaTime / 6f / Time.timeScale;
                else ost[i].pitch = 0;
            }
        }
    }
    public void Check()
    {
        var enemies = FindObjectsOfType<EnemyUnit>();
        for (int i = 0; i < ost.Length; i++)
        {
            active[i] = false;
        }
        foreach (var item in enemies)
        {
            active[item.type] = true;
        }
    }
    public void GameOver()
    {
        CancelInvoke();
        mainOst = GetComponentInChildren<AudioSource>();
    }
}
