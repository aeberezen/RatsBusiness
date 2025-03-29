using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//shaking camera effect
//blinking light effect

public class Effects : MonoBehaviour
{
    public static Effects Instance { get; private set; }

    [Header("Shaking camera effect")]
    private CinemachineFreeLook cinemachineFreeLook;
    private float shakeTimer;

    [Header("Blinking light effect")]
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    private float targetExposure;
    private float targetDuration;
    private float blinkTimer;
    public float currentMaxFrequency;
    public float currentMaxDuration;
    public float currentMaxExposure;
    private bool blinking = false;

    private void Awake()
    {
        Instance = this;
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }

    public void ShakeCamera(float intensity, float duration)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
        shakeTimer = duration;

        for (int i = 0; i < 3; i++)
        {
            cinemachineBasicMultiChannelPerlin =
            cinemachineFreeLook.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        }
    }

    public void BlinkLight(float maxExposure, float maxFrequency, float maxDuration) //how dark, how ofter, how fast
    {
        Debug.Log("BLINKING");

        blinking = true;

        currentMaxExposure = maxExposure;
        currentMaxFrequency = maxFrequency;
        currentMaxDuration = maxDuration;

        targetExposure = Random.Range(0, maxExposure);
        targetDuration = Random.Range(0, maxDuration);

        volume.profile.TryGet(out colorAdjustments);

        Debug.Log("targetExposure -" + targetExposure);
        Debug.Log("targetDuration -" + targetDuration);

        colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, targetExposure, Time.deltaTime * targetDuration);

        //blinkTimer = Time.time + Random.Range(0, maxFrequency);
        blinkTimer = 1f;
    }

    public void StopBlinkingLight()
    {
        blinking = false;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineFreeLook.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
                }
            }
        }
        if (blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0 && blinking)
            {
                BlinkLight(currentMaxExposure, currentMaxFrequency, currentMaxDuration);
            }
        }
    }
}
