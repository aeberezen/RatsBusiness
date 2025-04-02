using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//shaking camera effect
//blinking light effect
//color fog effect
//camera lookAt effect

public class Effects : MonoBehaviour
{
    public static Effects Instance { get; private set; }

    [Header("Shaking camera effect")]
    private CinemachineFreeLook cinemachineFreeLook;
    private float shakeTimer;
    public bool isShaking = false; //light shake and shake from MessageManager could possibly overlap

    [Header("Blinking light effect")]
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    private float targetExposure;
    private float targetDuration;
    private float blinkTimer;
    public float currentMaxFrequency;
    public float currentMaxDuration;
    public float currentMaxExposure;
    private bool isBlinking = false;

    [Header("Color fog effect")]
    public ParticleSystem[] fog;

    //REVIEW
    [Header("Camera lookAt effect")]
    public CinemachineFreeLook freeLookCam; // FreeLook камера
    public Transform player;  // Игрок
    public Transform[] cameraTargets;  // Объект, на который хотим сфокусироваться
    public float focusDuration = 1f;  // Время, сколько камера остается на объекте
    public float transitionSpeed = 2f;  // Скорость перехода

    private void Awake()
    {
        Instance = this;
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        DontDestroyOnLoad(gameObject);
    }

    public void ShakeCamera(float intensity, float duration)
    {
        isShaking = true;
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

        isBlinking = true;

        currentMaxExposure = maxExposure;
        currentMaxFrequency = maxFrequency;
        currentMaxDuration = maxDuration;

        targetExposure = Random.Range(0, maxExposure);
        targetDuration = Random.Range(0.5f, maxDuration);

        volume.profile.TryGet(out colorAdjustments);

        if (!isShaking)
        {
            ShakeCamera(5f, targetDuration);
        }

        StopCoroutine("BlinkCoroutine");
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        float startExposure = colorAdjustments.postExposure.value;
        float elapsedTime = 0f;

        while (elapsedTime < targetDuration)
        {
            colorAdjustments.postExposure.value = Mathf.Lerp(startExposure, targetExposure, elapsedTime / targetDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        colorAdjustments.postExposure.value = targetExposure;

        //back to normal light
        StopCoroutine("BlinkResetCoroutine");
        StartCoroutine(BlinkResetCoroutine());
    }

    private IEnumerator BlinkResetCoroutine()
    {
        float startExposure = colorAdjustments.postExposure.value;
        float elapsedTime = 0f;

        while (elapsedTime < targetDuration)
        {
            colorAdjustments.postExposure.value = Mathf.Lerp(startExposure, -0.6f, elapsedTime / targetDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        colorAdjustments.postExposure.value = -0.6f;

        //waiting till the next blink
        yield return new WaitForSeconds(Random.Range(0.1f, currentMaxFrequency));

        if (isBlinking)
        {
            BlinkLight(currentMaxExposure, currentMaxFrequency, currentMaxDuration);
        }
    }

    public void StopBlinkingLight()
    {
        isBlinking = false;
    }

    public void ColorFog(Color color)
    {
        fog = FindObjectsOfType<ParticleSystem>();
        foreach (ParticleSystem ps in fog)
        {
            var main = ps.main;
            main.startColor = color;
        }
    }

    //REVIEW
    public void CameraLookAt(int cameraTargetNum)
    {
        StartCoroutine(SmoothFocus(cameraTargets[cameraTargetNum]));
    }

    private IEnumerator SmoothFocus(Transform newTarget)
    {
        Transform originalLookAt = freeLookCam.LookAt;

        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * transitionSpeed;
            freeLookCam.LookAt = newTarget;
            yield return null;
        }

        yield return new WaitForSeconds(focusDuration);

        time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * transitionSpeed;
            freeLookCam.LookAt = originalLookAt;
            yield return null;
        }
    }

    //REVIEW
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    isShaking = false;
                    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineFreeLook.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
                }
            }
        }
        /*
        if (blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0 && blinking)
            {
                BlinkLight(currentMaxExposure, currentMaxFrequency, currentMaxDuration);
            }
        }
        */
    }
}
