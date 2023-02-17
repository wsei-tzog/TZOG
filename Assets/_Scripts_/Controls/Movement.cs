using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Movement : MonoBehaviour
{
    [Header("Move")]
    #region move
    Vector2 horizontalInput;
    [SerializeField] CharacterController controller;
    float speed;
    public float sprintSpeed;
    public float normalSpeed;

    #endregion


    [Header("Gravity && Jump")]
    #region gravity && jump settings
    public float gravity = -9.81f;
    float jumpHeight;
    public float normalJumpHeigt = 5f;
    public float sprintJumpHeigt = 7f;
    Vector3 verticalVelocity = Vector3.zero;
    public LayerMask groundMask;
    public bool isGrounded, isRunning;
    // bool jump;
    bool justJumped;
    private Animator animator;
    #endregion

    [Header("Sound")]
    #region sound
    public AudioSource audioStepSource;
    public AudioClip[] audioStepClips;
    public AudioClip[] audioJumpClips;
    public AudioClip[] audioLandClips;
    public AudioClip[] audioSprintClips;

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;

    }

    #endregion

    [Header("Life")]
    #region Life
    public float Health;
    DamageScreenEffect damageScreen;
    MouseLook mouseLook;
    EscController escController;


    private void Start()
    {
        damageScreen = FindObjectOfType<DamageScreenEffect>();
        Camera.main.GetComponent<AudioListener>().enabled = true;
    }

    #endregion
    public void TakeDamage(float damage)
    {
        float maxHealth = 100f;
        float flashDurationHealth = maxHealth / Health;
        float flashDurationHealthWithoutOne = flashDurationHealth - 1;

        float flashDuration = Mathf.Clamp(flashDurationHealthWithoutOne, 0.1f, 4);
        // damageScreen.flashDuration = flashDuration;
        Health -= damage;

        // float flashDuration = 0.25f + flashFactor;
        Debug.Log("Flash durtation " + flashDuration);

        StartCoroutine(damageScreen.Blooding(flashDuration, Health));

        if (Health <= 0)
        {
            mouseLook.enabled = false;
            damageScreen.StartCoroutine("Dead");

        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        speed = normalSpeed;
        OnSprintPressed(false);
        mouseLook = FindObjectOfType<MouseLook>();
        escController = FindObjectOfType<EscController>();
    }
    void Update()
    {
        // Set the skybox's color based on the current amount of red


        #region horizontal move
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed * Time.deltaTime;
        controller.Move(horizontalVelocity);
        #endregion

        #region gravity
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);

        if (!isGrounded)
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        if (!Physics.CheckSphere(transform.position, 0.1f, groundMask))
        {
            justJumped = true;
        }

        if (justJumped)
        {
            if (Physics.CheckSphere(transform.position, 0.1f, groundMask))
            {
                audioStepSource.pitch = Random.Range(0.85f, 1.3f);
                audioStepSource.volume = Random.Range(0.6f, 1);
                audioStepSource.PlayOneShot(audioLandClips[Random.Range(0, audioLandClips.Length)]);
                Debug.Log("Landing");
                justJumped = false;
            }
        }

        controller.Move(verticalVelocity * Time.deltaTime);
        #endregion

        if ((horizontalVelocity.x != 0 || horizontalVelocity.z != 0) && isGrounded)
        {
            if (!isRunning)
            {
                PlaySteps(true, false);
            }
            else
            {
                PlaySteps(true, true);
            }

        }

    }
    public void PlaySteps(bool playSteps, bool running)
    {
        if (playSteps && !audioStepSource.isPlaying && !running)
        {
            speed = normalSpeed;
            jumpHeight = normalJumpHeigt;

            audioStepSource.pitch = Random.Range(0.85f, 1.3f);
            audioStepSource.volume = Random.Range(0.6f, 1);
            audioStepSource.PlayOneShot(audioStepClips[Random.Range(0, audioStepClips.Length)]);
        }
        else if (playSteps && !audioStepSource.isPlaying && running)
        {
            speed = sprintSpeed;
            jumpHeight = sprintJumpHeigt;

            audioStepSource.pitch = Random.Range(1.5f, 2f);
            audioStepSource.volume = Random.Range(0.6f, 1);
            audioStepSource.PlayOneShot(audioSprintClips[Random.Range(0, audioSprintClips.Length)]);
        }
    }
    public void OnJumpPressed()
    {
        if (isGrounded)
        {
            audioStepSource.pitch = Random.Range(0.85f, 1.3f);
            audioStepSource.volume = Random.Range(0.6f, 1);
            audioStepSource.PlayOneShot(audioJumpClips[Random.Range(0, audioJumpClips.Length)]);
            verticalVelocity.y = 0;
            verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
        }
        // jump = false;
    }
    public void OnSprintPressed(bool sprint)
    {
        isRunning = sprint;
    }

}
