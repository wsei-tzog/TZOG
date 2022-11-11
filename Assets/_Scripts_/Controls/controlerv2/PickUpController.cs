using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public GunSystem gunSystem;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, defaultPosition, fpsCam;
    public RaycastHit rayHit;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Start()
    {
        //Setup
        if (!equipped)
        {
            gunSystem.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            gunSystem.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        //Check if player is in range and "E" is pressed
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, pickUpRange))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag("Weapon") && Input.GetKeyDown(KeyCode.E) && !slotFull)
            {
                PickUp();
            }
        }
        if (equipped && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }


        // Vector3 distanceToPlayer = player.position - transform.position;
        // if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) ;

    }

    private void PickUp()
    {
        // set that weapon as weapon swing and gun system

        InputManager.gunSystem = gameObject.GetComponent<GunSystem>();
        MouseLook.weaponSwing = gameObject.GetComponent<WeaponSwing>();
        GunSystem.weaponIsActive = true;
        // var inputmanager = parent.GetComponent<InputManager>();
        // var inputManager = GetComponentInParent<InputManager>();
        // var mouseLook = GetComponentInParent<MouseLook>();

        equipped = true;
        slotFull = true;

        //Make weapon a child of the camera and move it to default position
        transform.SetParent(defaultPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enable script
        gunSystem.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //Set parent to null
        transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Gun carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //AddForce
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        //Disable script
        gunSystem.enabled = false;
    }
}

