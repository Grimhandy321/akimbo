using AlterunaFPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHp : MonoBehaviour
{
    public static ShowHp Instance { get; private set; }
    public static Slider healthSlider;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(true);
    }
    public static void UpdateHealthUI(float hp)
    {
        if (healthSlider != null)
        {
            healthSlider.value = hp;
        }
    }
}
