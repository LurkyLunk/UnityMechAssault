using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelay = 1f;
    [SerializeField] ParticleSystem crashVFX;

    bool hasCrashed = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasCrashed)
        {
            StartCrashSequence();
        }
    }

    void StartCrashSequence()
    {
        hasCrashed = true;
        crashVFX.Play();

        Transform childMesh = transform.GetChild(1); // Assuming the child object with SkinnedMeshRenderer is the first child
        SkinnedMeshRenderer childRenderer = childMesh.GetComponent<SkinnedMeshRenderer>();

        if (childRenderer != null)
        {
            childRenderer.enabled = false;
        }

        GetComponent<PlayerControls>().enabled = false;
        StartCoroutine(ReloadLevelWithDelay());
    }

    IEnumerator ReloadLevelWithDelay()
    {
        yield return new WaitForSeconds(loadDelay);
        ReloadLevel();
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
