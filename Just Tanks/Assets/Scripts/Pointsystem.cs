using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UpgradeData
{
    public int upgrate;
    public int maxUpgrate;
    public int price;
    public int priceStart;
    public Image barUpgrate;
    public Text textUpgrate;
    public Image barPrice;
    public Text textPrice;
    public TextTranslation translation;
    public Text info;
}

public class Pointsystem : MonoBehaviour
{
    [SerializeField] private Text pointext, pointiplier;
    [SerializeField] private float multiForLevel = 1;
    private int points, language = 1;
    private float multiplier;
    private Data data;
    private Options options;
    [SerializeField] private Fire fireQuad;
    [SerializeField] private UpgradeData[] upgradesData;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private TextTranslation[] texts;
    private bool priceReady;
    [SerializeField] private Menu menu;

    void Start()
    {
        multiplier = 1;
    }

    private void OnEnable()
    {
        language = PlayerPrefs.GetInt("Language");
        LoadPoints();
    }

    public void LoadPoints()
    {
        data = new Data();
        options = new Options();
        DataSaver.Open("Options", out options);
        DataSaver.Open(options.activeSave, out data);
        points = data.points;
        pointext.text = "" + points;

        if (pointiplier == false)
        {
            Verification();
            priceReady = true;
        }
    }

    public void GetPoints(int points)
    {
        UpdatePoint((int)(points * multiplier * multiForLevel));
        if (points < 1000) multiplier += points / 1000f;
        else multiplier++;
        if (multiplier > 10) multiplier = 10;
        pointiplier.text = "x " + Math.Round(multiplier, 2);
        pointext.color = new Color(0.45f, 1, 0.45f);
        pointiplier.color = new Color(0.45f, 1, 0.45f);
        Invoke(nameof(PointIdle), 0.1f);
        if (fireQuad) fireQuad.multiplierPoints = multiplier;
    }

    public void LostPoints(int points)
    {
        UpdatePoint(-points);
        pointext.color = new Color(1, 0.45f, 0.45f);
        multiplier = 1;
        pointiplier.text = "";
        Invoke(nameof(PointIdle), 0.1f);
        if (fireQuad) fireQuad.multiplierPoints = multiplier;
    }

    void UpdatePoint(int points)
    {
        DataSaver.Open(options.activeSave, out data);
        this.points += points;
        if (this.points < 0) this.points = 0;
        data.points = this.points;
        DataSaver.Save(data, options.activeSave);
        pointext.text = "" + this.points;
    }

    void PointIdle()
    {
        pointext.color = Color.white;
        pointiplier.color = Color.white;
    }

    public void Shop(int number)
    {
        if (upgradesData[number].price <= points && upgradesData[number].upgrate < upgradesData[number].maxUpgrate)
        {
            upgradesData[number].upgrate++;
            sfx[0].Play();
            pointext.color = new Color(0.45f, 1, 0.45f);
            Invoke(nameof(ColorRestart), 0.2f);
            data.upgrates[number] = upgradesData[number].upgrate;
            data.points = points -= upgradesData[number].price;
            pointext.text = "" + points;
            if (number == 6) upgradesData[number].price *= 3;
            else upgradesData[number].price += upgradesData[number].priceStart;
            Verification();
            DataSaver.Save(data, options.activeSave);
            if (number == 6) menu.NewUnit();
        }
        else
        {
            sfx[1].Play();
            pointext.color = new Color(1, 0.45f, 0.45f);
            CancelInvoke(nameof(ColorRestart));
            Invoke(nameof(ColorRestart), 0.1f);
        }
    }

    void ColorRestart()
    {
        pointext.color = Color.white;
    }

    void Verification()
    {
        for (int i = 0; i < upgradesData.Length; i++)
        {
            upgradesData[i].upgrate = data.upgrates[i];
            if (priceReady == false)
            {
                upgradesData[i].priceStart = upgradesData[i].price;
                for (int j = 0; j < upgradesData[i].upgrate; j++)
                {
                    if (i == 6)
                    {
                        if (upgradesData[i].price == 0) upgradesData[i].price += upgradesData[i].priceStart;
                        upgradesData[i].price *= 3;
                    }
                    else upgradesData[i].price += upgradesData[i].priceStart;
                }
            }
            upgradesData[i].barUpgrate.fillAmount = upgradesData[i].upgrate * 1f / upgradesData[i].maxUpgrate * 1f;
            upgradesData[i].barPrice.fillAmount = points * 1f / upgradesData[i].price * 1f;
            if (upgradesData[i].barPrice.fillAmount < 1) upgradesData[i].barPrice.color = new Color(0.5f, 0.3f, 0.3f);
            else upgradesData[i].barPrice.color = new Color(0.3f, 0.5f, 0.3f);
            upgradesData[i].textUpgrate.text = upgradesData[i].upgrate + "/" + upgradesData[i].maxUpgrate;
            if (upgradesData[i].upgrate < upgradesData[i].maxUpgrate) upgradesData[i].textPrice.text = "" + upgradesData[i].price;
            else
            {
                upgradesData[i].upgrate = upgradesData[i].maxUpgrate;
                upgradesData[i].textPrice.text = texts[0].text[language];
                upgradesData[i].barPrice.fillAmount = 1;
                upgradesData[i].barPrice.color = new Color(0.4f, 0.4f, 0.4f);
            }
            upgradesData[i].info.text = upgradesData[i].translation.text[language];
        }
    }
}