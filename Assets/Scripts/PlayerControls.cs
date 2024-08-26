using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("General Setup")]
    [Tooltip("How fast ship moves based on input")] [SerializeField] float controlSpeed = 20f;
    [Tooltip("How far ship can moves horizontally")] [SerializeField] float xRange = 10f;
    [Tooltip("How far ship can moves vertically")] [SerializeField] float yRange = 5f;

    [Header("Array for lasers")]
    [Tooltip("Store all lasers here")] [SerializeField] GameObject[] lasers;

    [Header("Making rotation look smoother")]
    [SerializeField] private float rotationFactor = 1;

    [Header("Screen position based tuning")]
    [SerializeField] float postitionPitchFactor = -2f;
    [SerializeField] float postitionYawFactor = 2f;
    
    [Header("Player input based tuning")]
    [SerializeField] float controlPitchFactor = -15f;
    [SerializeField] float controlRollFactor = -15f;


    float xThrow, yThrow;
    
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    void ProcessRotation()
    {
        float pitchDueToPostition = transform.localPosition.y * postitionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;

        float pitch = pitchDueToPostition + pitchDueToControlThrow;
        float yaw = transform.localPosition.x * postitionYawFactor;
        float roll = xThrow * controlRollFactor;

        //transform.localRotation = Quaternion.Euler(pitch, yaw, roll);

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationFactor);
    }

    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessFiring()
    {
        if(Input.GetButton("Fire1"))
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

    void SetLasersActive(bool isActive)
    {
        // Foreach laser, turn them on
        foreach(GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
