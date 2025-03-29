using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveInfoSlots : MonoBehaviour
{
    private Data data;
    [SerializeField] private int maxLevels, maxUpgrates;
    [SerializeField] private Image[] bars;
    [SerializeField] private Text[] texts;
    private float time;
    private int upgrates, language;
    [SerializeField] private TextTranslation[] textTranslations;
    private void Awake()
    {
        OnEnable();
    }
    private void OnEnable()
    {
        language = PlayerPrefs.GetInt("Language");
        data = new Data();
        if (!DataSaver.IsSaveExists("SaveA"))
        {
            DataSaver.Save(data, "SaveA");
        }
        if (!DataSaver.IsSaveExists("SaveB"))
        {
            DataSaver.Save(data, "SaveB");
        }
        if (!DataSaver.IsSaveExists("SaveC"))
        {
            DataSaver.Save(data, "SaveC");
        }
        texts[3].text = textTranslations[0].text[language];
        texts[9].text = textTranslations[1].text[language];
        texts[15].text = textTranslations[2].text[language];
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    DataSaver.Open("SaveA", out data);
                    break;
                case 1:
                    DataSaver.Open("SaveB", out data);
                    break;
                case 2:
                    DataSaver.Open("SaveC", out data);
                    break;
                default:
                    break;
            }
            texts[i + 18].text = "" +data.points;
            time = data.time;
            int hours = Mathf.FloorToInt(time / 3600);
            int minutes = Mathf.FloorToInt((time % 3600) / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            texts[i*6].text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            texts[i*6 + 1].text = data.levelProgress + "/" + maxLevels;
            upgrates = 0;
            for (int j = 0; j < data.upgrates.Length; j++)
            {
                upgrates += data.upgrates[j];
            }
            texts[i * 6 + 2].text = upgrates + "/" + maxUpgrates;
            texts[i * 6 + 4].text = textTranslations[3].text[language];
            texts[i * 6 + 5].text = textTranslations[4].text[language];
            bars[i*2].fillAmount = data.levelProgress * 1f / maxLevels;
            bars[i*2 + 1].fillAmount = upgrates * 1f / maxUpgrates;
        }
    }
}
