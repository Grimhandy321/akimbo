using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject hitScanWeaponPrefab;
    public GameObject projectileWeaponPrefab;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        // Equip a hitscan weapon at start
        player.EquipWeapon(hitScanWeaponPrefab);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            player.EquipWeapon(hitScanWeaponPrefab);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            player.EquipWeapon(projectileWeaponPrefab);
    }
}
