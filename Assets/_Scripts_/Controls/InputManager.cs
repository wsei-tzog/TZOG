using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputManager : MonoBehaviour
{
    public Movement movement;
    public MouseLook mouseLook;
    public static GunSystem gunSystem;
    public MinimapController minimapController;
    public QuestController questController;
    public EscController escController;
    public static PickUpController pickUpController;
    public static Torch torch;
    // [SerializeField] WeaponSwing weaponSwing;
    PlayerControls controls;
    PlayerControls.GroundMovementActions groundMovement;
    PlayerControls.InteractionsActions interaction;
    Vector2 horizontalInput;
    Vector2 mouseInput;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controls = new PlayerControls();
        groundMovement = controls.GroundMovement;
        interaction = controls.Interactions;
        // movement
        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        groundMovement.Jump.performed += _ => movement.OnJumpPressed();


        // mouse
        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        // interaction
        interaction.Shoot.performed += ctx => gunSystem.OnShootPressed();
        interaction.Reload.performed += ctx => gunSystem.OnReloadPressed();

        interaction.PickUp.performed += _ => mouseLook.OnPickUpPressed();
        interaction.Drop.performed += _ => pickUpController.OnDropPressed();
        interaction.TorchSwitch.performed += _ => torch.OnTorchSwitchPressed();

        interaction.Map.performed += _ => minimapController.OnMapPressed();
        interaction.Mission.performed += _ => questController.OnMissionPressed();
        interaction.Esc.performed += _ => escController.OnEscPressed();
    }

    private void Update()
    {
        // mouse
        movement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
        // sprint
        if (groundMovement.Sprint.ReadValue<float>() > 0.1f)
        {
            Debug.Log("sprint true");
            movement.OnSprintPressed(true);
        }
        else
        {
            Debug.Log("sprint false");
            movement.OnSprintPressed(false);
        }

        // shooting
        if (interaction.ShootSeries.ReadValue<float>() > 0.1f)
        {
            gunSystem.ReceiveInput(true);
        }
        else
        {
            gunSystem.ReceiveInput(false);
        }
        // aiming
        if (interaction.Aim.ReadValue<float>() > 0.1f)
        {
            gunSystem.ReceiveAimInput(true);
            mouseLook.ReceiveAimingBool(true);
        }
        else
        {
            mouseLook.ReceiveAimingBool(false);
            gunSystem.ReceiveAimInput(false);
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
