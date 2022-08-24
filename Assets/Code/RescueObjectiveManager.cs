using UnityEngine;
using System.Collections.Generic;

public class RescueObjectiveManager : MonoBehaviour
{
    [SerializeField]
    private Transform statusContainer;
    [SerializeField]
    private GameObject statusUIPrefab;

    private List<RescueStatusUI> statusUIs = new List<RescueStatusUI>(10); //never going to be more than 10. might aswell. may be unused tho lets see.

    private void Start()
    {
        //this is bad but im lazy. sue me.
        var npcs = FindObjectsOfType<NPCController>();
        foreach (var n in npcs)
        {
            var go = Instantiate(statusUIPrefab, statusContainer);
            var comp = go.GetComponent<RescueStatusUI>();
            statusUIs.Add(comp);
            n.OnRescueStatusChanged += comp.UpdateImageByStatus;
        }
    }
}
