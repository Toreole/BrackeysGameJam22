using UnityEngine;
using System.Collections.Generic;

public class RescueObjectiveManager : MonoBehaviour
{
    [SerializeField] //force recompile
    private Transform statusContainer;
    [SerializeField]
    private GameObject statusUIPrefab;
    [SerializeField]
    private GameObject npcMarkerPrefab;
    [SerializeField]
    private GameObject zoneMarkerPrefab;
    [SerializeField]
    private Transform canvas;
    [SerializeField]
    private new Camera camera;

    private List<RescueStatusUI> statusUIs = new List<RescueStatusUI>(10); //never going to be more than 10. might aswell. may be unused tho lets see.

    private NPCController[] npcs;
    private RectTransform[] npcMarkers;
    private RectTransform zoneMarker;

    private int rescuedNPCs = 0;
    private int deadNPCs = 0;

    /// <summary>
    /// 1. rescued npcs (amount)
    /// 2. deadNPCs (amount)
    /// </summary>
    public System.Action<int, int> OnNPCsDeadOrRescued;

    private void Start()
    {
        //this is bad but im lazy. sue me.
        npcs = FindObjectsOfType<NPCController>();
        npcMarkers = new RectTransform[npcs.Length];
        for (int i = 0; i < npcs.Length; i++)
        {
            var n = npcs[i];
            var go = Instantiate(statusUIPrefab, statusContainer);
            var comp = go.GetComponent<RescueStatusUI>();
            statusUIs.Add(comp);
            n.OnRescueStatusChanged += comp.UpdateImageByStatus;
            n.OnRescueStatusChanged += OnNPCStatusChange;

            go = Instantiate(npcMarkerPrefab, canvas);
            npcMarkers[i] = go.transform as RectTransform;
        }
        zoneMarker = Instantiate(zoneMarkerPrefab, canvas).transform as RectTransform;
    }

    private void LateUpdate()
    {
        for(int i = 0; i < npcs.Length; i++)
        {
            if (npcs[i].Status == ERescueStatus.RescueFinal || npcs[i].Status == ERescueStatus.NpcDied)
            {
                npcMarkers[i].gameObject.SetActive(false);
                continue;
            }
            Vector3 pos = npcs[i].transform.position;
            Vector3 screenPos = camera.WorldToScreenPoint(pos);
            screenPos.x = Mathf.Clamp(screenPos.x, 0, 1920);
            screenPos.y = Mathf.Clamp(screenPos.y, 0, 1080);
            npcMarkers[i].gameObject.SetActive((screenPos - new Vector3(960, 540)).magnitude > 500);

            npcMarkers[i].anchoredPosition = screenPos;
        }
        //now the same for the zone
        Vector3 sPos = camera.WorldToScreenPoint(this.transform.position);
        sPos.x = Mathf.Clamp(sPos.x, 0, 1920);
        sPos.y = Mathf.Clamp(sPos.y, 0, 1080);
        zoneMarker.gameObject.SetActive((sPos - new Vector3(960, 540)).magnitude > 500);
        zoneMarker.anchoredPosition = sPos;
    }

    private void OnNPCStatusChange(ERescueStatus status)
    {
        if (status == ERescueStatus.RescueFinal)
            rescuedNPCs++;
        if (status == ERescueStatus.NpcDied)
            deadNPCs++;

        if(rescuedNPCs + deadNPCs == npcs.Length)
        {
            OnNPCsDeadOrRescued?.Invoke(rescuedNPCs, deadNPCs);
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
