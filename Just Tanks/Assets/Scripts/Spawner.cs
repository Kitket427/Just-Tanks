using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct TextTranslation
{
    public string[] text;
}

[System.Serializable]
struct SpawnEnemies
{
    public GameObject enemy;
    public int count;
}

[System.Serializable]
struct WaveEnemies
{
    public SpawnEnemies[] spawnEnemies;
    public GameObject[] friends;
}
public class Spawner : MonoBehaviour
{
    private int language = 1;
    [SerializeField] private TextTranslation[] textTranslations;
    [SerializeField] private Text[] texts;
    [SerializeField] private int waveNumber, enemiesCounter, timeToWave;
    [SerializeField] private WaveEnemies[] waveEnemies;
    [SerializeField] private float timeA, timeB, timeMultiplier, distance, timeOnTimer;
    [SerializeField] private Bonuses bonuses;
    private void Start()
    {
        texts[0].text = "";
        texts[1].text = "";
        texts[2].text = "";
        texts[3].text = "00:00";
        Invoke(nameof(WaveStart), 7);
        timeOnTimer = 0;
    }
    private void Update()
    {
        timeOnTimer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeOnTimer / 60);
        int seconds = Mathf.FloorToInt(timeOnTimer % 60);
        texts[3].text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void Spawn()
    {
        int a = Random.Range(0, 2);
        int b = Random.Range(0, 2);
        int randomTank = Random.Range(0, waveEnemies[waveNumber - 1].spawnEnemies.Length);
        bool active = true;
        while (active)
        {
            if (waveEnemies[waveNumber - 1].spawnEnemies[randomTank].count > 0)
            {
                Instantiate(waveEnemies[waveNumber - 1].spawnEnemies[randomTank].enemy, new Vector2(transform.position.x + distance * (-1 + a * 2), transform.position.y + distance * (-1 + b * 2)), transform.rotation);
                waveEnemies[waveNumber - 1].spawnEnemies[randomTank].count--;
                active = false;
            }
            else
            {
                if (randomTank < waveEnemies[waveNumber - 1].spawnEnemies.Length - 1) randomTank++;
                else randomTank = 0;
            }
        }
        int countSumm = 0;
        foreach (var count in waveEnemies[waveNumber - 1].spawnEnemies)
        {
            countSumm += count.count;
        }
        if (countSumm > 0)
        {
            Invoke(nameof(Spawn), Random.Range(timeA, timeB));
            timeA *= 1f - timeMultiplier;
            timeB *= 1f - timeMultiplier;
        }
        if (waveEnemies[waveNumber - 1].friends.Length > 0 && waveEnemies[waveNumber - 1].friends[0])
        {
            foreach (var friend in waveEnemies[waveNumber - 1].friends)
            {
                Instantiate(friend, new Vector2(transform.position.x + distance * (-1 + a * 2), transform.position.y + distance * (-1 + b * 2)), transform.rotation);
            }
            waveEnemies[waveNumber - 1].friends[0] = null;
        }
    }
    public void EnemyDown()
    {
        enemiesCounter--;
        texts[1].text = textTranslations[1].text[language] + " " + enemiesCounter;
        if (enemiesCounter == 0)
        {
            texts[1].text = textTranslations[2].text[language];
            timeToWave = 9;
            if (waveNumber < waveEnemies.Length) TimerToWave();
            else Victory();
        }
    }
    void TimerToWave()
    {
        if (timeToWave == 8) bonuses.StartChoise();
        if (timeToWave == 2) bonuses.EndChoise();
        texts[2].text = textTranslations[3].text[language] + " " + timeToWave;
        if (timeToWave >= 0)
        {
            Invoke(nameof(TimerToWave), 1);
            timeToWave--;
        }
        else WaveStart();
    }
    void WaveStart()
    {
        texts[2].text = "";
        waveNumber++;
        
        texts[0].text = textTranslations[0].text[language] + " " + waveNumber;
        foreach (var count in waveEnemies[waveNumber - 1].spawnEnemies)
        {
            enemiesCounter += count.count;
        }
        texts[1].text = textTranslations[1].text[language] + " " + enemiesCounter;
        Spawn();
    }
    void Victory()
    {
        texts[0].text = textTranslations[4].text[language];
        texts[1].text = textTranslations[5].text[language];
    }
}
