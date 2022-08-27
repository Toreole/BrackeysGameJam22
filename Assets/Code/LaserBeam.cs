using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour, IDamageable
{
    [SerializeField]
    private bool animated = true;
    [SerializeField, Range(0.5f, 10f)]
    private float width = 1f;
    [SerializeField]
    private SpriteRenderer beamRenderer;
    [SerializeField]
    private Transform rightStation, leftStation;

    private void OnValidate()
    {
        var beamSize = beamRenderer.size;
        beamSize.x = width;
        beamRenderer.size = beamSize;

        float offset = (width-1f) * 0.3333f + 0.5f;
        rightStation.localPosition = new Vector3(offset, 0);
        leftStation.localPosition = new Vector3(-offset, 0);
    }

    public void Damage()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().enabled = animated;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.isTrigger)
            return;
        IDamageable dm = collider.GetComponent<IDamageable>();
        if (dm != null)
            dm.Damage();
    }
}
