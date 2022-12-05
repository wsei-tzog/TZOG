using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    #region reference
    public GunSystem gunSystem;
    public Torch torch;
    public letterFound lF;
    public photoFound pQ3;
    public Transform defaultPosition;
    public MouseLook mouseLook;
    #endregion

    #region variables
    public float dropForwardForce, dropUpwardForce;
    public bool equipped;
    #endregion

    private void Start()
    {
        //Setup
        if (!equipped)
        {
            gunSystem.enabled = false;
            mouseLook.slotFull = false;
            torch.torchEquipped = false;
        }
        if (equipped)
        {
            gunSystem.enabled = true;
            mouseLook.slotFull = true;
        }
    }

    #region input

    public void OnDropPressed()
    {
        if (equipped)
        {
            Drop();
        }
    }
    #endregion

    #region equip / drop


    public void PickUpStuff(GameObject rayHittedGameObject)
    {

        if (rayHittedGameObject.name == "letter")
        {
            lF.showLetter();
        }
        else if (rayHittedGameObject.name == "photoQ3")
        {
            pQ3.showpQ3();
        }
        else
        {
            // set this torch in input
            InputManager.torch = rayHittedGameObject.GetComponent<Torch>();
            torch.torchEquipped = true;

            //Remove rigidbody and BoxCollider
            Destroy(rayHittedGameObject.GetComponent<Rigidbody>());
            rayHittedGameObject.GetComponent<Collider>().enabled = false;
            rayHittedGameObject.GetComponentInChildren<Light>().enabled = true;

            //Make weapon a child and move it to default position
            Vector3 torchScale = rayHittedGameObject.transform.localScale;
            rayHittedGameObject.transform.SetParent(defaultPosition, false);
            rayHittedGameObject.transform.localPosition = Vector3.zero;
            rayHittedGameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            rayHittedGameObject.transform.localScale = torchScale;
            // show L for light
        }
    }
    public void PickUp(GameObject rayHittedGameObject)
    {


        equipped = true;
        mouseLook.slotFull = true;
        // MouseLook.isPickingUp = false;

        //Remove rigidbody and BoxCollider
        Destroy(rayHittedGameObject.GetComponent<Rigidbody>());
        rayHittedGameObject.GetComponent<Collider>().enabled = false;

        // set that weapon as weapon swing and gun system
        InputManager.gunSystem = rayHittedGameObject.GetComponent<GunSystem>();
        MouseLook.weaponSwing = rayHittedGameObject.GetComponent<WeaponSwing>();
        GunSystem.weaponIsActive = true;
        gunSystem.turnOffCanvas = false;
        gunSystem.UIBullets();

        rayHittedGameObject.GetComponent<GunSystem>().enabled = true;
        rayHittedGameObject.GetComponent<WeaponSwing>().enabled = true;

        //Make weapon a child and move it to default position
        Vector3 scale = rayHittedGameObject.transform.localScale;
        rayHittedGameObject.transform.SetParent(defaultPosition, false);
        rayHittedGameObject.transform.localPosition = Vector3.zero;
        rayHittedGameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        rayHittedGameObject.transform.localScale = scale;


    }

    private void Drop()
    {
        Debug.Log("dropinn");
        // isDropping = false;
        Vector3 scale = transform.localScale;
        equipped = false;
        mouseLook.slotFull = false;
        transform.SetParent(null);
        transform.localScale = scale;

        // Disable script
        gameObject.GetComponent<GunSystem>().enabled = false;
        gameObject.GetComponent<WeaponSwing>().enabled = false;
        InputManager.gunSystem = null;
        InputManager.pickUpController = null;
        MouseLook.weaponSwing = null;
        GunSystem.weaponIsActive = false;
        gunSystem.turnOffCanvas = true;
        gunSystem.UIBullets();

        // Make Rigidbody and BoxCollider
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        transform.gameObject.GetComponent<Collider>().isTrigger = false;
        transform.gameObject.GetComponent<Collider>().enabled = true;

        // AddForce
        rb.AddForce(mouseLook.playerCamera.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(mouseLook.playerCamera.up * dropUpwardForce, ForceMode.Impulse);
        // Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
    }
    #endregion

}