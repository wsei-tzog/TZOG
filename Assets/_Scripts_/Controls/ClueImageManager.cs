using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueImageManager : MonoBehaviour
{

    public GameObject displayInteractImagePrefab;
    public GameObject stunEnemy;
    public GameObject NPCSister;
    public GameObject NPCside;
    public float offset = 0.5f;

    private Dictionary<Collider, GameObject> imageMap = new Dictionary<Collider, GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            TryGetComponent<Renderer>(out Renderer renderer);
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
                    GameObject displayImage = Instantiate(NPCSister);
                    displayImage.tag = "NPCsisterTag";
                    Vector3 imagePosition = bounds.center;
                    imageMap[other] = displayImage;
                }
            }
            else if (other.gameObject.CompareTag("NPCside"))
            {
                if (!imageMap.ContainsKey(other) && other.gameObject.activeInHierarchy)
                {
                    GameObject displayImage = Instantiate(NPCside);
                    Vector3 imagePosition = bounds.center;
                    imageMap[other] = displayImage;
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
    private void Update()
    {
        Dictionary<Collider, GameObject> imageMapCopy = imageMap;

        foreach (var entry in imageMapCopy)
        {
            Collider other = entry.Key;
            GameObject displayImage = entry.Value;

            if (entry.Value.CompareTag("enemytag"))
            {

                Bounds bounds = other.gameObject.GetComponent<Renderer>().bounds;
                Vector3 imagePosition = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
                Vector3 imageOffset = new Vector3(0, enemyOffsetY, 0);
                imageMap[other] = displayImage;
                imagePosition += offset * (Camera.main.transform.position - imagePosition).normalized;
                displayImage.transform.position = imagePosition + imageOffset;
                displayImage.transform.LookAt(Camera.main.transform.position);
            }
            else
            {
                Bounds bounds = other.gameObject.GetComponent<Renderer>().bounds;
                Vector3 imagePosition = bounds.center;
                imagePosition += offset * (Camera.main.transform.position - imagePosition).normalized;
                displayImage.transform.position = imagePosition;
                displayImage.transform.LookAt(Camera.main.transform.position);
            }


            // Calculate the distance between the display image and the camera
            float distance = Vector3.Distance(displayImage.transform.position, Camera.main.transform.position);
            if (entry.Value.CompareTag("NPCsisterTag"))
            {
                // Calculate a scale factor based on the distance
                float scaleFactor = Mathf.Clamp(2f / distance, 1f, 1f); // Clamp to avoid the image being too small or too large
                // Apply the scale factor to the display image
                displayImage.transform.localScale = Vector3.one * scaleFactor;
            }
            else
            {
                float scaleFactor = Mathf.Clamp(2f / distance, npcMin, npcpMax); // Clamp to avoid the image being too small or too large
                // Apply the scale factor to the display image
                displayImage.transform.localScale = Vector3.one * scaleFactor;
            }

        }

    }
}
