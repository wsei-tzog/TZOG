using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // move
    Vector2 horizontalInput;
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 5f;
    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    // gravity && jump settings
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 5f;
    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    bool jump;

    // polish
    [SerializeField] WeaponSwing weaponSwing;
    private bool isRunning = false;
    private Animator animator;
    Vector3 horizontalVelocity;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // gravity
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);
        if (!isGrounded)
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }
        controller.Move(verticalVelocity * Time.deltaTime);

        // horizontal move
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed * Time.deltaTime;
        controller.Move(horizontalVelocity);
        AnimateRun();



        //jump

        if (jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = 0;
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jump = false;
        }

    }

    void AnimateRun()
    {
        isRunning = (horizontalInput.x < 0 || horizontalInput.x > 0) || (horizontalInput.y < 0 || horizontalInput.y > 0) ? true : false;
        weaponSwing.ReceiveRunningBool(isRunning);
        // animator.SetBool("isRunning", isRunning);
        // Debug.Log("isrunning " + isRunning);
        // Debug.Log("horizontalInput.x " + horizontalInput.x);
        // Debug.Log("horizontalInput.y " + horizontalInput.y);

    }

    public void OnJumpPressed()
    {
        jump = true;
    }
    // public void OnSprintPressed()
    // {
    //     sprint = true;
    // }
    // public void OnSprintReleased()
    // {
    //     sprint = false;
    // }

}
