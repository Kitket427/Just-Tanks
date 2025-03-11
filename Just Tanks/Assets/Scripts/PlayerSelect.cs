using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    [SerializeField] private GameObject[] tanks;
    void Awake()
    {
        tanks[PlayerPrefs.GetInt("Tank")].SetActive(true);
        PlayerPrefs.DeleteKey("Tank");
    }
}
