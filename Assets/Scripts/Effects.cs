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

    [Header("MB with ShakeCamera effect references")]
    [SerializeField] public GameObject[] effectsTriggerBoxes;

    [Header("Shaking camera effect")]
    private CinemachineFreeLook cinemachineFreeLook;
    private float shakeTimer;
    public bool isShaking = false; //light shake and shake from MessageManager could possibly overlap

    [Header("Blinking light effect")]
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    private Coroutine blinkCoroutine;
    private Coroutine blinkResetCoroutine;
    private float targetExposure;
    private float targetDuration;
    private float blinkTimer;
    public float currentMaxFrequency;
    public float currentMaxDuration;
    public float currentMinExposure;
    public float currentMaxExposure;
    private bool isBlinking = true;
    private bool isWithShakeCamera = true;

    [Header("Color effect")]
    public ParticleSystem[] fog;
    public Camera camera;

    [Header("Camera lookAt effect")]
    public CinemachineFreeLook freeLookCam;
    public Transform player;
    public Transform[] cameraTargets;
    public float focusDuration = 1f;
    public float transitionSpeed = 2f;

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

    public void StartBlinkLight(float minExposure, float maxExposure, float maxFrequency, float maxDuration, bool withShakeCamera)
    {
        Debug.Log("HERES THE NEW withShakeCamera = " + withShakeCamera);
        isWithShakeCamera = withShakeCamera;
        Debug.Log("HERES THE NEW ISwithShakeCamera = " + isWithShakeCamera);
        isBlinking = true;
        BlinkLight(minExposure, maxExposure, maxFrequency, maxDuration, isWithShakeCamera);
    }

    public void StopBlinkingLight()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        if (blinkResetCoroutine != null)
        {
            StopCoroutine(blinkResetCoroutine);
            blinkResetCoroutine = null;
        }
        isBlinking = false;
        isWithShakeCamera = false;
    }

    public void BlinkLight(float minExposure, float maxExposure, float maxFrequency, float maxDuration, bool withShakeCamera) //how dark, how ofter, how fast
    {
        Debug.Log("Max Exp - " + maxExposure + "Max Freq - " + maxFrequency + "WITH SHAKE? -" + withShakeCamera);
        if (isBlinking)
        {
            Debug.Log("BLINKING");
            //isBlinking = true;

            currentMinExposure = minExposure;
            currentMaxExposure = maxExposure;
            currentMaxFrequency = maxFrequency;
            currentMaxDuration = maxDuration;

            targetExposure = Random.Range(0, maxExposure);
            targetDuration = Random.Range(0.5f, maxDuration);

            volume.profile.TryGet(out colorAdjustments);

            //TO FIX
            if (!isShaking && withShakeCamera)
            {
                ShakeCamera(5f, targetDuration);
            }

            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }

            blinkCoroutine = StartCoroutine(BlinkCoroutine());
        }
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
        if (blinkResetCoroutine != null)
        {
            StopCoroutine(blinkResetCoroutine);
        }

        blinkResetCoroutine = StartCoroutine(BlinkResetCoroutine());
    }

    private IEnumerator BlinkResetCoroutine()
    {
        float startExposure = colorAdjustments.postExposure.value;
        float elapsedTime = 0f;

        while (elapsedTime < targetDuration)
        {
            colorAdjustments.postExposure.value = Mathf.Lerp(startExposure, currentMinExposure, elapsedTime / targetDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        colorAdjustments.postExposure.value = currentMinExposure;

        //waiting till the next blink
        yield return new WaitForSeconds(Random.Range(0.1f, currentMaxFrequency));

        if (isBlinking)
        {
            BlinkLight(currentMinExposure, currentMaxExposure, currentMaxFrequency, currentMaxDuration, isWithShakeCamera);
        }
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

    public void ColorBackground(Color color)
    {
        camera.backgroundColor = color;
    }

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

    void Update()
    {
        //stop ShakeCamera after time
        if (shakeTimer > 0 && isShaking)
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
    }
}
