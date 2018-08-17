using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class oscillator : MonoBehaviour {
    [SerializeField] Vector3 movementvector;
    float movementFactor;
    Vector3 startpos;
    [SerializeField]float period=1;
    float cycles;
    const float x = Mathf.PI * 2;
    Vector3 offset;
    // Use this for initialization
    void Start () {
        startpos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if(period<=Mathf.Epsilon)
        {
            return;
        }
        cycles = Time.time / period;
        movementFactor = Mathf.Sin(x*cycles);
        offset = movementFactor * movementvector;
        transform.position = startpos+offset;
        
	}
}
