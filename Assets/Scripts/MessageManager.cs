using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public TextMeshProUGUI messageBox;
    public UIManager UIManager;
    public SoundManager soundManager;

    [Header("NPC Management")]
    public GameObject[] NPC_collision;
    //public bool waitsForNPC;
    //public bool NPC_onEnter;
    public GameObject MBIsBusy;
    public bool isNPC;

    [Header("Laptop Management")]
    public string[] JeffUpdate;
    public string[] LucyUpdate;
    public string[] KarenUpdate;
    public string[] MailUpdate;

    public ComputerManager laptop;

    [Header("Dialogue Management")]
    public string[] voiceAudioClip;

    //lines format - name:text
    public string[] lines;
    public float textSpeed = 0.05f;
    public float linesPause = 2f;
    public float failPause = 20f;

    public bool playable;
    public bool isTriggered;
    public bool laptopCheck;
    public bool failed;
    public bool done;
    //public bool myNPC;

    [Header("Effects Management")]
    public GameObject[] effects;
    public AudioClip newBackground;
    public int lineToStartEffect;

    [Header("Shake camera")]
    public bool shakeCamera;
    public float cameraShakeDuration;

    [Header("Blink light")]
    public bool blinkLight;
    public float maxFrequency;
    public float maxDuration;
    public float maxExposure;

    public Vector3 CollidingPos;
    private float alfaValue; //calculates text and voices fading
    private int index; //lines counter

    

    /* if regular MB - Done when all the lines were listened
     * if NPC MB - Done when reached the next point (doesn't have to be listened)
     * 
     * 
     * 
     */

    void Start()
    {
        //myNPC = false;
        failed = false;
        done = false;
        isTriggered = false;
        //waitsForNPC = false;
        MBIsBusy.SetActive(false);
        messageBox.text = string.Empty;
        index = 0;
        alfaValue = 1;
        CollidingPos = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        if (CollidingPos != new Vector3(0, 0, 0))
        {
            alfaValue = 1 - Vector3.Distance(CollidingPos, GetComponent<Transform>().position) / GetComponent<SphereCollider>().radius;

            if (alfaValue < 0.1)
            {
                alfaValue = 0;
            }
            Color textColor = messageBox.color;
            textColor.a = alfaValue;
            messageBox.color = textColor;

            soundManager.SetVolume(voiceAudioClip[index], alfaValue);
            if (isNPC) { soundManager.SetVolume("NPCSteps", alfaValue); }
        }
    }

    void WriteMessage()
    {
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            messageBox.text += c;
            //fail check. not supi, but have no a better idea:
            //if went too far AFTER 8th SYMBOL - fail
            if (messageBox.text.Length > 8 && alfaValue == 0 && !isNPC)
            {
                failed = true;
                Fail();
            }
            yield return new WaitForSeconds(textSpeed);
        }
        StartCoroutine(LinesPause());
    }
    IEnumerator LinesPause()
    {
        yield return new WaitForSeconds(linesPause);
        NextLine();
    }
    public IEnumerator FailPause()
    {
        yield return new WaitForSeconds(failPause);
        failed = false;
        playable = true;
        isTriggered = false;
    }

    void NextLine()
    {
        if (!failed)
        {
            if (index < lines.Length - 1)
            {
                messageBox.text = string.Empty;

                //voice managing
                index++;
                soundManager.Stop(voiceAudioClip[index-1]);
                soundManager.Play(voiceAudioClip[index], 0f);

                //effects managing
                if (effects != null)
                {
                    if (lineToStartEffect == index)
                    {
                        //play all the effects
                        if (shakeCamera)
                        {
                            //Effects effects = FindObjectOfType<Effects>();
                            Effects.Instance.ShakeCamera(10f, cameraShakeDuration);
                        }

                        if (blinkLight)
                        {
                            Effects.Instance.BlinkLight(maxExposure, maxFrequency, maxDuration);
                        }

                        foreach (GameObject g in effects) g.SetActive(true);

                        if (newBackground != null)
                        {
                            AudioSource audioSource = GetComponent<AudioSource>();
                            audioSource.clip = newBackground;
                            audioSource.Play();
                        }
                    }
                }

                StartCoroutine(TypeLine());
            }
            else
            {
                if (!isNPC) { Success(); }
            }
        }
        else
        {
            if (!isNPC) { Fail(); }
        }
    }

    void Fail()
    {
        Debug.Log("DIALOGUE FAILED");
        failed = true;
        playable = false;
        isTriggered = false;
        MBIsBusy.SetActive(false);
        messageBox.text = string.Empty;

        //voiceAudioClip[index].UnloadAudioData();
        soundManager.Stop(voiceAudioClip[index]);

        index = 0;
        alfaValue = 1;
        CollidingPos = new Vector3(0, 0, 0);

        StopAllCoroutines();

        UIManager.SetTaskFailed(true);
        if (!isNPC)
        {
            StartCoroutine(FailPause());
        }
    }

    public void Success()
    {
        Debug.Log("GOOD JOB");
        failed = false;
        playable = false;
        isTriggered = false;
        MBIsBusy.SetActive(false);
        messageBox.text = string.Empty;

        //voiceAudioClip[index].UnloadAudioData();
        soundManager.Stop(voiceAudioClip[index]);
        if(isNPC) { soundManager.Stop("NPCSteps"); }

        index = 0;
        alfaValue = 1;
        CollidingPos = new Vector3(0, 0, 0);

        StopAllCoroutines();

        if (laptopCheck)
        {
            LaptopUpdate();
        }

        done = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playable && other.CompareTag("Player"))
        {
            //START CONDITIONS
            if (!isTriggered && !MBIsBusy.active)
            { 
                Debug.Log("PLAYER CAME!");
                if (isNPC)
                {
                    //steps
                    soundManager.Play("NPCSteps", 0f);
                }

                isTriggered = true;

                //clear Fail UI
                UIManager.SetTaskFailed(false);

                MBIsBusy.SetActive(true);

                soundManager.Play(voiceAudioClip[index], 0f);
                WriteMessage();
            }
            CollidingPos = other.GetComponent<Transform>().position;
        }
        //when player enters Done NPC turn tag off
        if (isNPC && done && other.CompareTag("Player"))
        {
            GetComponent<AIMovement>().tag.SetActive(false);
        }
    }
    public void LaptopClear()
    {
        laptop.JeffInfo.text = "\n";
        laptop.LucyInfo.text = "\n";
        laptop.KarenInfo.text = "\n";
        laptop.MailInfo.text = "\n";
    }

    public void LaptopUpdate()
    {
        if (JeffUpdate.Length != 0)
        {
            laptop.SetButtonActive("Jeff");
            foreach (string s in JeffUpdate)
            {
                laptop.JeffInfo.text += "\n";
                laptop.JeffInfo.text += s;
            }
        }
        if (LucyUpdate.Length != 0)
        {
            laptop.SetButtonActive("Lucy");
            foreach (string s in LucyUpdate)
            {
                laptop.LucyInfo.text += "\n";
                laptop.LucyInfo.text += s;
            }
        }
        if (KarenUpdate.Length != 0)
        {
            laptop.SetButtonActive("Karen");
            foreach (string s in KarenUpdate)
            {
                laptop.KarenInfo.text += "\n";
                laptop.KarenInfo.text += s;
            }
        }
        if (MailUpdate.Length != 0)
        {
            UIManager.SetTaskCompleted(true);
            laptop.SetButtonActive("Mail");
            foreach (string s in MailUpdate)
            {
                laptop.MailInfo.text = "\n\n";
                laptop.MailInfo.text += s;
            }
        }
    }
}
