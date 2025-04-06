using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance { get; private set; }

    [Header("MB references")]
    public GameObject[] triggerBoxes;

    [Header("UI ferences")]
    [SerializeField] public GameObject gameSaved;
    [SerializeField] public GameObject keysTutorial;

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

    private bool isSaved = false;
    private bool fromLoad = false;
    private bool settingStory = true;

    public void CheckStoryState()
    {
        if (currentStoryState < 14 && triggerBoxes[currentStoryState].GetComponent<MessageManager>().done && !checkLaptopButton.GetComponent<Image>().enabled)
        {
            isSaved = false;
            fromLoad = false;

            currentStoryState++;
            Debug.Log("CurrentState did ++! - " + currentStoryState);

            PlayerPrefs.SetInt("Scene", currentStoryState);
            if(currentStoryState > PlayerPrefs.GetInt("Progress", currentStoryState))
            {
                PlayerPrefs.SetInt("Progress", currentStoryState);
            }
            PlayerPrefs.Save();
            triggerBoxes[currentStoryState].GetComponent<MessageManager>().playable = true;
        }

        //if episode switches, save and unlock it
        if (Episodes.ContainsKey(currentStoryState) && !isSaved && !fromLoad)
        {
            //UI game - "GAME SAVED"
            isSaved = true;
            gameSaved.SetActive(true);
            StartCoroutine(HideAfterTime(gameSaved, 5f));
        }
    }

    private IEnumerator HideAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    public void SetStoryState(int StoryState)
    {
        fromLoad = true;
        currentStoryState = StoryState;
        /*
            for (int i = 0; i < 15; i++)
            {
                triggerBoxes[i].GetComponent<MessageManager>().playable = false;
                triggerBoxes[i].GetComponent<MessageManager>().done = true; //
                if (i == currentStoryState)
                {
                    triggerBoxes[i].GetComponent<MessageManager>().playable = true;
                    triggerBoxes[i].GetComponent<MessageManager>().done = false; //
                }
            }
        */

        triggerBoxes[0].GetComponent<MessageManager>().LaptopClear();
        for (int i = 0; i < currentStoryState; i++)
        {
            Debug.Log("Set StoryState is going for story num - " + i);
            triggerBoxes[i].GetComponent<MessageManager>().SuccessFromLoad();
            //TO FIX
            //turning on all the effects
            triggerBoxes[i].GetComponent<MessageManager>().EffectsCheck(fromLoad);
        }

        triggerBoxes[currentStoryState].GetComponent<MessageManager>().playable = true;

        settingStory = false;
    }

    private void Awake()
    {
        //getting story state from the main menu to load correct episode/start new game
        //PlayerPrefs.SetInt("Scene", 11); // FOR TESTING
        currentStoryState = PlayerPrefs.GetInt("Scene", 0);
        //SetStoryState(currentStoryState);

        keysTutorial.SetActive(true);
        StartCoroutine(HideAfterTime(keysTutorial, 5f));

        if (instance != null)
        {
            Debug.LogError("There is more than one Story Manager!");
        }
        instance = this;
    }

    private void Start()
    {
        settingStory = true;
        SetStoryState(currentStoryState);
    }

    // Update is called once per frame
    void Update()
    {
        if (!settingStory)
        {
            CheckStoryState();
        }
    }
}
