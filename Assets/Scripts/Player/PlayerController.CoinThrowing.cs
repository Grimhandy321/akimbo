using Alteruna.Trinity;
using Alteruna;
using UnityEngine;

public partial class PlayerController
{
    [Header("Coin Throwing")]
    public GameObject coinPrefab;
    public Transform throwOrigin;
    public float throwForce = 20f;
    public float throwCooldown = 1f;
    private float throwCooldownTimer = 0f;

    private Multiplayer multiplayer;

    private void Awake()
    {
        multiplayer = FindObjectOfType<Multiplayer>(); // pres to vlak nejede 
        if (multiplayer != null)
        {
            multiplayer.RegisterRemoteProcedure("RPC_ThrowCoin", RPC_ThrowCoin);
        }
        else
        {
            Debug.LogError("Multiplayer not found");
        }
    }

    private void UpdateCoinThrowing()
    {
        if (throwCooldownTimer > 0f)
            throwCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.G) && throwCooldownTimer <= 0f)
        {
            throwCooldownTimer = throwCooldown;

            if (!_isOwner || multiplayer == null)
                return;

            Vector3 pos = throwOrigin.position;
            Vector3 dir = throwOrigin.forward;

            ProcedureParameters parameters = new ProcedureParameters();
            parameters.Set("posX", pos.x);
            parameters.Set("posY", pos.y);
            parameters.Set("posZ", pos.z);
            parameters.Set("dirX", dir.x);
            parameters.Set("dirY", dir.y);
            parameters.Set("dirZ", dir.z);
            multiplayer.InvokeRemoteProcedure("RPC_ThrowCoin", UserId.All, parameters);

            ThrowCoin(pos, dir);
        }
    }

    public void RPC_ThrowCoin(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader reader)
    {
        if (_isOwner)
            return; 

        if (parameters.Get("posX", out float posX) &&
            parameters.Get("posY", out float posY) &&
            parameters.Get("posZ", out float posZ) &&
            parameters.Get("dirX", out float dirX) &&
            parameters.Get("dirY", out float dirY) &&
            parameters.Get("dirZ", out float dirZ))
        {
            Vector3 pos = new Vector3(posX, posY, posZ);
            Vector3 dir = new Vector3(dirX, dirY, dirZ);
            ThrowCoin(pos, dir);
        }
    }

    private void ThrowCoin(Vector3 pos, Vector3 dir)
    {
        GameObject coin = Instantiate(coinPrefab, pos, Quaternion.LookRotation(dir));
        Rigidbody rb = coin.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = dir * throwForce;
        }
    }
}
