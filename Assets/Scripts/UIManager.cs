using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject player;
    public GameObject deathMenu;
    Animator animator;

    public float deathTimer = 2f;
    public float timeSinceDeath = 0f;
    public void Awake()
    {
        Time.timeScale = 1.0f;
        animator = player.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        CharacterActions.characterDamaged += CharacterTookDamage;
        CharacterActions.characterHealed += CharacterHealed;
    }

    private void OnDisable()
    {
        CharacterActions.characterDamaged -= CharacterTookDamage;
        CharacterActions.characterHealed -= CharacterHealed;
    }
    void CharacterTookDamage(GameObject character, int damageReceived)
    {

    }

    void CharacterHealed(GameObject character, int damageHealed)
    {

    }

    private void Update()
    {
        if (animator != null && !animator.GetBool(AnimationStrings.IsAlive))
        {
            if (timeSinceDeath > deathTimer) { OnPlayerDeath(); }
            timeSinceDeath += Time.deltaTime;
        }
    }
    public void OnPause(InputAction.CallbackContext context) 
    {
        if (context.started) { Time.timeScale = 0; }
    }

    public void OnResume()
    {
        Time.timeScale = 1;
    }

    public void onQuitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnPlayerDeath()
    {
        Time.timeScale = 0;
        deathMenu.SetActive(true);
    }

    public void OnRetry()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
