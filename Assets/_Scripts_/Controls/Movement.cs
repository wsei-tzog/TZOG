using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region move
    Vector2 horizontalInput;
    [SerializeField] CharacterController controller;
    public float speed;
    public float sprintSpeed;
    public float normalSpeed;
    public AudioSource audioStepSource;
    public AudioClip[] audioClips;
    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
        // StopCoroutine(PlaySteps());

    }
    #endregion
    #region gravity && jump settings
    public float gravity = -9.81f;
    public float jumpHeight = 5f;
    Vector3 verticalVelocity = Vector3.zero;
    public LayerMask groundMask;
    bool isGrounded;
    bool jump;

    #endregion
    #region polish
    // public WeaponSwing weaponSwing;
    // private bool isRunning = false;
    private Animator animator;
    // Vector3 horizontalVelocity;
    #endregion
    void Awake()
    {
        animator = GetComponent<Animator>();
        speed = normalSpeed;
        OnSprintPressed(false);
    }

    void Update()
    {
        #region gravity
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);

        if (!isGrounded)
        {
            verticalVelocity.y += gravity * Time.deltaTime;
            audioStepSource.Stop();
        }
        controller.Move(verticalVelocity * Time.deltaTime);
        #endregion

        #region horizontal move
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed * Time.deltaTime;
        controller.Move(horizontalVelocity);

        if (horizontalVelocity.x != 0 || horizontalVelocity.z != 0 && isGrounded)
        {
            PlaySteps(true);
        }
        else
        {
            audioStepSource.Stop();
        }
        #endregion
    }

    // private IEnumerator PlaySteps()
    // {

    //     var duration = 1f;
    //     var timePassed = 0f;

    //     while (timePassed < duration)
    //     {
    //         float pitch = Random.Range(0.6f, 2);
    //         audioStepSource.pitch = pitch;
    //         float volume = Random.Range(0.3f, 1);
    //         audioStepSource.volume = volume;

    //         audioStepSource.Play();
    //         timePassed += Time.deltaTime;

    //         yield return null;
    //     }
    //     PlaySteps(true, true);
    // }

    public void PlaySteps(bool playSteps)
    {
        audioStepSource.pitch = Random.Range(0.85f, 1.3f);
        audioStepSource.volume = Random.Range(0.6f, 1);

        if (playSteps && !audioStepSource.isPlaying)
        {
            audioStepSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
        }
        // else if (!playSteps && audioStepSource.isPlaying)
        // {
        //     audioStepSource.Stop();
        // }

    }
    public void OnSprintPressed(bool sprint)
    {
        if (sprint)
        {
            speed = sprintSpeed;
            // play sound sprint
        }
        else if (!sprint)
        {
            speed = normalSpeed;
        }
    }
    public void OnJumpPressed()
    {
        if (isGrounded)
        {
            verticalVelocity.y = 0;
            verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
        }
        jump = false;
    }

}
