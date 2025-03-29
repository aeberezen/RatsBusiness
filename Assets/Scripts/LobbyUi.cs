using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button quitButton;

    [SerializeField] public Button ep1Button;
    [SerializeField] public Button ep2Button;
    [SerializeField] public Button ep3Button;
    [SerializeField] public Button ep4Button;
    [SerializeField] public Button ep5Button;
    [SerializeField] public GameObject epInfo;
    [SerializeField] public GameObject progressInfo;

    private Color epColor;

    public GameObject LoadingScene;
    public Image LoadingBarFill;

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");

        LoadingScene.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        loadGameButton.onClick.AddListener(LoadGameOnClick);
        newGameButton.onClick.AddListener(NewGameOnClick);
        quitButton.onClick.AddListener(QuitOnClick);

        ep1Button.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("Scene", 1);
            PlayerPrefs.SetInt("FromLoad", 1);
            StartCoroutine(LoadSceneAsync());
        });

        ep2Button.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("Scene", 5);
            PlayerPrefs.SetInt("FromLoad", 1);
            StartCoroutine(LoadSceneAsync());
        });

        ep3Button.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("Scene", 7);
            PlayerPrefs.SetInt("FromLoad", 1);
            StartCoroutine(LoadSceneAsync());
        });

        ep4Button.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("Scene", 11);
            PlayerPrefs.SetInt("FromLoad", 1);
            StartCoroutine(LoadSceneAsync());
        });

        ep5Button.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("Scene", 13);
            PlayerPrefs.SetInt("FromLoad", 1);
            StartCoroutine(LoadSceneAsync());
        });
    }
    private void NewGameOnClick()
    {
        if (PlayerPrefs.GetInt("Progress", 0) != 0 && progressInfo.active == false)
        {
            progressInfo.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("Progress", 0);
            PlayerPrefs.SetInt("Scene", 0);
            PlayerPrefs.SetInt("FromLoad", 0);
            StartCoroutine(LoadSceneAsync());
            //SceneManager.LoadScene("GameScene");
        }
    }

    private void LoadGameOnClick()
    {
        if (PlayerPrefs.GetInt("Progress", 0) == 0)
        {
            epInfo.SetActive(true);
            epColor = ep1Button.GetComponent<Image>().color;
            epColor.a = 0.1f;
            ep1Button.GetComponent<Image>().color = epColor;
            ep2Button.GetComponent<Image>().color = epColor;
            ep3Button.GetComponent<Image>().color = epColor;
            ep4Button.GetComponent<Image>().color = epColor;
            ep5Button.GetComponent<Image>().color = epColor;
        }
        else
        {
            //super ugly, but it's 5 in the morning and i'm too tired to optimise
            epInfo.SetActive(false);
            if (PlayerPrefs.GetInt("Progress", 0) >= 1 && PlayerPrefs.GetInt("Progress", 0) < 5)
            {
                ep1Button.interactable = true;
                epColor = ep1Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep1Button.GetComponent<Image>().color = epColor;

            }
            if (PlayerPrefs.GetInt("Progress", 0) >= 5 && PlayerPrefs.GetInt("Progress", 0) < 7)
            {
                ep1Button.interactable = true;
                epColor = ep1Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep1Button.GetComponent<Image>().color = epColor;

                ep2Button.interactable = true;
                epColor = ep2Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep2Button.GetComponent<Image>().color = epColor;
            }
            if (PlayerPrefs.GetInt("Progress", 0) >= 7 && PlayerPrefs.GetInt("Progress", 0) < 11)
            {
                ep1Button.interactable = true;
                epColor = ep1Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep1Button.GetComponent<Image>().color = epColor;

                ep2Button.interactable = true;
                epColor = ep2Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep2Button.GetComponent<Image>().color = epColor;

                ep3Button.interactable = true;
                epColor = ep3Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep3Button.GetComponent<Image>().color = epColor;
            }
            if (PlayerPrefs.GetInt("Progress", 0) >= 11 && PlayerPrefs.GetInt("Progress", 0) < 13)
            {
                ep1Button.interactable = true;
                epColor = ep1Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep1Button.GetComponent<Image>().color = epColor;

                ep2Button.interactable = true;
                epColor = ep2Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep2Button.GetComponent<Image>().color = epColor;

                ep3Button.interactable = true;
                epColor = ep3Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep3Button.GetComponent<Image>().color = epColor;

                ep4Button.interactable = true;
                epColor = ep4Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep4Button.GetComponent<Image>().color = epColor;
            }
            if (PlayerPrefs.GetInt("Progress", 0) >= 13)
            {
                ep1Button.interactable = true;
                epColor = ep1Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep1Button.GetComponent<Image>().color = epColor;

                ep2Button.interactable = true;
                epColor = ep2Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep2Button.GetComponent<Image>().color = epColor;

                ep3Button.interactable = true;
                epColor = ep3Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep3Button.GetComponent<Image>().color = epColor;

                ep4Button.interactable = true;
                epColor = ep4Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep4Button.GetComponent<Image>().color = epColor;

                ep5Button.interactable = true;
                epColor = ep5Button.GetComponent<Image>().color;
                epColor.a = 1f;
                ep5Button.GetComponent<Image>().color = epColor;
            }
        }
    }

    private void QuitOnClick()
    {
        Application.Quit();
    }
}
