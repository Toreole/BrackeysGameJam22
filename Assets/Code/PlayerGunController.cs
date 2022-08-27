using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGunController : MonoBehaviour
{
    [SerializeField]
    private int shots = 10;
    [SerializeField]
    private GameObject ammoHUD;
    [SerializeField]
    private TextMeshProUGUI ammoText;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Transform gunTransform;
    [SerializeField]
    private Transform shootTransform;
    [SerializeField]
    private LayerMask hitMask;
    [SerializeField]
    private PolygonCollider2D hitShape;
    [SerializeField]
    private AudioClip shootClip;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip pickupClip;

    private RaycastHit2D[] hitBuffer = new RaycastHit2D[10];

    private float direction = 1;

    private int loadedAmmo;
    private int LoadedAmmo
    {
        get => loadedAmmo;
        set
        {
            if (value != loadedAmmo)
                ammoText.text = $"{loadedAmmo.ToString("00")}/{shots}";
            loadedAmmo = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleShooting();
    }

    private void HandleRotation()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector2 worldPos = cam.ScreenToWorldPoint(mousePos);
        Vector2 delta = worldPos - (Vector2)transform.position;
        //mouse is to the right side.
        if (0 < delta.x)
        {
            direction = 1;
            gunTransform.localScale = Vector3.one;
            gunTransform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, delta));
        }
        else //mouse is on the left.
        {
            direction = -1;
            gunTransform.localScale = new Vector3(-1, 1, 1);
            delta.x = -delta.x; //invert x.
            gunTransform.localRotation = Quaternion.Euler(0, 0, -Vector2.SignedAngle(Vector2.right, delta));
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //ContactFilter2D filter = new ContactFilter2D();
            //filter.layerMask = hitMask;
            Vector2 castDirection = gunTransform.right * direction;
            Vector2 castPos = shootTransform.position;
            hitBuffer = Physics2D.CircleCastAll(shootTransform.position, 0.35f, castDirection, 2.5f, hitMask);
            int found = hitBuffer.Length;//hitShape.Cast(gunTransform.right, filter, hitBuffer, 0.1f);
            LoadedAmmo--;
            audioSource.PlayOneShot(shootClip);
            Debug.Log($"Found colliders: {found}");
            for (int i = 0; i < found; i++)
            {
                var hit = hitBuffer[i];
                if ((direction > 0 && castPos.x > hit.point.x ) || (direction < 0 && castPos.x < hit.point.x))
                    continue;
                var dm = hit.collider.GetComponent<IDamageable>();
                if (dm != null)
                {
                    dm.Damage();
                }
            }

            if (LoadedAmmo == 0)
            {
                this.enabled = false;
                ammoHUD.SetActive(false);
                gunTransform.gameObject.SetActive(false);
            }
        }
    }

    public void Wakeup()
    {
        audioSource.PlayOneShot(pickupClip);
        this.enabled = true;
        ammoHUD.SetActive(true);
        gunTransform.gameObject.SetActive(true);
        loadedAmmo = shots;
    }
}
