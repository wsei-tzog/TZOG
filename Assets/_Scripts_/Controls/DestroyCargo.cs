using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCargo : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeToDestroy;
    public float timeFactorToDestroy = 40;
    public Transform holder;
    void Start()
    {
        holder = GetComponent<Interactable>().objectHolder;
        timeToDestroy = Time.time + timeFactorToDestroy;
    }

    void Update()
    {
        if (transform.parent != holder)
        {
            if (Time.time >= timeToDestroy)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            timeToDestroy = Time.time + timeFactorToDestroy;
        }
    }
}
