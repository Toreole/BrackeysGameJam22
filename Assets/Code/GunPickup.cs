using UnityEngine;
public class GunPickup : PlayerTriggerBase
{
    protected override void OnPlayerEnter(Player player)
    {
        gameObject.SetActive(false);
        player.GetComponent<PlayerGunController>().Wakeup();
    }
}

