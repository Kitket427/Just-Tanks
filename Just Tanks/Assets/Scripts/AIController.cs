using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Vector2 targetPosition;
    private Vector2 direction;
    [SerializeField] private float speed;
    [SerializeField] private float changeDirectionTime, timeA, timeB, distance; // время до смены направления
    [SerializeField] private float rotationSpeed = 5f; // скорость поворота
    private Rigidbody2D rb;
    [SerializeField] private GameObject trail;
    private float time;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetNewTargetPosition();

    }

    void FixedUpdate()
    {
        MoveTowardsTarget();
        if (trail && (rb.velocity.x > 0.1f || rb.velocity.y > 0.1f || rb.velocity.x < -0.1f || rb.velocity.y < -0.1f))
        {
            if (time >= 0.3f / speed)
            {
                Instantiate(trail, transform.position, transform.rotation);
                time = 0;
            }
            time += Time.fixedDeltaTime;
        }
    }

    private void MoveTowardsTarget()
    {
        direction = (targetPosition - rb.position).normalized;
        if (Vector2.Distance(targetPosition, transform.position) < 1)
        {
            CancelInvoke(nameof(ChangeDirection));
            ChangeDirection();
        }

        // Применяем силу для движения
        rb.AddForce(direction * speed * 111, ForceMode2D.Force);

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

    private void SetNewTargetPosition()
    {
        // Генерируем случайную точку в пределах игрового мира
        targetPosition = new Vector2(transform.position.x + Random.Range(-distance, distance), transform.position.y + Random.Range(-distance, distance)); // Замените диапазон на нужный
        changeDirectionTime = Random.Range(timeA, timeB);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CancelInvoke(nameof(ChangeDirection));
        ChangeDirection();
    }
    private void ChangeDirection()
    {
        SetNewTargetPosition();
        Invoke(nameof(ChangeDirection), changeDirectionTime);
    }
}