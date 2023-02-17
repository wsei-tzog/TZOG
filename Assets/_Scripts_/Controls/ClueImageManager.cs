using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueImageManager : MonoBehaviour
{

    public GameObject displayInteractImagePrefab;
    public GameObject stunEnemy;
    public GameObject NPCSister;
    public GameObject NPCSister2;
    public GameObject NPCside;
    public GameObject NPCside2;
    public float offset = 0.5f;

    private Dictionary<Collider, GameObject> imageMap = new Dictionary<Collider, GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<Renderer>(out Renderer renderer);
        Bounds bounds = renderer.bounds;

        if (other.gameObject.CompareTag("Interactable"))
        {
            if (!imageMap.ContainsKey(other) && other.gameObject.activeInHierarchy)
            {
                GameObject displayImage = Instantiate(displayInteractImagePrefab);
                Vector3 imagePosition = bounds.center;
                imageMap[other] = displayImage;
            }
        }
        else if (other.gameObject.CompareTag("NPCsister"))
        {

            if (!imageMap.ContainsKey(other) && other.gameObject.activeInHierarchy)
            {
                if (!other.gameObject.GetComponent<NPCScript>().hasDeliveredMedicalPacks)
                {
                    GameObject displayImage = Instantiate(NPCSister);
                    displayImage.tag = "NPCsisterTag";
                    Vector3 imagePosition = bounds.center;
                    imageMap[other] = displayImage;
                }
                else
                {
                    GameObject displayImage = Instantiate(NPCSister2);
                    displayImage.tag = "NPCsisterTag";
                    Vector3 imagePosition = bounds.center;
                    imageMap[other] = displayImage;
                }


            }
        }
        else if (other.gameObject.CompareTag("NPCside"))
        {
            if (!imageMap.ContainsKey(other) && other.gameObject.activeInHierarchy)
            {
                if (!other.gameObject.GetComponent<NPCScript>().hasDeliveredMedicalPacks)
                {
                    GameObject displayImage = Instantiate(NPCside);
                    displayImage.tag = "NPCsideTag";
                    Vector3 imagePosition = bounds.center;
                    imageMap[other] = displayImage;
                }
                else
                {
                    GameObject displayImage = Instantiate(NPCside2);
                    displayImage.tag = "NPCsideTag";
                    Vector3 imagePosition = bounds.center;
                    imageMap[other] = displayImage;
                }
            }
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {

            bool isEnemyAleret = other.GetComponentInParent<NewEnemyAI>().Alerted;
            bool isEnemyAlive = other.GetComponentInParent<NewEnemyAI>().Alive;

            if (!imageMap.ContainsKey(other) && other.gameObject.activeInHierarchy && !isEnemyAleret && isEnemyAlive)
            {
                GameObject displayImage = Instantiate(stunEnemy);
                displayImage.tag = "enemytag";
                Vector3 imagePosition = bounds.max;
                imageMap[other] = displayImage;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (imageMap.ContainsKey(other))
        {
            if (other.gameObject.CompareTag("Interactable") ||
                other.gameObject.CompareTag("NPCsister") ||
                other.gameObject.CompareTag("NPCside") ||
                other.gameObject.CompareTag("Enemy"))
            {
                GameObject displayImage = imageMap[other];
                Destroy(displayImage);
                imageMap.Remove(other);
            }
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



    public float clampMin = 0.2f;
    public float clampMax = 1.0f;
    public float npcMin = 0.2f;
    public float npcpMax = 1.0f;
    public float enemyOffsetY = 0.5f;
    public float npcOffsetY = 0.5f;
    public float npcOffsetX = 0.5f;
    public float npcOffsetZ = 0.5f;
    private void Update()
    {
        // Dictionary<Collider, GameObject> imageMapCopy = imageMap;
        Dictionary<Collider, GameObject> imageMapCopy = new Dictionary<Collider, GameObject>(imageMap);


        foreach (var entry in imageMapCopy)
        {
            Collider other = entry.Key;
            GameObject displayImage = entry.Value;

            // Calculate the distance between the display image and the camera
            float distance = Vector3.Distance(displayImage.transform.position, Camera.main.transform.position);
            if (entry.Value.CompareTag("NPCsisterTag"))
            {
                // Calculate a scale factor based on the distance
                float scaleFactor = Mathf.Clamp(1f / distance, npcMin, npcpMax); // Clamp to avoid the image being too small or too large
                // Apply the scale factor to the display image
                displayImage.transform.localScale = Vector3.one * scaleFactor;
            }
            else if (entry.Value.CompareTag("NPCsideTag"))
            {
                // Calculate a scale factor based on the distance
                float scaleFactor = Mathf.Clamp(1f / distance, npcMin, npcpMax); // Clamp to avoid the image being too small or too large
                // Apply the scale factor to the display image
                displayImage.transform.localScale = Vector3.one * scaleFactor;
            }
            else
            {
                float scaleFactor = Mathf.Clamp(1f / distance, clampMin, clampMax);
                // Apply the scale factor to the display image
                displayImage.transform.localScale = Vector3.one * scaleFactor;
            }

            if (entry.Value.CompareTag("enemytag"))
            {

                Bounds bounds = other.gameObject.GetComponent<Renderer>().bounds;
                Vector3 imagePosition = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
                Vector3 imageOffset = new Vector3(0, enemyOffsetY, 0);
                imagePosition += offset * (Camera.main.transform.position - imagePosition).normalized;
                displayImage.transform.position = imagePosition + imageOffset;
                displayImage.transform.LookAt(Camera.main.transform.position);
                displayImage.tag = "enemytag";
                imageMap[other] = displayImage;
            }
            else if (entry.Value.CompareTag("NPCsisterTag"))
            {

                Bounds bounds = other.gameObject.GetComponent<Renderer>().bounds;
                Vector3 imagePosition = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
                Vector3 imageOffset = new Vector3(npcOffsetX, npcOffsetY, npcOffsetZ);
                imagePosition += offset * (Camera.main.transform.position - imagePosition).normalized;
                displayImage.transform.position = imagePosition + imageOffset;
                displayImage.transform.LookAt(Camera.main.transform.position);
                displayImage.tag = "NPCsisterTag";
                imageMap[other] = displayImage;
            }
            else if (entry.Value.CompareTag("NPCsideTag"))
            {

                Bounds bounds = other.gameObject.GetComponent<Renderer>().bounds;
                Vector3 imagePosition = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
                Vector3 imageOffset = new Vector3(npcOffsetX, npcOffsetY, npcOffsetZ);
                imagePosition += offset * (Camera.main.transform.position - imagePosition).normalized;
                displayImage.transform.position = imagePosition + imageOffset;
                displayImage.transform.LookAt(Camera.main.transform.position);
                displayImage.tag = "NPCsideTag";
                imageMap[other] = displayImage;
            }
            else
            {
                Debug.Log("Displaying " + entry.Value);
                Bounds bounds = other.gameObject.GetComponent<Renderer>().bounds;
                Vector3 imagePosition = bounds.center;
                imagePosition += offset * (Camera.main.transform.position - imagePosition).normalized;
                displayImage.transform.position = imagePosition;
                displayImage.transform.LookAt(Camera.main.transform.position);
            }





        }

    }
}
