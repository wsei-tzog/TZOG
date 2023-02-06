using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public QuestController questController;

    [Header("Door settings")]
    public bool isItDoor;
    bool doorOpened;
    public int lockID;
    // public Quaternion targetRotation;
    float duration = 1.5f;
    // public Quaternion closeRotation;
    // public List<Transform> wings = new List<Transform>();
    // public Quaternion openRotation1Wing;
    // public Quaternion openRotation2Wings;

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



    public virtual void Interact()
    {
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
    }


    private void TryClose()
    {
        foreach (var door in doorList)
        {
            StartCoroutine(RotateOverTime(door.wing, Quaternion.identity, duration, door.boxCollider));
            doorOpened = false;
            Debug.Log("Closing");
        }
        Debug.Log("Is it closed?");
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

















}
