using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("GameUI References")]
    [SerializeField] public Button checkLaptop;
    [SerializeField] public TextMeshProUGUI checkLaptopText;
    [SerializeField] public Button taskFailed;
    [SerializeField] public TextMeshProUGUI taskFailedText;

    [Header("PauseMenuUI References")]
    [SerializeField] public GameObject mainMenuButton;
    [SerializeField] public GameObject quitButton;
    [SerializeField] public GameObject backButton;
    [SerializeField] public GameObject PauseMenuUI;
    [SerializeField] public GameObject progressInfo;
    [SerializeField] public GameObject instructions;
    [SerializeField] public GameObject map;

    [SerializeField] public SoundManager soundManager;

    private Color epColor;

    public GameObject LoadingScene;
    public Image LoadingBarFill;

    private bool isPaused = false;
    Dictionary<int, int> Episodes = new Dictionary<int, int>()
    {
        { 1, 1 },
        { 5, 2 },
        { 7, 3 },
        { 11, 4 },
        { 13, 5 }
    };

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MenuScene");

        LoadingScene.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }
    public void SetTaskFailed(bool set)
    {
        //checkLaptop.GetComponent<Image>().enabled = false;
        //checkLaptopText.enabled = false;

        taskFailed.GetComponent<Image>().enabled = set;
        taskFailedText.enabled = set;
    }

    public void SetTaskCompleted(bool set)
    {
        checkLaptop.GetComponent<Image>().enabled = set;
        checkLaptopText.enabled = set;

        //taskFailed.GetComponent<Image>().enabled = false;
        //taskFailedText.enabled = false;
    }

    public void ClearUI()
    {
        checkLaptop.GetComponent<Image>().enabled = false;
        taskFailed.GetComponent<Image>().enabled = false;
        
        checkLaptopText.enabled = false;
        taskFailedText.enabled = false;
    }

    public void Map()
    {
        if (Input.GetKeyDown("m"))
        {
            map.SetActive(!map.activeSelf);
        }
    }

    private void GamePause()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (!isPaused)
            {
                isPaused = true;
                Time.timeScale = 0;
                PauseMenuUI.SetActive(true);
                GetComponent<CursorManager>().EnableCursor(Screen.height / 2);
                soundManager.PauseAll();
            }
            else
            {
                //set MenuUI to default
                MenuUISet(true);

                isPaused = false;
                Time.timeScale = 1;
                PauseMenuUI.SetActive(false);
                Debug.Log("CURSOR HAS TO BE DISABLED");
                GetComponent<CursorManager>().DisableCursor();
                soundManager.ResumeAll();
            }
        }
    }

    public void QuitOnClick()
    {
        //when there progress to lose, throw warning
        if (!Episodes.ContainsKey(PlayerPrefs.GetInt("Scene", 0)) && PlayerPrefs.GetInt("Scene", 0) != 0 && progressInfo.active == false)
        {
            progressInfo.SetActive(true);
        }
        else
        {
            //MenuUISet(true);
            progressInfo.SetActive(false);
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1;
            Application.Quit();
        }
    }
    public void MainMenuOnClick()
    {
        //when there progress to lose, throw warning
        if (!Episodes.ContainsKey(PlayerPrefs.GetInt("Scene", 0)) && PlayerPrefs.GetInt("Scene", 0) != 0 && progressInfo.active == false)
        {
            progressInfo.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            progressInfo.SetActive(false);
            MenuUISet(true);
            StartCoroutine(LoadSceneAsync());
        }
    }

    public void MenuUISet(bool value)
    {
        quitButton.SetActive(value);
        mainMenuButton.SetActive(value);
        backButton.SetActive(!value);
        progressInfo.SetActive(!value);
        instructions.SetActive(value);
    }

    private void Start()
    {
        quitButton.GetComponent<Button>().onClick.AddListener(QuitOnClick);
        mainMenuButton.GetComponent<Button>().onClick.AddListener(MainMenuOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        GamePause();
        Map();
    }
}
