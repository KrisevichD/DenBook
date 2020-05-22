using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Transform image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        //StartCoroutine(rotateCube());

        Vector3 relativePos = -image.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);

        transform.rotation = rotation;
        
    }

}
