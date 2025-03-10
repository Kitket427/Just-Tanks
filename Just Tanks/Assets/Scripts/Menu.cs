using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    private int language = 1;
    [SerializeField] private TextTranslation[] texts;
    [SerializeField] private Text[] buttons;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject[] menus;
    private void Start()
    {
        Language();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !IsInvoking())
        {
            foreach (var item in menus)
            {
                item.SetActive(false);
            }
            menus[0].SetActive(true);
        }
    }
    void BlackScreen()
    {
        anim.Play("EndGame");
    }
    public void Quit()
    {
        BlackScreen();
        Invoke(nameof(Bye), 2);
    }
    public void DeleteSave()
    {
        DataSaver.DeleteGameSaves();
        Quit();
    }
    void Bye()
    {
        Application.Quit();
    }
    public void Language()
    {
        language = PlayerPrefs.GetInt("Language");
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].text = texts[i].text[language];
        }
    }
}
