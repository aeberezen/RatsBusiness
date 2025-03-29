using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance { get; private set; }

    [Header("MB ferences")]
    [SerializeField] public GameObject[] triggerBoxes;

    // game state - episode num
    Dictionary<int, int> Episodes = new Dictionary<int, int>()
    {
        { 1, 1 },
        { 5, 2 },
        { 7, 3 },
        { 11, 4 },
        { 13, 5 }
    };

    public int currentStoryState;

    public Button checkLaptopButton;

    public void CheckStoryState()
    {
        if (triggerBoxes[currentStoryState].GetComponent<MessageManager>().done && !checkLaptopButton.GetComponent<Image>().enabled && currentStoryState < 15)
        {
            currentStoryState++;
            PlayerPrefs.SetInt("Scene", currentStoryState);
            if(currentStoryState > PlayerPrefs.GetInt("Progress", currentStoryState))
            {
                PlayerPrefs.SetInt("Progress", currentStoryState);
            }
            PlayerPrefs.Save();
            triggerBoxes[currentStoryState].GetComponent<MessageManager>().playable = true;
        }

        //if episode switches, save and unlock it
        if (Episodes.ContainsKey(currentStoryState))
        {
            //UI game - "GAME SAVED"
        }
    }

    public void SetStoryState(int StoryState)
    {
        currentStoryState = StoryState;

        for (int i = 0; i < 15; i++)
        {
            triggerBoxes[i].GetComponent<MessageManager>().playable = false;
            if (i == currentStoryState)
            {
                triggerBoxes[i].GetComponent<MessageManager>().playable = true;
            }
            
        }

        //setting Computer info from previous states
        triggerBoxes[0].GetComponent<MessageManager>().LaptopClear();
        for (int i = 0; i < currentStoryState; i++)
        {
            triggerBoxes[i].GetComponent<MessageManager>().LaptopUpdate();
        }
    }

    private void Awake()
    {
        //getting story state from the main menu to load correct episode/start new game
        //PlayerPrefs.SetInt("Scene", 11); // FOR TESTING
        currentStoryState = PlayerPrefs.GetInt("Scene", 0);
        SetStoryState(currentStoryState);

        if (instance != null)
        {
            Debug.LogError("There is more than one Story Manager!");
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        CheckStoryState();
    }
}
