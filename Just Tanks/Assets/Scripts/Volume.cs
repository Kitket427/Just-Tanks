using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Volume : MonoBehaviour
{
    [SerializeField] private Slider[] sliders;
    [SerializeField] private AudioMixerGroup audioMixer;
    private Options options;
    private void Awake()
    {
        options = new Options();
        if (!DataSaver.IsSaveExists("Options"))
        {
            options.sfxVolume = -15;
            options.ostVolume = -15;
            DataSaver.Save(options, "Options");
        }
        else
        {
            DataSaver.Open("Options", out options);
        }
    }
    private void Start()
    {
        //if (!PlayerPrefs.HasKey("sfxVolume"))
        //{
        //    PlayerPrefs.SetFloat("sfxVolume", -15);
        //    PlayerPrefs.SetFloat("ostVolume", -15);
        //}
        //audioMixer.audioMixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume"));
        //audioMixer.audioMixer.SetFloat("ostVolume", PlayerPrefs.GetFloat("ostVolume"));
        //sliders[0].value = (30f + PlayerPrefs.GetFloat("sfxVolume")) / 30f;
        //sliders[1].value = (30f + PlayerPrefs.GetFloat("ostVolume")) / 30f;
        sliders[0].onValueChanged.AddListener(ChangeVolumeSFX);
        sliders[1].onValueChanged.AddListener(ChangeVolumeOST);
        sliders[0].value = (30f + options.sfxVolume) / 30f;
        sliders[1].value = (30f + options.ostVolume) / 30f;
        if (sliders[0].value > 0.01f) audioMixer.audioMixer.SetFloat("sfxVolume", options.sfxVolume);
        else audioMixer.audioMixer.SetFloat("sfxVolume", -80);
        if (sliders[1].value > 0.01f) audioMixer.audioMixer.SetFloat("ostVolume", options.ostVolume);
        else audioMixer.audioMixer.SetFloat("ostVolume", -80);
    }
    private void Update()
    {
        if (Time.timeScale != 0)
        {
            audioMixer.audioMixer.SetFloat("lowpass", 22000);
            audioMixer.audioMixer.SetFloat("ostSpeed", 1);
            audioMixer.audioMixer.SetFloat("sfxSpeed", 1);
        }
        else
        {
            audioMixer.audioMixer.SetFloat("lowpass", 700);
            audioMixer.audioMixer.SetFloat("ostSpeed", 0.7f);
            audioMixer.audioMixer.SetFloat("sfxSpeed", 0);
        }
    }
    void ChangeVolumeSFX(float volume)
    {
        options.sfxVolume = -30f + 30f * volume;
        if (sliders[0].value > 0.01f) audioMixer.audioMixer.SetFloat("sfxVolume", options.sfxVolume);
        else audioMixer.audioMixer.SetFloat("sfxVolume", -80);
        DataSaver.Save(options, "Options");
    }
    void ChangeVolumeOST(float volume)
    {
        options.ostVolume = -30f + 30f * volume;
        if (sliders[1].value > 0.01f) audioMixer.audioMixer.SetFloat("ostVolume", options.ostVolume);
        else audioMixer.audioMixer.SetFloat("ostVolume", -80);
        DataSaver.Save(options, "Options");
    }
    private void OnDestroy()
    {
        sliders[0].onValueChanged.RemoveListener(ChangeVolumeSFX);
        sliders[1].onValueChanged.RemoveListener(ChangeVolumeOST);
    }
}
