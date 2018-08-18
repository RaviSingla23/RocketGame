using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocket : MonoBehaviour {

    Rigidbody rigidBody;
    [SerializeField] int multirotate = 50;
    [SerializeField] int multithrust = 50;

    AudioSource audiosorce;
    [SerializeField] AudioClip engine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip leavelcomplete;

    enum State {alive,dead,levelchange};
    State state;
    [SerializeField] ParticleSystem flame;
    [SerializeField] ParticleSystem explode;
    [SerializeField] ParticleSystem win;
    int level;
    int finalLevel = 2;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audiosorce = GetComponent<AudioSource>();
        state = State.alive;
        level = SceneManager.GetActiveScene().buildIndex;
	}
	
	// Update is called once per frame
	void Update () {
        processInput();
	}

    private void processInput()
    {
        if(state==State.alive)
        {
            thrust();
            rotate();
        }
    }

    private void rotate()
    {
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * Time.deltaTime * multirotate);
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * multirotate);
        }

        rigidBody.freezeRotation = false;
    }

    private void thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * multithrust);
            flame.Play();

            if(!audiosorce.isPlaying)
            {
                audiosorce.PlayOneShot(engine);
            }

        }

        else
        {
            flame.Stop();
            audiosorce.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state!=State.alive)
        {
            return;
        }
        switch(collision.gameObject.tag)
        {
            case "friendly":
                break;

            case "Finish":
                state = State.levelchange;
                audiosorce.Stop();
                win.Play();
                audiosorce.PlayOneShot(leavelcomplete);
                Invoke("LoadNextLevel",2f);
                break;
            default:
                state = State.dead;
                audiosorce.Stop();
                explode.Play();
                audiosorce.PlayOneShot(death);
                Invoke("LoadSameLevel",2f);
                break;

        }
    }

    private void LoadSameLevel()
    {
        SceneManager.LoadScene(level);
    }

    private void LoadNextLevel()
    {
        if (level<finalLevel)
            SceneManager.LoadScene(level + 1);
        else
            SceneManager.LoadScene(finalLevel);
    }
}
