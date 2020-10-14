using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rotSpeed = 100f;
    [SerializeField] float flySpeed = 100f;
    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip boomSound;
    [SerializeField] AudioClip finishSound;

    [SerializeField] ParticleSystem flyParticles;
    [SerializeField] ParticleSystem boomParticles;
    [SerializeField] ParticleSystem finishParticles;

    bool collisionOff = false;

    Rigidbody rigidBody;
    AudioSource audioSource;

    Vector3 xyzPos;
    Quaternion xyzRot;
    enum State {Playing, Dead, NextLevel};
    State state = State.Playing;
    void Start()
    {
        xyzPos = transform.position;
        xyzRot = transform.rotation;
        state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(state == State.Playing)
        {
            Launch();
            Rotation();
        }
        DebugKeys();
    }

    void DebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.F11))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.F6))
        {
            collisionOff = !collisionOff;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Playing || collisionOff)
        {
            return;
        }
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Finish":
                Finish();
                break;
            case "Battery":
                print("Plus Energy");
                break;
            case "Nastja":
                SceneManager.LoadScene(10);
                break;
            case "Makatya":
                SceneManager.LoadScene(11);
                break;
            default:
                Lose();
                break;
        }
    }

    void Lose()
    {
        state = State.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(boomSound);
        boomParticles.Play();
        Invoke("LoadLevelAgain", 2f);
    }

    void Finish()
    {
        state = State.NextLevel;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        finishParticles.Play();
        Invoke("LoadNextLevel", 2f);
    }

    void LoadNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 0;
        }
        SceneManager.LoadScene(nextLevelIndex);
    }

    void LoadLevelAgain()
    {
        transform.position = xyzPos;
        transform.rotation = xyzRot;
        state = State.Playing;
    }

    void Launch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(flySound);
                flyParticles.Play();
            }
        }
        else
        {
            audioSource.Pause();
            flyParticles.Stop();
        }
        
    }
    void Rotation()
    {

        rigidBody.freezeRotation = true;
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotSpeed * Time.deltaTime);
        }
        rigidBody.freezeRotation = false;
    }
    
}
