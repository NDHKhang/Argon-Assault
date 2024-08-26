using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem crashVFX;

    [SerializeField] AudioClip crashAudioClip;
    AudioSource audioSource;

    // For debug/cheat keybind
    private bool collisionDisable = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!collisionDisable)
            StartCoroutine(StartCrashSequence());
    }

    IEnumerator StartCrashSequence()
    {
        audioSource.PlayOneShot(crashAudioClip, 0.3f);
        crashVFX.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<PlayerControls>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1);
        LoadManager.instance.GameOver();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable; //toggle collision
            Debug.Log("Collision Disable: " + collisionDisable);
        }
    }
}
