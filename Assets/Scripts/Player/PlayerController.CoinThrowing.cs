using UnityEngine;

public partial class PlayerController
{
    [Header("Coin Throwing")]
    public GameObject coinPrefab;
    public Transform throwOrigin;
    public float throwForce = 20f;
    public float throwCooldown = 1f;
    private float throwCooldownTimer = 0f;

    private void UpdateCoinThrowing()
    {
        if (throwCooldownTimer > 0f)
            throwCooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && throwCooldownTimer <= 0f)
        {
            ThrowCoin();
            throwCooldownTimer = throwCooldown;
        }
    }

    private void ThrowCoin()
    {
        GameObject coin = Instantiate(coinPrefab, throwOrigin.position, throwOrigin.rotation);
        Rigidbody rb = coin.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = throwOrigin.forward * throwForce;
        }
    }
}
