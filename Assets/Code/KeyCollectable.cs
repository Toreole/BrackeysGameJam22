using UnityEngine;

public class KeyCollectable : PlayerTriggerBase
{
    [SerializeField]
    private EKey key;

    protected override void OnPlayerEnter(Player player)
    {
        player.AddKey(key);
        gameObject.SetActive(false);
    }
}

