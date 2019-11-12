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
    int finalLevel;
    bool thrustenable = false;
    bool rotate_right = false;
    bool rotate_left = false;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audiosorce = GetComponent<AudioSource>();
        state = State.alive;
        level = SceneManager.GetActiveScene().buildIndex;
        finalLevel = SceneManager.sceneCountInBuildSettings-1;
    }
	
	// Update is called once per frame
	void Update () {
        process_input();
	}

    private void process_input()
    {
        if (state ==State.alive)
        {
            thrust();
            rotate();
        }
    }

    public void enable_right_rotation()
    {
        rotate_right = true;
    }

    public void disable_right_rotation()
    {
        rotate_right = false;
    }

    public void enable_left_rotation()
    {
        rotate_left = true;
    }

    public void disable_leftt_rotation()
    {
        rotate_left = false;
    }

    private void rotate()
    {
        

        if (rotate_right)
        {
            transform.Rotate(-Vector3.forward * Time.deltaTime * multirotate);
        }

        else if (rotate_left)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * multirotate);
        }

        
    }

    public void enable_thrust()
    {
        thrustenable = true;
    }

    public void disable_thrust()
    {
        thrustenable = false;
    }

    private void thrust()
    {
        if (thrustenable)
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
