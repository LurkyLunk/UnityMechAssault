using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject hitVFX;
    [SerializeField] int scorePerHit = 15;
    [SerializeField] int hitPoints = 4;

    ScoreBoard scoreBoard;
    GameObject parentGameObject;

    void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();
        parentGameObject = GameObject.FindWithTag("SpawnAtRuntime");
        // AddRigidbody();
    }
    
    // void AddRigidbody()
    // {
    //     Rigidbody rb = gameObject.AddComponent<Rigidbody>();
    //     rb.useGravity = false;

    // }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Projectile"))
        {
            ProcessHit();
            if (hitPoints < 1)
            {
                 KillEnemy();
            }
               
        }
    }
    void ProcessHit()
    {
        GameObject vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        hitPoints--;
        
    }


    void KillEnemy()
    {
        if (deathVFX != null)
        {
            
            scoreBoard.IncreaseScore(scorePerHit);
            GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
            vfx.transform.parent = parentGameObject.transform;
            Destroy(gameObject);
            
        }
    }
}

