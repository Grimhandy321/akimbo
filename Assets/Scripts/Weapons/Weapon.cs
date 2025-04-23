using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName = "Weapon";
    public float damage = 10f;
    public float fireRate = 0.5f; // Shots per second
    public bool canFriendlyFire = false;
    public bool isFullAuto = false;

    [Header("Audio")]
    public AudioClip fireSound;
    protected AudioSource audioSource;

    protected float nextFireTime;
    protected bool hasFiredSinceLastRelease = false;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public abstract void Fire(Transform firePoint, Team shooterTeam);

    protected bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    protected void ResetFireCooldown()
    {
        nextFireTime = Time.time + 1f / fireRate;
    }

    protected void PlayFireSound()
    {
        if (fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

    public void TryFire(Transform firePoint, Team shooterTeam, bool isFiringHeld)
    {
        if (isFullAuto)
        {
            if (isFiringHeld && CanFire())
            {
                Fire(firePoint, shooterTeam);
                ResetFireCooldown();
            }
        }
        else
        {
            // Semi-auto: allow only one shot per click (no holding)
            if (!isFiringHeld)
            {
                hasFiredSinceLastRelease = false;
            }

            if (isFiringHeld && !hasFiredSinceLastRelease && CanFire())
            {
                Fire(firePoint, shooterTeam);
                ResetFireCooldown();
                hasFiredSinceLastRelease = true;
            }
        }
    }
}
