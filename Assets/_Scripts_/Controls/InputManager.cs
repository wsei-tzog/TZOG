using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputManager : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    public static GunSystem gunSystem;
    public static PickUpController pickUpController;
    // [SerializeField] WeaponSwing weaponSwing;
    PlayerControls controls;
    PlayerControls.GroundMovementActions groundMovement;
    PlayerControls.InteractionsActions interaction;
    Vector2 horizontalInput;
    Vector2 mouseInput;
    private void Awake()
    {
        controls = new PlayerControls();
        groundMovement = controls.GroundMovement;
        interaction = controls.Interactions;
        // movement
        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        groundMovement.Jump.performed += _ => movement.OnJumpPressed();
        // groundMovement.Sprint.performed += _ => movement.sprint = true;

        // mouse
        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        // interaction
        interaction.Shoot.performed += ctx => gunSystem.OnShootPressed();
        interaction.Reload.performed += ctx => gunSystem.OnReloadPressed();

        interaction.PickUp.performed += _ => mouseLook.OnPickUpPressed();
        interaction.Drop.performed += _ => pickUpController.OnDropPressed();

    }

    private void Update()
    {

        // foreach (GameObject t in transform)
        // {
        //     if (t.CompareTag("Weapon"))
        //     {
        //         var script = t.GetComponentInChildren<GunSystem>();
        //         gunSystem = script;
        //     }
        // }

        movement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
        if (interaction.ShootSeries.ReadValue<float>() > 0.1f)
        {
            gunSystem.ReceiveInput(true);
        }

    }


    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
