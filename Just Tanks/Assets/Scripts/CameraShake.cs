using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Ссылка на виртуальную камеру
    private CinemachineBasicMultiChannelPerlin perlin; // Ссылка на компонент Perlin

    [SerializeField] private float shakeDuration = 0.5f; // Длительность тряски
    [SerializeField] private float shakeAmplitude = 3f; // Амплитуда тряски
    [SerializeField] private float shakeFrequency = 10f; // Частота тряски

    private void Start()
    {
        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void TriggerShake(float dir, float ampl, float freq)
    {
        shakeDuration = dir;
        shakeAmplitude = ampl;
        shakeFrequency = freq;
        StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        perlin.m_AmplitudeGain = shakeAmplitude; // Устанавливаем амплитуду
        perlin.m_FrequencyGain = shakeFrequency; // Устанавливаем частоту

        yield return new WaitForSeconds(shakeDuration); // Ждем длительность тряски

        perlin.m_AmplitudeGain = 0; // Возвращаем амплитуду в 0
        perlin.m_FrequencyGain = 0; // Возвращаем частоту в 0
    }
}