using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RescueStatusUI : MonoBehaviour
{
    [SerializeField]
    private Image img;

    [SerializeField]
    private Sprite defaultSprite, rescuedSprite, deadSprite;

    public ERescueStatus Status { get; private set; }

    public void UpdateImageByStatus(ERescueStatus status)
    {
        Status = status;
        switch(status)
        {
            case ERescueStatus.None:
            default:
                img.sprite = defaultSprite;
                break;
            case ERescueStatus.RescueFinal:
            case ERescueStatus.BeingRescued:
                img.sprite = rescuedSprite;
                break;
            case ERescueStatus.NpcDied:
                img.sprite = deadSprite;
                break;
        }
    }
}

public enum ERescueStatus
{
    None,
    RescueFinal,
    BeingRescued,
    NpcDied
}
