using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    private int language = 1, level = 0;
    [SerializeField] private TextTranslation[] texts;
    [SerializeField] private Text[] buttons;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject[] menus;
    [SerializeField] private TextTranslation[] levelInfo;
    [SerializeField] private Text[] levels;
    [SerializeField] private TextTranslation[] tankInfo;
    [SerializeField] private Text[] tanks;
    private void Start()
    {
        Time.timeScale = 1;
        Language();
        PlayerPrefs.DeleteKey("Tank");
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
    public void Selectank(int level)
    {
        this.level = level;
    }
    public void StartLevel(int tank)
    {
        PlayerPrefs.SetInt("Tank", tank);
        BlackScreen();
        Invoke(nameof(LevelStart), 2f);
    }
    public void LevelStart()
    {
        SceneManager.LoadScene(level);
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
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].text = levelInfo[i].text[language];
        }
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].text = tankInfo[i].text[language];
        }
    }
}
