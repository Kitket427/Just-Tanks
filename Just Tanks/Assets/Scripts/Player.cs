using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private TankControl control;
    private Vector2 direction;
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed; // скорость поворота
    [SerializeField] private GameObject trail;
    private float time;
    [Header("Bonuses")]
    public int speedMultiplier;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        control = InputManager.inputManager.control;
        control.TankGame.Enable();
        control.TankGame.Moving.performed += move => direction = move.ReadValue<Vector2>();
        control.TankGame.Moving.canceled += move => direction = Vector2.zero; // Если отпущено, направление ноль
    }

    void FixedUpdate()
    {
        // Применяем силу для движения
        rb.AddForce(direction * (speed + speed * speedMultiplier / 10f) * 111, ForceMode2D.Force);
        if(rb.velocity.x > 0.1f || rb.velocity.y > 0.1f || rb.velocity.x < -0.1f || rb.velocity.y < -0.1f)
        {
            if (time >= 0.3f/(speed + speed * speedMultiplier/10f))
            {
                Instantiate(trail, transform.position, transform.rotation);
                time = 0;
            }
            time += Time.fixedDeltaTime;
        }
        // Если направление не нулевое, поворачиваем танк
        if (direction != Vector2.zero)
        {
            // Рассчитываем целевое вращение
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle - 90)); // -90 для корректировки угла

            // Плавный поворот с учетом направления
            float angleDifference = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle - 90);
            if (Mathf.Abs(angleDifference) > 90)
            {
                targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle + 90)); // Поворачиваем на 180 градусов
            }

            // Плавный поворот
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
    public void Bonus(float speed)
    {
        this.speed *= speed;
        rotationSpeed *= speed;
    }
}