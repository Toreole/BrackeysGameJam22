using UnityEngine;

/// <summary>
/// Manages general Player data like for example a "inventory" of keys.
/// </summary>
public class Player : MonoBehaviour
{
    private EKey collectedKeys = EKey.NONE_OR_DEFAULT;

    public void AddKey(EKey key) => collectedKeys |= key;
    public bool HasKey(EKey key) => collectedKeys.HasFlag(key);
}
