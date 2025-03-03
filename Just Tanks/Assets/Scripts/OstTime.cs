using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OstTime : MonoBehaviour
{
    private AudioSource ost;
    [SerializeField] private float time;
    void Start()
    {
        ost = GetComponent<AudioSource>();
        ost.Play();
    }
}
