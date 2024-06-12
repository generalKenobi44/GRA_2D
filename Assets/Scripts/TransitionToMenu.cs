using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToMenu : MonoBehaviour
{
    public float transitionTime = 10f;
    public float timeSinceEntered;
    // Start is called before the first frame update
    private void Awake()
    {
        timeSinceEntered = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceEntered > transitionTime) { SceneManager.LoadScene(0); }
        timeSinceEntered += Time.deltaTime;
    }
}
