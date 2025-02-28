using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSpeed : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float speed;
    [SerializeField] private bool floatSpeed;
    [SerializeField] private Transform target;
    private void Start()
    {
        anim = GetComponent<Animator>();
        if (floatSpeed == false) anim.speed = speed;
        else anim.SetFloat("speed", speed);
    }
    private void Update()
    {
        if (target) transform.position = target.position;
    }
}
