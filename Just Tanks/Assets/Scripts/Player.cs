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
    private const float angleThreshold = 7f; // Допустимый угол поворота

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        control = InputManager.inputManager.control;
        control.TankGame.Enable();
        control.TankGame.Moving.performed += move => direction = move.ReadValue<Vector2>();
        control.TankGame.Moving.canceled += move => direction = Vector2.zero; // Если отпущено, направление ноль
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }

    void FixedUpdate()
    {
        // Применяем силу для движения
        rb.AddForce(direction * speed * 111, ForceMode2D.Force);
        if (rb.velocity.magnitude > 0.1f)
        {
            if (time >= 0.3f / speed)
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

            // Рассчитываем разницу углов
            float angleDifference = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle - 90);

            // Проверяем, превышает ли угол допустимый порог
            if (Mathf.Abs(angleDifference) > rotationSpeed)
            {
                // Поворачиваем на 180 градусов, если угол больше 90
                if (Mathf.Abs(angleDifference) > 90)
                {
                    targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle + 90));
                }

                // Плавный поворот
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                // Доворачиваем танк, когда близко к целевому углу
                float smoothAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle - 90, rotationSpeed);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, smoothAngle));
            }
        }
    }

    public void Bonus(float speed)
    {
        this.speed *= speed;
        if (this.speed > 10) this.speed = 10;
        rotationSpeed *= speed;
    }
}