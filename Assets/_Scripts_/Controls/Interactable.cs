using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{


    [Header("Default settings")]
    public bool canItBePickedUp;
    public float range;
    public Transform objectHolder;
    public MouseLook mouseLook;
    public float dropForwardForce, dropUpwardForce;
    private Vector3 scale;
    public LayerMask groundMask;
    public AudioSource audioSource;
    public AudioClip[] hitSounds;

    [Header("Ammo settings")]

    public AmmoManager ammoManager;
    public bool isItAmmo;
    public AmmoType ammoType;
    public int amountOfAmmo;

    [Header("Destroy settings")]
    public bool isItDestrucable;
    public GameObject notDestroyed;
    public GameObject destroyed;
    public List<GameObject> cargo = new List<GameObject>();
    public float healt;


    [Header("Door settings")]
    public bool isItDoor;
    bool doorOpened;
    public int lockID;
    float duration = 1.5f;

    [System.Serializable]
    public struct DoorInfo
    {
        public Transform wing;
        public Quaternion openRotation;
        public BoxCollider boxCollider;
    }

    public List<DoorInfo> doorList;


    [Header("Key settings")]
    public bool isItKey;
    public QuestController questController;

    private void Start()
    {
        notDestroyed.SetActive(true);
        destroyed.SetActive(false);
        if (null != cargo)
            foreach (GameObject thing in cargo)
            {
                thing.SetActive(false);
            }

        if (this.gameObject.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.material.SetFloat("startClue", 1f);
    }


    public virtual void Interact()
    {


        if (canItBePickedUp)
        {
            GrabObjects();
        }

        if (isItKey)
        {
            CollectKey();
        }

        if (isItDoor)
        {
            if (!doorOpened)
            {
                TryOpen();
            }
            else
            {
                TryClose();
            }
        }

        if (isItAmmo)
        {
            CollectAmmo();
        }
    }


    private void GrabObjects()
    {
        // rg and coll
        Destroy(this.GetComponent<Rigidbody>());
        // this.GetComponent<Collider>().enabled = false;

        // init
        scale = this.transform.localScale;
        this.transform.SetParent(objectHolder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        mouseLook.objectSlotFull = true;

        mouseLook.grabbedObject = this.gameObject;
        mouseLook.gun.SetActive(false);


        // scale and rotation
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = scale;

    }
    public void Throw()
    {
        mouseLook.objectSlotFull = false;
        mouseLook.grabbedObject = null;
        // Vector3 scale = transform.localScale;
        transform.SetParent(null);
        transform.localScale = scale;

        gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        transform.gameObject.GetComponent<Collider>().isTrigger = false;
        transform.gameObject.GetComponent<Collider>().enabled = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.AddForce(mouseLook.playerCamera.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(mouseLook.playerCamera.up * dropUpwardForce, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // audioSource.pitch = Random.Range(0.85f, 1.3f);
        // audioSource.volume = Random.Range(0.8f, 1);
        // audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);

        Debug.Log("Hitting flore");
        if (isItDestrucable)
        {
            destroyObject(1);
        }

        // if (canItBePickedUp)
        // {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range);
        Debug.Log("spawning overlap sphere");

        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Debug.Log("Getting enemy " + enemy);
                enemy.GetComponent<NewEnemyAI>().CheckNoise(this.transform.position);
            }
        }
        // }
    }
    private void CollectKey()
    {
        Key key = this.transform.gameObject.GetComponent<Key>();
        if (key != null)
        {
            questController.keys.Add(key.keyID);
            // Destroy the key game object
            Destroy(key.gameObject);
        }

    }
    private void TryClose()
    {
        foreach (var door in doorList)
        {
            StartCoroutine(RotateOverTime(door.wing, Quaternion.identity, duration, door.boxCollider));
            doorOpened = false;
        }
    }
    private void TryOpen()
    {
        if (questController.keys.Contains(lockID))
        {
            foreach (var door in doorList)
            {
                StartCoroutine(RotateOverTime(door.wing, door.openRotation, duration, door.boxCollider));
            }
            doorOpened = true;
        }
        else
        {
            Debug.Log("No such key!");
        }
    }
    private IEnumerator RotateOverTime(Transform transformToRotate, Quaternion targetRotation, float duration, BoxCollider boxCollider)
    {
        // this.GetComponent<Collider>().enabled = false;

        var startRotation = transformToRotate.localRotation;

        var timePassed = 0f;
        while (timePassed < duration)
        {
            var factor = timePassed / duration;
            // optional add ease-in and -out
            // factor = Mathf.SmoothStep(0, 1, factor);
            factor = 1f - Mathf.Cos(factor * Mathf.PI * 0.7f);

            transformToRotate.localRotation = Quaternion.Lerp(startRotation, targetRotation, factor);
            // or
            //transformToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, factor);

            // increae by the time passed since last frame
            timePassed += Time.deltaTime;

            // important! This tells Unity to interrupt here, render this frame
            // and continue from here in the next frame
            yield return null;
        }

        // remove collider if needed
        if (null != boxCollider)
        { boxCollider.enabled = false; }

        // to be sure to end with exact values set the target rotation fix when done
        transformToRotate.localRotation = targetRotation;

    }
    private void CollectAmmo()
    {
        Destroy(this.gameObject);
        ammoManager.AddAmmo(ammoType, amountOfAmmo);
    }

    // Destroy 
    public void destroyObject(int damange)
    {
        healt -= damange;

        if (healt <= 0)
        {
            switchModel();
        }
    }
    public void switchModel()
    {
        Transform notDestroyedTransform = notDestroyed.transform;
        notDestroyed.SetActive(false);
        destroyed.transform.SetParent(null);

        destroyed.transform.position = notDestroyedTransform.position;
        destroyed.SetActive(true);
        destroyed.AddComponent<Rigidbody>();
        Rigidbody drb = destroyed.GetComponent<Rigidbody>();
        drb.AddForce(Vector3.up * 1, ForceMode.Impulse);
        destroyed.transform.gameObject.GetComponent<Renderer>().material.SetFloat("startClue", 1f);
        destroyed.transform.gameObject.GetComponent<Collider>().isTrigger = false;
        destroyed.transform.gameObject.GetComponent<Collider>().enabled = true;

        if (null != cargo)
            foreach (GameObject thing in cargo)
            {
                thing.transform.SetParent(null);
                thing.transform.position = notDestroyedTransform.position;

                thing.AddComponent<Rigidbody>();
                Rigidbody rb = thing.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                thing.transform.gameObject.GetComponent<Renderer>().material.SetFloat("startClue", 1f);
                thing.transform.gameObject.GetComponent<Collider>().isTrigger = false;
                thing.transform.gameObject.GetComponent<Collider>().enabled = true;

                thing.SetActive(true);
                rb.AddForce(Vector3.up * 3, ForceMode.Impulse);
            }
        Destroy(this);
    }
















}
