using UnityEngine;

public abstract class PlayerTriggerBase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            OnPlayerEnter(collider.GetComponent<Player>());
    }

    protected abstract void OnPlayerEnter(Player player);
}
