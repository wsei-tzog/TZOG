using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    #region reference
    public GunSystem gunSystem;
    public Transform player, defaultPosition, fpsCam;
    public RaycastHit rayHit;
    #endregion

    #region variables
    public float pickUpRange, dropForwardForce, dropUpwardForce;

    public static bool slotFull;
    public bool equipped, isPickingUp, isDropping;
    #endregion

    #region input
    public void OnPickUpPressed()
    {
        isPickingUp = true;
    }
    public void OnDropPressed()
    {
        isDropping = true;
    }
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

    private void Update()
    {
        //Check if player is in range and "E" is pressed || Q for drop
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, pickUpRange))
        {
            Debug.Log(rayHit.collider.name);
            // InputManager.pickUpController = gameObject.GetComponent<PickUpController>();
            InputManager.pickUpController = rayHit.transform.gameObject.GetComponent<PickUpController>();
            if (rayHit.transform.gameObject.CompareTag("Weapon") && isPickingUp)
            {
                PickUp(rayHit.transform.gameObject);
            }
        }
        if (equipped && isDropping)
        {
            Drop();
        }

    }

    private void PickUp(GameObject rayHittedGameObject)
    {
        isPickingUp = false;
        //Remove rigidbody and BoxCollider
        Destroy(rayHittedGameObject.GetComponent<Rigidbody>());
        rayHittedGameObject.GetComponent<Collider>().enabled = false;

        // set that weapon as weapon swing and gun system
        InputManager.gunSystem = rayHittedGameObject.GetComponent<GunSystem>();
        MouseLook.weaponSwing = rayHittedGameObject.GetComponent<WeaponSwing>();
        GunSystem.weaponIsActive = true;
        rayHittedGameObject.GetComponent<GunSystem>().enabled = true;

        //set bools
        equipped = true;
        slotFull = true;

        //Make weapon a child and move it to default position
        rayHittedGameObject.transform.SetParent(defaultPosition, false);
        rayHittedGameObject.transform.localPosition = Vector3.zero;
        rayHittedGameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        // rayHittedGameObject.transform.localScale = Vector3.one;

    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //Set parent to null
        transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal
        // gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        gameObject.GetComponent<Collider>().isTrigger = false;
        gameObject.GetComponent<Collider>().enabled = true;
        InputManager.gunSystem = null;
        MouseLook.weaponSwing = null;
        GunSystem.weaponIsActive = false;
        //Gun carries momentum of player
        // rb.velocity = gameObject.GetComponentInParent<>

        //AddForce
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        //Disable script
        gameObject.GetComponent<GunSystem>().enabled = false;
    }
}

