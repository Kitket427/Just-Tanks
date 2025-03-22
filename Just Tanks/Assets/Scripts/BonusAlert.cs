using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
struct AlertType
{
    public int number;
    public bool add;

    public AlertType(int number, bool add) // Конструктор для удобного создания объектов
    {
        this.number = number;
        this.add = add;
    }
}

public class BonusAlert : MonoBehaviour
{
    [SerializeField] private Text alertText;
    private int language = 1;
    [SerializeField] private TextTranslation[] texts;
    [SerializeField] private List<AlertType> alertTypes = new List<AlertType>(); // Инициализация списка

    private void Start()
    {
        language = PlayerPrefs.GetInt("Language");
    }

    public void Alert(int number, bool add, float time = 1)
    {
        CancelInvoke(nameof(Display));
        AlertType newAlert = new AlertType(number, add);
        if (add)
        {
            alertTypes.Add(newAlert);
        }
        else
        {
            alertTypes.Insert(0, newAlert);
        }
        InvokeRepeating(nameof(Display), time, 0.7f);
    }

    void Display()
    {
        alertText.gameObject.SetActive(false);
        alertText.text = texts[alertTypes[alertTypes.Count - 1].number].text[language];
        if (alertTypes[alertTypes.Count - 1].add)
        {
            alertText.color = new Color(0.45f, 1, 0.45f);
            alertText.GetComponent<AudioSource>().pitch = 1.2f;
        }
        else
        {
            alertText.color = new Color(1, 0.45f, 0.45f);
            alertText.GetComponent<AudioSource>().pitch = 0.8f;
        }
        alertText.gameObject.SetActive(true);
        alertTypes.RemoveAt(alertTypes.Count - 1);
        if(alertTypes.Count == 0) CancelInvoke(nameof(Display));
    }
}