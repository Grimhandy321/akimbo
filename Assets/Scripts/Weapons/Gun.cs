using UnityEngine;
using Alteruna;
using Alteruna.Trinity;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gundata;
    public Transform muzzle;
    private float timeSinceLastShot;
    public PlayerController controller;
    private Multiplayer multiplayer;

    private void Awake()
    {
        multiplayer = FindObjectOfType<Multiplayer>();
        if (multiplayer == null)
        {
            Debug.LogError("Multiplayer component not found");
        }
        else
        {
            multiplayer.RegisterRemoteProcedure("RPC_Shoot", RPC_Shoot);
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private bool CanShoot() => timeSinceLastShot >= 60f / gundata.fireRate;

    public void Shoot()
    {
        if (!controller._isOwner || gundata == null || gundata.projectile == null)
            return;

        if (CanShoot())
        {
            timeSinceLastShot = 0;

            Vector3 pos = controller.cam.transform.position;
            Vector3 dir = controller.cam.transform.forward;
            ushort senderID = multiplayer.Me.Index;

            //DEBUG LINE:
            Debug.DrawRay(pos, dir * 1000f, Color.red, 1.5f);

            ProcedureParameters parameters = new ProcedureParameters();
            parameters.Set("team", (ushort)controller.playerTeam);
            parameters.Set("posX", pos.x);
            parameters.Set("posY", pos.y);
            parameters.Set("posZ", pos.z);
            parameters.Set("dirX", dir.x);
            parameters.Set("dirY", dir.y);
            parameters.Set("dirZ", dir.z);
            parameters.Set("senderID", senderID);

            multiplayer.InvokeRemoteProcedure("RPC_Shoot", UserId.All, parameters);

            FireLocal(controller.playerTeam, pos, dir, senderID);
        }
    }

    public void RPC_Shoot(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        if (controller._isOwner)
            return;

        if (parameters.Get("team", out ushort teamValue) &&
            parameters.Get("posX", out float posX) &&
            parameters.Get("posY", out float posY) &&
            parameters.Get("posZ", out float posZ) &&
            parameters.Get("dirX", out float dirX) &&
            parameters.Get("dirY", out float dirY) &&
            parameters.Get("dirZ", out float dirZ) &&
            parameters.Get("senderID", out ushort senderID))
        {
            Vector3 pos = new Vector3(posX, posY, posZ);
            Vector3 dir = new Vector3(dirX, dirY, dirZ);
            Team team = (Team)teamValue;

            FireLocal(team, pos, dir, senderID);
        }
    }

    private void FireLocal(Team team, Vector3 pos, Vector3 dir, ushort senderId)
    {
        if (gundata?.projectile == null)
            return;

        GameObject projectileInstance = Instantiate(gundata.projectile, pos, Quaternion.LookRotation(dir));
        ProjectileBase projectile = projectileInstance.GetComponent<ProjectileBase>();

        if (projectile != null)
        {
            projectile.Fire(pos, dir, team, senderId);
        }
        else
        {
            Debug.LogWarning("Projectile prefab does not contain a ProjectileBase component.");
        }

        if (gundata.shootsound != null)
        {
            AudioSource.PlayClipAtPoint(gundata.shootsound, controller.transform.position);

        }
    }
}
