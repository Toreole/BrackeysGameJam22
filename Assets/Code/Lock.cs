using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : PlayerTriggerBase
{
    [SerializeField]
    private Sprite defaultSprite;
    [SerializeField]
    private Sprite successSprite, failureSprite;

    [SerializeField]
    private new SpriteRenderer renderer;
    [SerializeField]
    private new AudioSource audio;
    [SerializeField]
    private AudioClip successClip, failureClip;

    [SerializeField]
    private EKey requiredKey;

    Coroutine animCoroutine;

    private void Start()
    {
        renderer.sprite = defaultSprite;
    }

    private IEnumerator DoAnim(Sprite s)
    {
        renderer.sprite = s;
        yield return new WaitForSeconds(1f);
        renderer.sprite = defaultSprite;
        animCoroutine = null;
    }

    protected override void OnPlayerEnter(Player player)
    {
        if (animCoroutine == null)
        {
            //do stuff
            if (player.HasKey(requiredKey)) //has key
            {
                animCoroutine = StartCoroutine(DoAnim(successSprite));
                audio.PlayOneShot(successClip);
            }
            else
            {
                animCoroutine = StartCoroutine(DoAnim(failureSprite));
                audio.PlayOneShot(failureClip);
            }
        }
    }
}

[System.Flags]
public enum EKey
{
    NONE_OR_DEFAULT = 0,
    Cyan = 1,
    Yellow = 2,
    Magenta = 4,
    Blue = 8
}
