using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject hitScanWeaponPrefab;

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

    }
}
