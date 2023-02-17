using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NPCScript : MonoBehaviour
{
    public GameObject player;

    public static int medicalPackCount = 0;

    public int medicalPackRequirement;

    public int keyID = 10;
    public static int npcHelped;

    public bool hasDeliveredMedicalPacks = false;
    public bool alradyDelivered = false;
    public QuestController questController;
    public BossKarma bossKarma;

    private void Start()
    {
        bossKarma = FindObjectOfType<BossKarma>();
        questController = FindObjectOfType<QuestController>();
        npcHelped = 0;
    }

    // Function to check if the player is close to the NPC and has delivered the required medical packs
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == player && !hasDeliveredMedicalPacks && !alradyDelivered)
        {
            int medicalPacksToDeliver = Mathf.Min(medicalPackRequirement, medicalPackCount);

            if (medicalPacksToDeliver > 0)
            {
                medicalPackCount -= medicalPacksToDeliver;
                medicalPackRequirement -= medicalPacksToDeliver;
                CheckIfDeliveredMedicalPacks();
            }
        }
    }

    private void CheckIfDeliveredMedicalPacks()
    {
        if (medicalPackRequirement <= 0)
        {
            Debug.Log("Player has delivered to sister medical packs.");
            hasDeliveredMedicalPacks = true;
            alradyDelivered = true;
            medicalPackRequirement = 0;
            questController.keys.Add(keyID);
            Debug.Log(questController.keys);
            npcHelped += 1;
            bossKarma.useNPCKarma();

        }
    }
}

