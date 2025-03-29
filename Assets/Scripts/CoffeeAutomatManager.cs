using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class OptionsNode
{
    public string option { get; set; }
    public OptionsNode leftOption { get; set; }
    public OptionsNode rightOption { get; set; }

    public OptionsNode(string setOption)
    {
        this.option = setOption;
        leftOption = null;
        rightOption = null;
    }

}
public class CoffeeAutomatOptions
{
    public OptionsNode root;
    private Queue<OptionsNode> _nodesQueue;

    public CoffeeAutomatOptions()
    {
        root = null;
        _nodesQueue = new Queue<OptionsNode>();
    }
    public void AddOption(string Option)
    {
        Debug.Log("ADD OPTION ENTER");
        OptionsNode newNode = new OptionsNode(Option);
        if (root == null)
        {
            Debug.Log("n == 0 - " + Option);
            root = newNode;
            _nodesQueue.Enqueue(root);
        }
        else
        {
            if (_nodesQueue.Count > 0)
            {
                OptionsNode current = _nodesQueue.Peek();
                if (current.leftOption == null)
                {
                    Debug.Log("n left == 0 - ADD TO LEFT " + Option);
                    current.leftOption = newNode;
                }
                else if (current.rightOption == null)
                {
                    Debug.Log("n right == 0 - ADD TO RIGHT " + Option);
                    current.rightOption = newNode;
                    _nodesQueue.Dequeue();
                }
                _nodesQueue.Enqueue(newNode);
            }
        }
    }
}

public class CoffeeAutomatManager : MonoBehaviour, IInteractable
{
    CoffeeAutomatOptions menu;
    CoffeeAutomatOptions menuTmp;

    [SerializeField] public CinemachineVirtualCamera _coffeeCamera;

    [Header("UI References")]
    [SerializeField] public TextMeshProUGUI optionTextBox;
    [SerializeField] public Button leftButton;
    [SerializeField] public TextMeshProUGUI leftButtonTxt;
    [SerializeField] public Button rightButton;
    [SerializeField] public TextMeshProUGUI rightButtonTxt;

    bool isUsed = false;
    bool IInteractable.Interact()
    {
        Debug.Log("Use the Coffee automat?");

        if (isUsed == false)
        {
            TurnCoffeeOn();

            //return true - player can't move
            return false;
        }
        else
        {
            TurnCoffeeOff();

            //return true - player can move
            return true;
        }
    }

    public void TurnCoffeeOn()
    {
        //setting the cursor
        GetComponent<CursorManager>().EnableCursor(Screen.height / 2);

        CoffeeAutomatOptionsSetup();

        //change camera
        _coffeeCamera.Priority = 20;

        optionTextBox.text = menu.root.option;
        leftButtonTxt.text = "{" + menu.root.leftOption.option + ")";
        rightButtonTxt.text = "{" + menu.root.rightOption.option + ")";

        leftButtonTxt.enabled = true;
        rightButtonTxt.enabled = true;
        leftButton.enabled = true;
        rightButton.enabled = true;

        isUsed = true;
    }

    public void TurnCoffeeOff()
    {
        //turning off the cursor
        GetComponent<CursorManager>().DisableCursor();

        //leave camera
        _coffeeCamera.Priority = 0;

        optionTextBox.text = "press \"E\" \n to run the machine";

        leftButtonTxt.enabled = false;
        rightButtonTxt.enabled = false;
        leftButton.enabled = false;
        rightButton.enabled = false;

        isUsed = false;
    }

    public void CoffeeAutomatOptionsSetup()
    {
        menu = new CoffeeAutomatOptions();
        menu.AddOption("bereit");

        menu.AddOption("espresso");

        menu.AddOption("latte");

        menu.AddOption("strong");
        menu.AddOption("strange");

        menu.AddOption("rat milk");
        menu.AddOption("oat milk");

        menu.AddOption("sugar");
        menu.AddOption("zucker");

        menu.AddOption("blue");
        menu.AddOption("red");

        menu.AddOption("male");
        menu.AddOption("female");

        menu.AddOption("ota");
        menu.AddOption("aot");

        Debug.Log("MENU = " + menu);

        menuTmp = menu;
    }

    public void LeftOptionOnClick()
    {
        Debug.Log("LEFT BUTTON WAS PRESSED");
        if (menu.root.leftOption != null)
        {
            Debug.Log("LEFT CHILD FROM " + menu.root.option + " IS NOT NULL");
            menuTmp = menu;
            menu.root = menuTmp.root.leftOption;

            optionTextBox.text = menu.root.option;
            if (menu.root.leftOption != null)
            {
                leftButtonTxt.text = "{" + menu.root.leftOption.option + ")";
                rightButtonTxt.text = "{" + menu.root.rightOption.option + ")";
            }
            else
            {
                optionTextBox.text = "machine is out of order";

                leftButtonTxt.enabled = false;
                rightButtonTxt.enabled = false;
                leftButton.enabled = false;
                rightButton.enabled = false;
            }
        }
    }

    public void RightOptionOnClick()
    {
        Debug.Log("RIGHT BUTTON WAS PRESSED");
        if (menu.root.rightOption != null)
        {
            Debug.Log("RIGHT CHILD FROM " + menu.root.option + " IS NOT NULL");
            menuTmp = menu;
            menu.root = menuTmp.root.rightOption;
            optionTextBox.text = menu.root.option;
            if (menu.root.leftOption != null)
            {
                leftButtonTxt.text = "{" + menu.root.leftOption.option + ")";
                rightButtonTxt.text = "{" + menu.root.rightOption.option + ")";
            }
            else
            {
                optionTextBox.text = "machine is out of order";

                leftButtonTxt.enabled = false;
                rightButtonTxt.enabled = false;
                leftButton.enabled = false;
                rightButton.enabled = false;
            }

        }
    }

    void Start()
    {
        CoffeeAutomatOptionsSetup();
        leftButton.onClick.AddListener(LeftOptionOnClick);
        rightButton.onClick.AddListener(RightOptionOnClick);
    }
}
