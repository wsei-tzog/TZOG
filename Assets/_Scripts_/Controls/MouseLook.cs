using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    #region reference
    public RaycastHit rayHit;
    public static WeaponSwing weaponSwing;
    public static PickUpController pickUpController;
    public Transform playerCamera;
    public letterFound lF;
    public photoFound pQ3;
    #endregion

    #region vars
    public float sensitivityX = 5f;
    public float sensitivityY = 1f;
    float mouseX, mouseY;
    public float xCamlp = 85f;
    float xRotation = 0;
    public static bool isPickingUp;
    public bool slotFull;
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

        // call disabling methods on UI on mouse movement
        if (lF.letterUI.enabled)
        {
            lF.hideLetter();
        }

        if (pQ3.photoUI.enabled)
        {
            pQ3.hidepQ3();
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



        #region raycast
        Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out rayHit, 80);
        {
            if (rayHit.transform.gameObject.CompareTag("Clue") || rayHit.transform.gameObject.CompareTag("PickAble"))
            {
                rayHit.transform.gameObject.GetComponent<Renderer>().material.SetFloat("_startClue", 1f);
            }


            if (isPickingUp)
            {
                if (rayHit.transform.gameObject.CompareTag("Weapon") && !slotFull)
                {
                    rayHit.transform.gameObject.GetComponent<Renderer>().material.SetFloat("_startClue", 0f);
                    Debug.Log(rayHit.transform.gameObject.name + " Weapon");
                    isPickingUp = false;

                    pickUpController = rayHit.transform.gameObject.GetComponent<PickUpController>();

                    pickUpController.PickUp(rayHit.transform.gameObject);
                    InputManager.pickUpController = pickUpController;
                }

                else if (rayHit.transform.gameObject.CompareTag("Weapon") && slotFull)
                {
                    isPickingUp = false;
                    Debug.Log("Cannot pickup that weapon!");
                }

                else if (rayHit.transform.gameObject.CompareTag("PickAble"))
                {
                    rayHit.transform.gameObject.GetComponent<Renderer>().material.SetFloat("_startClue", 0f);
                    Debug.Log(rayHit.transform.gameObject.name + " Pickable");
                    isPickingUp = false;

                    pickUpController = rayHit.transform.gameObject.GetComponent<PickUpController>();
                    pickUpController.PickUpStuff(rayHit.transform.gameObject);

                    // InputManager.pickUpController = pickUpController;

                }
                else
                {
                    isPickingUp = false;
                }

            }

        }
        #endregion
    }
}
