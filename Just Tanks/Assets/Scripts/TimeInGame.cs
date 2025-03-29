using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeInGame : MonoBehaviour
{
    private float timeOnTimer;
    [SerializeField] private Text text;
    private Options options;
    private Data data;
    void Start()
    {
        timeOnTimer = 0;
        options = new Options();
        data = new Data();
        DataSaver.Open("Options", out options);
    }

    private void Update()
    {
        if (Time.timeScale != 0)
            timeOnTimer += Time.deltaTime / Time.timeScale;

        int hours = Mathf.FloorToInt(timeOnTimer / 3600);
        int minutes = Mathf.FloorToInt((timeOnTimer % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeOnTimer % 60);

        text.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
    private void OnApplicationQuit()
    {
        SaveTime();
    }
    private void OnDisable()
    {
        SaveTime();
    }
    private void SaveTime()
    {
        DataSaver.Open(options.activeSave, out data);
        data.time += timeOnTimer;
        DataSaver.Save(data, options.activeSave);
    }
}