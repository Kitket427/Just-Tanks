using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private int language = 1;
    [SerializeField] private TextTranslation[] textTranslations;
    [SerializeField] private Text[] texts;
    [SerializeField] private GameObject menu;
    //[SerializeField] private AudioMixerGroup audioMixer;
    private TankControl control;
    [SerializeField] private float time = 1, timeAdd;
    private void Start()
    {
        Time.timeScale = 1;
        control = InputManager.inputManager.control;
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = textTranslations[i].text[language];
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
        if (Time.timeScale != 0 && time < 1 + timeAdd)
        {
            time += Time.deltaTime/ 420f / Time.timeScale;
            Time.timeScale = time;
        }
    }
    public void PauseMenu()
    {
        Time.timeScale = Time.timeScale != 0 ? 0 : time;
        if (Time.timeScale != 0)
        {
            menu.SetActive(false);
            //audioMixer.audioMixer.SetFloat("lowpass", 22000);
            //audioMixer.audioMixer.SetFloat("sfxP", 1);
            control.TankGame.Enable();
        }
        else
        {
            menu.SetActive(true);
            //audioMixer.audioMixer.SetFloat("lowpass", 1000);
            //audioMixer.audioMixer.SetFloat("sfxP", 0);
            control.TankGame.Disable();
        }
    }
    public void Bonus(float time)
    {
        timeAdd = time;
    }
}
