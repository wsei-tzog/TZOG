using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    #region reference
    public GunSystem gunSystem;
    public Transform defaultPosition;
    public GameObject rayHit;
    public MouseLook mouseLook;
    #endregion

    #region variables
    public float pickUpRange, dropForwardForce, dropUpwardForce;
    public static bool slotFull;
    public static bool isPickingUp, equipped, isDropping;
    #endregion

    private void Start()
    {
        //Setup
        if (!equipped)
        {
            gunSystem.enabled = false;
        }
        if (equipped)
        {
            gunSystem.enabled = true;
            slotFull = true;
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
    public void PickUp(GameObject rayHittedGameObject)
    {
        //set bools
        equipped = true;
        slotFull = true;
        MouseLook.isPickingUp = false;

        //Remove rigidbody and BoxCollider
        Destroy(rayHittedGameObject.GetComponent<Rigidbody>());
        rayHittedGameObject.GetComponent<Collider>().enabled = false;

        // set that weapon as weapon swing and gun system
        InputManager.gunSystem = rayHittedGameObject.GetComponent<GunSystem>();
        MouseLook.weaponSwing = rayHittedGameObject.GetComponent<WeaponSwing>();
        GunSystem.weaponIsActive = true;
        rayHittedGameObject.GetComponent<GunSystem>().enabled = true;

        //Make weapon a child and move it to default position
        rayHittedGameObject.transform.SetParent(defaultPosition, false);
        rayHittedGameObject.transform.localPosition = Vector3.zero;
        rayHittedGameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        rayHittedGameObject.transform.localScale = new Vector3(0.265f, 0.315f, 0.256f);
    }

    private void Drop()
    {
        Debug.Log("dropinn");
        // isDropping = false;
        equipped = false;
        slotFull = false;

        // Set parent to null
        transform.SetParent(null);

        // Disable script
        gameObject.GetComponent<GunSystem>().enabled = false;

        // Make Rigidbody and BoxCollider
        gameObject.AddComponent<Rigidbody>();
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        transform.gameObject.GetComponent<Collider>().isTrigger = false;
        transform.gameObject.GetComponent<Collider>().enabled = true;

        InputManager.gunSystem = null;
        MouseLook.weaponSwing = null;
        GunSystem.turnOffCanvas = true;
        GunSystem.weaponIsActive = false;

        // AddForce
        rb.AddForce(mouseLook.playerCamera.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(mouseLook.playerCamera.up * dropUpwardForce, ForceMode.Impulse);
        // Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
        InputManager.pickUpController = null;
    }
    #endregion

}