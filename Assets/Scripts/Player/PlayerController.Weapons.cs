using UnityEngine;


public partial class PlayerController
{
    [Header("Weapons")]
    public GameObject[] weaponPrefabs;
    private int currentIndex = 0;

    private GameObject[] instantiatedObjects;
    private void UpdateWeapons()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            ScrollTo(1);
        }
        else if (scroll < 0f)
        {
            ScrollTo(-1);
        }
    }

    void ScrollTo(int direction)
    {

        instantiatedObjects[currentIndex].SetActive(false);

        currentIndex = (currentIndex + direction + weaponPrefabs.Length) % weaponPrefabs.Length;

        instantiatedObjects[currentIndex].SetActive(true);
    }
}


