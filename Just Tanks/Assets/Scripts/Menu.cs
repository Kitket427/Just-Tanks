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
    [SerializeField] private Text slotActive;
    [SerializeField] private GameObject[] menus;
    [SerializeField] private TextTranslation[] levelInfo;
    [SerializeField] private Text[] levels;
    [SerializeField] private TextTranslation[] tankInfo;
    [SerializeField] private Text[] tanks;
    private Options options;
    private Data data;
    private void Start()
    {
        options = new Options();
        data = new Data();
        Time.timeScale = 1;
        PlayerPrefs.DeleteKey("Tank");
        DataSaver.Open("Options", out options);
        if (!DataSaver.IsSaveExists(options.activeSave))
        {
            DataSaver.Save(data, options.activeSave);
        }
        else
        {
            DataSaver.Open(options.activeSave, out data);
        }
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
            menus[5].SetActive(true);
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
    public void SelectSave(string saveName)
    {
        DataSaver.Open("Options", out options);
        options.activeSave = saveName;
        DataSaver.Save(options, "Options");
        if (!DataSaver.IsSaveExists(options.activeSave))
        {
            data = new Data();
            DataSaver.Save(data, options.activeSave);
        }
        else
        {
            DataSaver.Open(options.activeSave, out data);
        }
        slotActive.text = options.activeSave switch
        {
            "SaveA" => texts[13].text[language],
            "SaveB" => texts[14].text[language],
            "SaveC" => texts[15].text[language],
            _ => "ÎØÈÁÊÀ! ÎØÈÁÊÀ!"
        };
    }
    public void DeleteSave()
    {
        DataSaver.DeleteFile(options.activeSave);
        data = new Data();
        DataSaver.Save(data, options.activeSave);
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
                slotActive.text = options.activeSave switch
        {
            "SaveA" => texts[13].text[language],
            "SaveB" => texts[14].text[language],
            "SaveC" => texts[15].text[language],
            _ => "ÎØÈÁÊÀ! ÎØÈÁÊÀ!"
        };
    }
}
