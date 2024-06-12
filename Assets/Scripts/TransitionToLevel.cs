using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToLevel : MonoBehaviour
{
    public float transitionTime = 4f;
    public float timeSinceEntered;
    // Start is called before the first frame update
    private void Awake()
    {
        timeSinceEntered = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceEntered > transitionTime) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
        timeSinceEntered += Time.deltaTime;
    }
}
