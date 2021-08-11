using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunLogic : MonoBehaviour
{
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] Transform BulletSpawnPoint;

    [SerializeField] Text AmmoCountText;
    [SerializeField] AudioClip PistolShot;
    [SerializeField] AudioClip PistolEmpty;
    [SerializeField] public AudioClip PistolReload;

    AudioSource GunaudioSOurce;
    Rigidbody rb;
    new Collider collider;

    [SerializeField]
    const float MAX_COOLDOWN = .1f;
    [SerializeField]
    float current_Cooldown = 0.0f;
    [SerializeField]
    const int MaxAmmo = 20;
    
          int AmmoCount = MaxAmmo;

    bool isEquiped = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        GunaudioSOurce = GetComponent<AudioSource>();
        ClearAmmoText();
    }
   public void SetAmmoText()
    {
        AmmoCountText.text = $"Ammo: {AmmoCount}";
    }
    public void ClearAmmoText()
    {
        AmmoCountText.text = "Ammo: ";
    }
    public void PlaySound(AudioClip Sound)
    {
        if (GunaudioSOurce)
        {
            GunaudioSOurce.PlayOneShot(Sound);
        }
    }

    public void RefillAmmo()
    {
        AmmoCount += MaxAmmo;
        SetAmmoText();
    }

    public void EquipGun()
    {
        isEquiped = true;
        if (rb)
        {
            rb.useGravity = false;
        }
        if (collider)
        {
            collider.enabled = false;
        }
    }
    public void UnequipGun()
    {
        isEquiped = false;
        if (rb)
        {
            rb.useGravity = true;
        }
        if (collider)
        {
            collider.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isEquiped)
        {
            return;
        }

        if (current_Cooldown > 0.0f)
        {
            current_Cooldown -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Fire1") && current_Cooldown <= 0.0f)
        {
            if (AmmoCount > 0)
            {
                if (BulletPrefab && BulletSpawnPoint)
                {
                    Instantiate(BulletPrefab, BulletSpawnPoint.position, BulletSpawnPoint.rotation * BulletPrefab.transform.rotation);
                    current_Cooldown = MAX_COOLDOWN;
                    --AmmoCount;
                    SetAmmoText();
                    PlaySound(PistolShot);
                }
            }

            else
            {
                PlaySound(PistolEmpty);
            }

        }

    }
}
