using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodOption
{
    public string name;
    public bool vegan;
    public int price;
    public FoodOption(string setName, bool setVegan, int setPrice)
    {
        name = setName;
        vegan = setVegan;
        price = setPrice;
    }
}
public class FoodManager : MonoBehaviour, IInteractable
{
    [SerializeField] public CinemachineVirtualCamera _foodCamera;

    [Header("UI References")]
    [SerializeField] public TextMeshProUGUI Name;
    [SerializeField] public TextMeshProUGUI Vegan;
    [SerializeField] public TextMeshProUGUI Price;
    [SerializeField] public Button cheapest;
    [SerializeField] public Button vegan;
    [SerializeField] public Button meat;
    [SerializeField] public Button all;

    public FoodOption[] menu;
    public FoodOption[] menuTmp;

    bool isUsed = false;
    bool IInteractable.Interact()
    {
        Debug.Log("Use the Food automat?");

        if (isUsed == false)
        {
            TurnFoodOn();

            //return true - player can't move
            return false;
        }
        else
        {
            TurnFoodOff();

            //return true - player can move
            return true;
        }
    }

    public void PrintMenu(FoodOption[] _menu)
    {
        Name.text = "";
        Vegan.text = "";
        Price.text = "";

        foreach (FoodOption f in _menu)
        {
            Name.text += f.name + "\n";
            if (f.vegan)
            {
                Vegan.text += "YES\n";
            }
            else
            {
                Vegan.text += "NO\n";
            }
            Price.text += f.price + "\n";
        }
    }

    public void FoodSetup()
    {
        menu = new FoodOption[8];
        menu[0] = new FoodOption("ear", false, 3);
        menu[1] = new FoodOption("plant", true, 1);
        menu[2] = new FoodOption("cookie", true, 2);
        menu[3] = new FoodOption("liver", false, 5);
        menu[4] = new FoodOption("cockroach", false, 1);
        menu[5] = new FoodOption("apple", true, 2);
        menu[6] = new FoodOption("brain", false, 10);
        menu[7] = new FoodOption("pizza", false, 4);
        menuTmp = (FoodOption[])menu.Clone();
    }

    //bubble sort
    public void CheapestOnClick()
    {
        for (int i = 0; i < menuTmp.Length; i++)
        {
            for (int j = 0; j < menuTmp.Length - 1 - i; j++)
            {
                if (menuTmp[j].price > menuTmp[j+1].price)
                {
                    FoodOption tmp = menuTmp[j];
                    menuTmp[j] = menuTmp[j + 1];
                    menuTmp[j + 1] = tmp;
                }
            }
        }
        PrintMenu(menuTmp);
    }

    public void VeganOnClick()
    {
        Name.text = "";
        Vegan.text = "";
        Price.text = "";
        foreach (FoodOption f in menu)
        {
            if (f.vegan)
            {
                Name.text += f.name + "\n";
                Vegan.text += "YES\n";
                Price.text += f.price + "\n";
            }
        }
    }

    public void MeatOnClick()
    {
        Name.text = "";
        Vegan.text = "";
        Price.text = "";
        foreach (FoodOption f in menu)
        {
            if (!f.vegan)
            {
                Name.text += f.name + "\n";
                Vegan.text += "NO\n";
                Price.text += f.price + "\n";
            }
        }
    }

    public void AllOnClick()
    {
        PrintMenu(menu);
    }

    public void TurnFoodOn()
    {
        //setting the cursor
        GetComponent<CursorManager>().EnableCursor(Screen.height / 2);

        //change camera
        _foodCamera.Priority = 20;

        isUsed = true;
    }

    public void TurnFoodOff()
    {
        //turning off the cursor
        GetComponent<CursorManager>().DisableCursor();

        //leave camera
        _foodCamera.Priority = 0;

        isUsed = false;
    }

    private void Start()
    {
        cheapest.onClick.AddListener(CheapestOnClick);
        vegan.onClick.AddListener(VeganOnClick);
        meat.onClick.AddListener(MeatOnClick);
        all.onClick.AddListener(AllOnClick);

        FoodSetup();

        Name.text = "";
        Vegan.text = "";
        Price.text = "";
    }
}
