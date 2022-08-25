using UnityEngine;
using System.Collections.Generic;

public class RescueObjectiveManager : MonoBehaviour
{
    [SerializeField]
    private Transform statusContainer;
    [SerializeField]
    private GameObject statusUIPrefab;

    private List<RescueStatusUI> statusUIs = new List<RescueStatusUI>(10); //never going to be more than 10. might aswell. may be unused tho lets see.

    private NPCController[] npcs;

    private int rescuedNPCs = 0;
    private int deadNPCs = 0;

    private void Start()
    {
        //this is bad but im lazy. sue me.
        npcs = FindObjectsOfType<NPCController>();
        foreach (var n in npcs)
        {
            var go = Instantiate(statusUIPrefab, statusContainer);
            var comp = go.GetComponent<RescueStatusUI>();
            statusUIs.Add(comp);
            n.OnRescueStatusChanged += comp.UpdateImageByStatus;
            n.OnRescueStatusChanged += OnNPCStatusChange;
        }
    }

    private void OnNPCStatusChange(ERescueStatus status)
    {
        if (status == ERescueStatus.RescueFinal)
            rescuedNPCs++;
        if (status == ERescueStatus.NpcDied)
            deadNPCs++;

        if(rescuedNPCs + deadNPCs == npcs.Length)
        {
            throw new System.NotImplementedException("Complete Level");
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Friendly NPC"))
        {
            collider.GetComponent<NPCController>().CompleteRescue();
        }
    }
}
