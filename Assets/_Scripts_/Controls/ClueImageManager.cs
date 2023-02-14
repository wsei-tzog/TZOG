using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueImageManager : MonoBehaviour
{

    public GameObject displayInteractImagePrefab;
    public GameObject stunEnemy;
    public float offset = 1f;

    private Dictionary<Collider, GameObject> imageMap = new Dictionary<Collider, GameObject>();

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Interactable"))
        {
            if (!imageMap.ContainsKey(other) && other.gameObject.activeInHierarchy)
            {

                GameObject displayImage = Instantiate(displayInteractImagePrefab);
                Bounds bounds = other.gameObject.GetComponent<Renderer>().bounds;
                Vector3 imagePosition = bounds.center;
                imageMap[other] = displayImage;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (imageMap.ContainsKey(other))
        {
            GameObject displayImage = imageMap[other];
            Destroy(displayImage);
            imageMap.Remove(other);
        }
    }

    public void RemoveDisplayImage(Collider other)
    {
        if (imageMap.ContainsKey(other))
        {
            GameObject displayImage = imageMap[other];
            Destroy(displayImage);
            imageMap.Remove(other);
        }
    }




    private void Update()
    {
        foreach (var entry in imageMap)
        {
            Collider other = entry.Key;
            GameObject displayImage = entry.Value;
            Bounds bounds = other.gameObject.GetComponent<Renderer>().bounds;
            Vector3 imagePosition = bounds.center;
            imagePosition += offset * (Camera.main.transform.position - imagePosition).normalized;
            displayImage.transform.position = imagePosition;
            displayImage.transform.LookAt(Camera.main.transform.position);
            displayImage.transform.rotation = Quaternion.LookRotation(displayImage.transform.position - Camera.main.transform.position);
        }

    }
}
