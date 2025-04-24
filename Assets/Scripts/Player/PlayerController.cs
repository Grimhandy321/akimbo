using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float maxHealth = 100f;
    public Team playerTeam;
    public Transform weaponHolder;
    private float currentHealth;
    public static Action shootInput;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        HandleShooting();
    }

    void HandleShooting()
    {
        if (Input.GetMouseButton(0))
        {
            shootInput?.Invoke();
        }
    }
    /*
    public void TakeDamage(float amount, Team attackerTeam, bool canFriendlyFire)
    {
        if (!canFriendlyFire && attackerTeam == playerTeam)
        {
            return;
        }

        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died");
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (weapon != null)
        {
            Destroy(weapon.gameObject);
        }

        GameObject weaponInstance = Instantiate(weaponPrefab, weaponHolder);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;

        weapon = weaponInstance.GetComponent<Weapon>();
    }*/
}
