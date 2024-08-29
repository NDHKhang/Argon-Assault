using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject DeathFX;
    [SerializeField] GameObject HitVFX;

    [SerializeField] int score = 10;
    [SerializeField] int hitPoint = 4;

    ScoreBoard scoreBoard;
    GameObject parentGameObject; // Storing gameobject spawn at runtime

    public List<ParticleCollisionEvent> collisionEvents;


    void Start()
    {
        scoreBoard = FindAnyObjectByType<ScoreBoard>();
        parentGameObject = GameObject.FindWithTag("SpawnAtRuntime");
        AddRigridBody(); // Adding Rigridbody at runtime instead of manual
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void AddRigridBody()
    {
        if(gameObject.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true; // Avoid enemy collide with each other or the terrain
        }   
    }

    void OnParticleCollision(GameObject other)
    {
        ParticleSystem part = other.GetComponent<ParticleSystem>();
        int numCollisionEvents = part.GetCollisionEvents(this.gameObject, collisionEvents);
        
        foreach (ParticleCollisionEvent collisionEvent in collisionEvents)
        {
            Vector3 pos = collisionEvent.intersection;
            ProcessHit(pos);
        }
        if (hitPoint == 0)
        {
            KillEnemy();
        }
    }

    void ProcessHit(Vector3 intersectionPos)
    {
        hitPoint--;
        GameObject vfx = Instantiate(HitVFX, intersectionPos, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
    }

    void KillEnemy()
    {
        scoreBoard.IncreaseScore(score);
        GameObject vfx = Instantiate(DeathFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        Destroy(gameObject);
    }

}
