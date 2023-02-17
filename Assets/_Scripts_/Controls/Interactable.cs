using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    private ClueImageManager clueImageManager;


    [Header("Sound")]
    #region 
    public AudioSource audioSource;
    public SoundManager soundManager;
    public SoundType soundType;
    #endregion


    [Header("PickedUp settings")]
    #region 
    public MouseLook mouseLook;
    public bool canItBePickedUp;
    public float range;
    public Transform objectHolder;
    public float dropForwardForce, dropUpwardForce;
    private Vector3 scale;
    // public LayerMask groundMask;
    #endregion


    [Header("Ammo settings")]
    #region 

    public AmmoManager ammoManager;
    public bool isItAmmo;
    public AmmoType ammoType;
    public int amountOfAmmo;

    #endregion


    [Header("Destroy settings")]
    #region 
    public bool isItDestrucable;
    public GameObject notDestroyed;
    public List<GameObject> destroyed = new List<GameObject>();
    public List<GameObject> cargo = new List<GameObject>();
    public float healt;

    #endregion


    [Header("Door settings")]
    #region 
    public YouDontHaveKey youDontHaveKey;
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

    #endregion


    [Header("Key settings")]
    #region 
    public bool isItKey;
    public QuestController questController;
    #endregion


    [Header("Lever settings")]
    #region 
    public bool isItLever;
    bool leverSwitched;

    [System.Serializable]
    public struct LeverInfo
    {
        public Transform lever;
        public Quaternion openRotation;
    }
    public List<LeverInfo> LeverList;
    public List<GameObject> LightList;
    public List<GameObject> LightOffList;

    #endregion


    [Header("Bateerry settings")]
    #region 
    public float rechargeAmount = 20f;

    public GameObject torch;
    public bool isItBattery;
    #endregion

    [Header("Medical pack settings")]
    #region 
    public bool isItMedicalPack;
    public int amoutOfPack;

    #endregion



    private void Awake()
    {
        youDontHaveKey = FindObjectOfType<YouDontHaveKey>();

        soundManager = FindObjectOfType<SoundManager>();
        ammoManager = FindObjectOfType<AmmoManager>();
        mouseLook = FindObjectOfType<MouseLook>();
        audioSource = GetComponent<AudioSource>();
        questController = FindObjectOfType<QuestController>();


        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Set the playOnAwake property to false
        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        clueImageManager = GameObject.FindObjectOfType<ClueImageManager>();

        #region light

        foreach (var light in LightList)
        {
            light.SetActive(false);
        }

        foreach (var light in LightOffList)
        {
            light.SetActive(true);
        }
        #endregion

        #region destroyed
        if (!isItDestrucable)
        {
            notDestroyed = null;
            destroyed = null;
        }

        if (null != notDestroyed)
        {
            notDestroyed.SetActive(true);
        }
        else
            notDestroyed = null;

        if (destroyed != null)
            foreach (GameObject thing in destroyed)
            {
                thing.SetActive(false);
            }
        else
        {
            destroyed = null;
        }

        if (null != cargo)
            foreach (GameObject thing in cargo)
            {
                thing.SetActive(false);
            }
        else
            cargo = null;
        #endregion

        #region clue
        if (this.gameObject.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.material.SetFloat("startClue", 1f);
        #endregion
    }


    public virtual void Interact()
    {
        switch (true)
        {
            case bool x when canItBePickedUp && x:
                GrabObjects();
                break;
            case bool x when isItKey && x:
                CollectKey();
                break;
            case bool x when isItDoor && x:
                if (!doorOpened)
                {
                    TryOpen();
                }
                else
                {
                    TryClose();
                }
                break;
            case bool x when isItAmmo && x:
                CollectAmmo();
                break;
            case bool x when isItLever && x:
                if (!leverSwitched)
                {
                    TurnOnLever();
                }
                else
                {
                    TurnOffLever();
                }
                break;
            case bool x when isItBattery && x:
                RechargeTorch();
                break;
            case bool x when isItMedicalPack && x:
                CollectMedicalpack();
                break;
        }
    }

    private void GrabObjects()
    {

        // rg and coll
        Destroy(this.GetComponent<Rigidbody>());
        this.GetComponent<Collider>().enabled = false;

        // init
        scale = this.transform.localScale;
        this.transform.SetParent(objectHolder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        mouseLook.objectSlotFull = true;

        mouseLook.grabbedObject = this.gameObject;
        if (mouseLook.gun != null)
        {
            mouseLook.gun.SetActive(false);

        }


        // scale and rotation
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = scale;

    }
    public void Throw()
    {
        if (mouseLook.gun != null)
        {
            mouseLook.gun.SetActive(true);

        }

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
        rb.AddForce(mouseLook.playerCamera.forward * 5, ForceMode.Impulse);
        rb.AddForce(mouseLook.playerCamera.up * 3, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CompareTag("Enemy"))
        {
            NewEnemyAI newEnemyAI = collision.collider.gameObject.GetComponent<NewEnemyAI>();
            if (newEnemyAI != null)
            {
                newEnemyAI.TakeDamage(1);
            }
        }

        if (isItDestrucable)
        {
            destroyObject(1);
        }

        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range);

        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy != null && enemy.CompareTag("Enemy"))
            {
                NewEnemyAI enemyAI = enemy.GetComponent<NewEnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.CheckNoise(this.transform.position);
                }
            }
        }
    }
    private void CollectMedicalpack()
    {
        NPCScript.medicalPackCount += amoutOfPack;
        Destroy(gameObject);
    }
    private void CollectKey()
    {
        Key key = this.transform.gameObject.GetComponent<Key>();
        if (key != null)
        {
            questController.keys.Add(key.keyID);
            // Destroy the key game object
            Destroy(key.gameObject);

            PlaySound();
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
            List<AudioClip> soundList = soundManager.GetSoundList(SoundType.DoorsClosed);

            if (soundList != null)
            {
                AudioClip clip = soundList[Random.Range(0, soundList.Count)];
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.PlayOneShot(clip);
            }
            youDontHaveKey.StartCoroutine("YouDontHaveKeyDisplay");
        }
    }
    private void CollectAmmo()
    {

        PlaySound();
        Destroy(this.gameObject);
        ammoManager.AddAmmo(ammoType, amountOfAmmo);
    }
    private void TurnOnLever()
    {

        foreach (var l in LeverList)
        {
            StartCoroutine(RotateOverTime(l.lever, l.openRotation, duration, null));
            leverSwitched = true;
        }

        foreach (var light in LightList)
        {
            light.SetActive(true);
        }
        foreach (var light in LightOffList)
        {
            light.SetActive(false);
        }
    }
    private void TurnOffLever()
    {

        foreach (var l in LeverList)
        {
            StartCoroutine(RotateOverTime(l.lever, Quaternion.identity, duration, null));
            leverSwitched = false;
        }

        foreach (var light in LightList)
        {
            light.SetActive(false);
        }

        foreach (var light in LightOffList)
        {
            light.SetActive(true);
        }
    }
    private void RechargeTorch()
    {
        torch.GetComponent<Torch>().Recharge(rechargeAmount);
        Destroy(gameObject);
    }
    private IEnumerator RotateOverTime(Transform transformToRotate, Quaternion targetRotation, float duration, BoxCollider boxCollider)
    {
        // this.GetComponent<Collider>().enabled = false;
        PlaySound();

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


    // Destroy 
    public void destroyObject(int damange)
    {
        healt -= damange;

        PlaySound();

        if (healt <= 0)
        {
            switchModel();
        }
    }
    public void switchModel()
    {
        if (notDestroyed != null)
        {
            PlaySound();

            Transform notDestroyedTransform = notDestroyed.transform;
            notDestroyed.SetActive(false);
            this.transform.gameObject.GetComponent<Collider>().enabled = false;

            foreach (GameObject destroyedThing in destroyed)
            {
                destroyedThing.transform.SetParent(null);

                // destroyedThing.transform.position = notDestroyedTransform.position;
                destroyedThing.SetActive(true);
                destroyedThing.AddComponent<Rigidbody>();
                destroyedThing.AddComponent<DestroyCargo>();
                Rigidbody drb = destroyedThing.GetComponent<Rigidbody>();
                drb.AddForce(Vector3.up * 0.2f, ForceMode.Impulse);
                destroyedThing.transform.gameObject.GetComponent<Renderer>().material.SetFloat("startClue", 1f);
                // destroyedThing.transform.gameObject.GetComponent<Collider>().isTrigger = false;
                // destroyedThing.transform.gameObject.GetComponent<Collider>().enabled = true;
            }

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
    private void OnDestroy()
    {
        if (clueImageManager != null)
        {
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                clueImageManager.RemoveDisplayImage(collider);
            }
        }
    }

    // Sound
    private void PlaySound()
    {
        soundManager.PlaySound(audioSource, soundType);
        // audioSource.enabled = true;

        // List<AudioClip> soundList = soundManager.GetSoundList(soundType);

        // if (soundList != null)
        // {
        //     AudioClip clip = soundList[Random.Range(0, soundList.Count)];
        //     audioSource.clip = clip;

        //     audioSource.pitch = Random.Range(0.85f, 1.3f);
        //     audioSource.volume = Random.Range(0.8f, 1);
        //     audioSource.PlayOneShot(clip);
        // }
        // else
        // {
        //     Debug.LogError("Sound type not found: " + soundType);
        // }

    }



}