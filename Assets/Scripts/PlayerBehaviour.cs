using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    public Transform camera;
    public Animator animator;
    public GameObject flag;
    private GameObject currentFlag;
    public SoundManager soundManager;

    //public AudioClip[] walkingAudioClips;
    AudioClip currentClip;

    [Header("Flags Management")]
    //public GameObject flagDeleteUI;
    public GameObject flagLimitUI;
    public List<GameObject> flags = new List<GameObject>();
    public int flagLimit = 3;

    [Header("Movement Settings")]
    [SerializeField] public bool canMove;
    public float movementSpeed = 5f;
    public float rotationTime = 0.5f;
    public float stepsDelay;
    private float rotationSpeed;
    private float targetAngle;

    private Vector3 moveDirection;

    [Header("Camera Settings")]
    public CinemachineFreeLook fl;
    public AudioListener listener;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("States")]
    bool grounded;

    public void PlaceFlag()
    {
        if (flags.Count >= flagLimit)
        {
            flagLimitUI.SetActive(true);
        }
        else
        {
            currentFlag = Instantiate(flag, new Vector3(controller.transform.position.x, 2.69f, controller.transform.position.z + 1f), Quaternion.Euler(0f, targetAngle + 90f, 0f));
            flags.Add(currentFlag);
            currentFlag.GetComponent<FlagManager>().ChangeColor();
        }
    }

    private void WarpToComputer()
    {
        //check on Y(that character doesn't fly)
        controller.enabled = false;
        Vector3 computerPos = new Vector3(177f, 0f, 196f);
        controller.transform.position = computerPos;
        controller.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        camera.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
        controller.enabled = true;
    }

    private void InputManager()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        canMove = true;

        if (PlayerPrefs.GetInt("FromLoad", 0) == 1)
        {
            PlayerPrefs.SetInt("FromLoad", 0);
            WarpToComputer();
        }
    }

    void Update()
    {
        InputManager();

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        if (movement.magnitude >= 0.1f && canMove)
        {
            //doesn't work
            //PlayWalkSound();
            soundManager.Play("PlayerSteps", stepsDelay);
            animator.SetBool("IsWalking", true);
            
            //player rotation
            targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, rotationTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            //direction + camera
            Vector3 movementDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //move character
            controller.Move(movementDir.normalized * movementSpeed * Time.deltaTime);

            //check on Y (that character doesn't fly)
            controller.enabled = false;
            Vector3 yCheck = controller.transform.position;
            yCheck.y = 0;
            controller.transform.position = yCheck;
            controller.enabled = true;
        }
        else { animator.SetBool("IsWalking", false); }

        /*
        if (Input.GetKeyDown("f"))
        {
            PlaceFlag();
        }
        */
    }
}
