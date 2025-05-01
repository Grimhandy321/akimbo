using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gundata;
    public Transform muzzle;
    private float timeSinceLastShot;
    private AudioSource audioSource;
    public PlayerController controller;

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawLine(Camera.main.transform.position, transform.forward);
    }
    private void Start()
    {
        PlayerController.shootInput += Shoot;
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = gundata?.shootsound;
    }

    private bool CanShoot() => timeSinceLastShot > 1f / (gundata.fireRate / 60);

    public void Shoot() 
    {
        if (CanShoot()) 
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,out RaycastHit hitInfo, gundata.range)) 
            {
                ITargetable target = hitInfo.transform.GetComponent<ITargetable>();
                CoinManager coinManager = hitInfo.transform.GetComponent<CoinManager>();
                if (target != null)
                {
                    target.Damage(controller.playerTeam, gundata.damage); 
                }
                if (coinManager != null) 
                {
                    coinManager.HitByHitScan(gundata.damage, controller.playerTeam);
                }
            }
            timeSinceLastShot = 0;
            OnGunShot();
        }
    }

    private void OnGunShot()
    {
        audioSource.Play();
    }
}
