using Alteruna;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gundata;
    public Transform muzzle;
    private float timeSinceLastShot;
    private AudioSource audioSource;
    public PlayerController controller;

    private void Start()
    {
        PlayerController.shootInput += Shoot;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gundata?.shootsound;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private bool CanShoot() => timeSinceLastShot > 1f / (gundata.fireRate / 60f);

    public void Shoot()
    {
        if (!controller.IsOwner) return;

        if (CanShoot())
        {
            timeSinceLastShot = 0;

            Multiplayer.InvokeRemoteProcedure("RPC_Shoot", UserId.All, controller.playerTeam);

            FireLocal(controller.playerTeam);
        }
    }

    [SynchronizableMethod]
    private void RPC_Shoot(Team team)
    {
        if (controller.IsOwner) return;
        FireLocal(team);
    }

    private void FireLocal(Team team)
    {
        gundata?.projectile?.Fire(team);
        if (audioSource && gundata?.shootsound)
        {
            audioSource.Play();
        }
    }
}
