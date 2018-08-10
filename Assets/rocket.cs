using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour {

    Rigidbody rigidBody;
    int multi = 25;
    AudioSource rthrust;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        rthrust = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        processInput();
	}

    private void processInput()
    {
        thrust();
        rotate();

    }

    private void rotate()
    {
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * Time.deltaTime * multi);
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * multi);
        }

        rigidBody.freezeRotation = false;
    }

    private void thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);

            if(!rthrust.isPlaying)
            {
                rthrust.Play();
            }

        }

        else
        {
            rthrust.Stop();
        }
    }
}
