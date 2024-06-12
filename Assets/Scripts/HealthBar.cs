using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    Damageable playerDamagealbe;
    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(playerDamagealbe.Health, playerDamagealbe.MaxHealth);
        healthBarText.text = "HP: " + playerDamagealbe.Health + " / " + playerDamagealbe.MaxHealth;
    }

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null ) { Debug.Log("No Player found in scene"); }
        playerDamagealbe = player.GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        playerDamagealbe.healtChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        playerDamagealbe.healtChanged.RemoveListener(OnPlayerHealthChanged);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return Mathf.Max(currentHealth, 0f) / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP: " + Mathf.Max(newHealth, 0) + " / " + maxHealth;
    }
}
