using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ComputerManager : MonoBehaviour, IInteractable
{
    [SerializeField] public CinemachineVirtualCamera _laptopCamera;

    [Header("UI References")]
    //[SerializeField] public UIManager UIManager;
    public UIManager UIManager;
    [SerializeField] public GameObject _laptopCanvas;

    [SerializeField] public TextMeshProUGUI JeffInfo;
    [SerializeField] public TextMeshProUGUI LucyInfo;
    [SerializeField] public TextMeshProUGUI KarenInfo;
    [SerializeField] public TextMeshProUGUI MailInfo;

    public Button JeffButton;
    public Button LucyButton;
    public Button KarenButton;
    public Button MailButton;

    //[SerializeField] public Button checkLaptop;
    public Sprite MailCheckSprite;
    public Sprite MailSprite;

    bool isUsed = false;

    bool IInteractable.Interact()
    {
        Debug.Log("Use the Laptopi?");

        if (isUsed == false)
        {
            TurnLaptopOn();

            //return true - player can't move
            return false;
        }
        else
        {
            TurnLaptopOff();

            //return true - player can move
            return true;
        }
    }

    public void SetButtonActive(string Button)
    {
        if (Button == "Mail")
        {
            MailButton.GetComponent<Image>().sprite = MailCheckSprite;
        }
        else if (Button == "Jeff")
        {
            JeffButton.GetComponent<Image>().color = Color.red;
        }
        else if (Button == "Lucy")
        {
            LucyButton.GetComponent<Image>().color = Color.red;
        }
        else if (Button == "Karen")
        {
            KarenButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            Debug.Log("Button name " + Button + "doesn't exist");
        }
    }

    public void TurnLaptopOn()
    {
        //setting the cursor
        GetComponent<CursorManager>().EnableCursor(Screen.height / 2);

        //turning off UI
        UIManager.SetTaskCompleted(false);

        //turn on canvas
        _laptopCanvas.SetActive(true);

        //change camera
        _laptopCamera.Priority = 20;

        isUsed = true;
    }
    public void TurnLaptopOff()
    {
        //turning off the cursor
        GetComponent<CursorManager>().DisableCursor();

        //turn off all canvas fields
        TurnInfosOff();
        _laptopCanvas.SetActive(false);
        
        //leave camera
        _laptopCamera.Priority = 0;

        isUsed = false;
    }
    public void TurnInfosOff()
    {
        JeffInfo.enabled = false;
        LucyInfo.enabled = false;
        KarenInfo.enabled = false;
        MailInfo.enabled = false;
        Debug.Log("MAIL INFO - " + MailInfo.enabled);
    }

    void NewMessageMail()
    {

    }

    void MailOnClick()
    {
        MailButton.GetComponent<Image>().sprite = MailSprite;
        TurnInfosOff();
        MailInfo.enabled = true;
    }
    void JeffOnClick()
    {
        //MailButton.GetComponent<Image>().sprite = MailSprite;
        JeffButton.GetComponent<Image>().color = Color.white;
        TurnInfosOff();
        JeffInfo.enabled = true;
    }
    void LucyOnClick()
    {
        //MailButton.GetComponent<Image>().sprite = MailSprite;
        LucyButton.GetComponent<Image>().color = Color.white;
        TurnInfosOff();
        LucyInfo.enabled = true;
    }
    void KarenOnClick()
    {
        //MailButton.GetComponent<Image>().sprite = MailSprite;
        KarenButton.GetComponent<Image>().color = Color.white;
        TurnInfosOff();
        KarenInfo.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        MailButton.onClick.AddListener(MailOnClick);
        JeffButton.onClick.AddListener(JeffOnClick);
        LucyButton.onClick.AddListener(LucyOnClick);
        KarenButton.onClick.AddListener(KarenOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsed)
        {
            //MailButton.onClick.AddListener(MailOnClick);
        }
    }
}
