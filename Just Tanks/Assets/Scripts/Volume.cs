using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Volume : MonoBehaviour
{
    [SerializeField] private Slider[] sliders;
    [SerializeField] private AudioMixerGroup audioMixer;
    private Data data;
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
        data = new Data();
        if(!DataSaver.IsSaveExists("Save"))
        {
            data.sfxVolume = -15;
            data.ostVolume = -15;
            DataSaver.Save(data, "Save");
        }
        else
        {
            DataSaver.Open("Save", out data);
        }
        sliders[0].value = (30f + data.sfxVolume) / 30f;
        sliders[1].value = (30f + data.ostVolume) / 30f;
        if (sliders[0].value > 0.01f) audioMixer.audioMixer.SetFloat("sfxVolume", data.sfxVolume);
        else audioMixer.audioMixer.SetFloat("sfxVolume", -80);
        if (sliders[1].value > 0.01f) audioMixer.audioMixer.SetFloat("ostVolume", data.ostVolume);
        else audioMixer.audioMixer.SetFloat("ostVolume", -80);
    }
    //private void Update()
    //{
    //    if (sliders[0].value > 0.01f) audioMixer.audioMixer.SetFloat("sfxVolume", data.sfxVolume);
    //    else audioMixer.audioMixer.SetFloat("sfxVolume", -80);
    //    if (sliders[1].value > 0.01f) audioMixer.audioMixer.SetFloat("ostVolume", PlayerPrefs.GetFloat("ostVolume"));
    //    else audioMixer.audioMixer.SetFloat("ostVolume", -80);
    //    //PlayerPrefs.SetFloat("sfxVolume", -30f + 30f * sliders[0].value);
    //    //PlayerPrefs.SetFloat("ostVolume", -30f + 30f * sliders[1].value);
    //}
    void ChangeVolumeSFX(float volume)
    {
        data.sfxVolume = -30f + 30f * volume;
        if (sliders[0].value > 0.01f) audioMixer.audioMixer.SetFloat("sfxVolume", data.sfxVolume);
        else audioMixer.audioMixer.SetFloat("sfxVolume", -80);
        DataSaver.Save(data, "Save");
    }
    void ChangeVolumeOST(float volume)
    {
        data.ostVolume = -30f + 30f * volume;
        if (sliders[1].value > 0.01f) audioMixer.audioMixer.SetFloat("ostVolume", data.ostVolume);
        else audioMixer.audioMixer.SetFloat("ostVolume", -80);
        DataSaver.Save(data, "Save");
    }
    private void OnDestroy()
    {
        sliders[0].onValueChanged.RemoveListener(ChangeVolumeSFX);
        sliders[1].onValueChanged.RemoveListener(ChangeVolumeOST);
    }
}
