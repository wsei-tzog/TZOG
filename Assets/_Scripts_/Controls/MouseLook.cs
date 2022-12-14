using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    #region reference
    public RaycastHit rayHit;
    public static WeaponSwing weaponSwing;
    public static PickUpController pickUpController;
    public static bool slotFull;
    public QuestController questController;
    public Transform playerCamera;
    public letterFound lF;
    public photoFound pQ2;
    public q3Found pQ3;
    #endregion

    #region vars
    public float sensitivityX = 5f;
    public float sensitivityY = 1f;
    float mouseX, mouseY;
    public float xCamlp = 85f;
    float xRotation = 0;
    public static bool isPickingUp;
    #endregion


    public void ReceiveAimingBool(bool _isAiming)
    {
        weaponSwing.isAiming = _isAiming;
    }
    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
        weaponSwing.ReceiveInput(mouseInput);



    }
    public void OnPickUpPressed()
    {
        isPickingUp = true;

        if (lF.letterUI.enabled == true)
        {
            Debug.Log("true enabled");
            lF.hideLetter();
            isPickingUp = false;
        }
        else if (pQ2.photoUI.enabled == true)
        {
            pQ2.hidepQ2();
            isPickingUp = false;
        }
        else if (pQ3.q3UI.enabled == true)
        {
            pQ3.hideq3();
            isPickingUp = false;
        }
        else
        {
            #region raycast
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out rayHit, 80);
            {

                if (isPickingUp)
                {
                    if (rayHit.transform.gameObject.CompareTag("Weapon") && !slotFull)
                    {
                        // turn off shader
                        rayHit.transform.gameObject.GetComponent<Renderer>().material.SetFloat("_startClue", 0f);

                        slotFull = true;
                        pickUpController = rayHit.transform.gameObject.GetComponent<PickUpController>();

                        pickUpController.PickUp(rayHit.transform.gameObject);
                        InputManager.pickUpController = pickUpController;
                    }
                    else if (rayHit.transform.gameObject.CompareTag("Weapon") && slotFull)
                    {
                        Debug.Log("Cannot pickup that weapon!");
                    }
                    else if (rayHit.transform.gameObject.CompareTag("Quest"))
                    {
                        rayHit.transform.gameObject.GetComponent<Renderer>().material.SetFloat("_startClue", 0f);
                        pickUpController = rayHit.transform.gameObject.GetComponent<PickUpController>();
                        pickUpController.PickUpStuff(rayHit.transform.gameObject);

                        // questController.takeNewMission(rayHit.transform.gameObject.name);
                    }
                    else if (rayHit.transform.gameObject.CompareTag("Torch"))
                    {
                        rayHit.transform.gameObject.GetComponent<Renderer>().material.SetFloat("_startClue", 0f);

                        pickUpController = rayHit.transform.gameObject.GetComponent<PickUpController>();
                        pickUpController.PickUpTorch(rayHit.transform.gameObject);

                    }
                    isPickingUp = false;
                }

            }
            #endregion
        }
    }
    private void Awake()
    {
    }

    void Update()
    {
        #region MouseLook
        transform.Rotate(Vector3.up, Mathf.Clamp(mouseX, -450, 450) * Time.deltaTime);
        xRotation -= mouseY / 2;
        xRotation = Mathf.Clamp(xRotation, -xCamlp, xCamlp);

        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
        #endregion
    }
}
