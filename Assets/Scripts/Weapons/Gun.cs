using Alteruna;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gundata;
    public Transform muzzle;
    private float timeSinceLastShot;
    private AudioSource audioSource;
    public PlayerController controller;
    private Multiplayer multiplayer;

    private void Start()
    {
        PlayerController.shootInput += Shoot;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gundata?.shootsound;
    }

    private void Awake()
    {
        multiplayer = FindObjectOfType<Multiplayer>(); // presto vlak nejede 
        if (multiplayer == null)
        {
            Debug.LogError("Multiplayer component not found");
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private bool CanShoot() => timeSinceLastShot > 1f / (gundata.fireRate / 60f);

    public void Shoot()
    {
        if (!controller._isOwner) return;

        if (CanShoot())
        {
            timeSinceLastShot = 0;

            Vector3 pos = Camera.main.transform.position;
            Vector3 dir = Camera.main.transform.forward;
            ushort senderID  = (ushort)controller.Avatar.GetInstanceID();

            ProcedureParameters parameters = new ProcedureParameters();
            parameters.Set("team", (ushort)controller.playerTeam);
            parameters.Set("posX", pos.x);
            parameters.Set("posY", pos.y);
            parameters.Set("posZ", pos.z);
            parameters.Set("senderID", senderID);

            parameters.Set("dirX", dir.x);
            parameters.Set("dirY", dir.y);
            parameters.Set("dirZ", dir.z);


            multiplayer.InvokeRemoteProcedure("RPC_Shoot", (ushort)UserId.All, parameters);

            FireLocal(controller.playerTeam, pos, dir,senderID);
        }
    }

    private void RPC_Shoot(ProcedureParameters parameters)
    {
        if (controller._isOwner) return;

        if (parameters.Get("team", out ushort teamValue) &&
            parameters.Get("posX", out float posX) &&
            parameters.Get("posY", out float posY) &&
            parameters.Get("posZ", out float posZ) &&
            parameters.Get("dirX", out float dirX) &&
            parameters.Get("dirY", out float dirY) &&
            parameters.Get("senderID" , out ushort senderID) &&
            parameters.Get("dirZ", out float dirZ))
        {
            Vector3 pos = new Vector3(posX, posY, posZ);
            Vector3 dir = new Vector3(dirX, dirY, dirZ);
            Team team = (Team)teamValue;

            FireLocal(team, pos, dir, senderID);
        }

    }

    private void FireLocal(Team team, Vector3 pos, Vector3 dir,ushort senderId)
    {
        gundata?.projectile?.Fire(pos, dir, team,senderId);

        if (audioSource && gundata?.shootsound)
        {
            audioSource.Play();
        }
    }
}
