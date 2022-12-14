using UnityEngine;

/// <summary>
/// Manages general Player data like for example a "inventory" of keys.
/// </summary>
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip pickupClip;

    private bool isDead = false;

    private static readonly int anim_dead = Animator.StringToHash("IsDead");

    private EKey collectedKeys = EKey.NONE_OR_DEFAULT;

    public void AddKey(EKey key)
    {
        audioSource.PlayOneShot(pickupClip);
        collectedKeys |= key;
    }
    public static Player Current { get; private set; }

    public event System.Action OnPlayerDied;

    private void Awake()
    {
        Current = this;   
    }

    public void Damage()
    {
        if (isDead)
            return;
        isDead = true;
        animator.SetBool(anim_dead, true);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<PlayerMovement>().enabled = false;
        OnPlayerDied?.Invoke();
    }

    public bool HasKey(EKey key) => collectedKeys.HasFlag(key);
}
