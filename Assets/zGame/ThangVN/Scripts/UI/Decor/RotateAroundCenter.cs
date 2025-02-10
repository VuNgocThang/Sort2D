using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundCenter : MonoBehaviour
{
    public Transform center; 
    public float speed = 50f; 

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
        transform.RotateAround(center.position, Vector3.forward, speed * Time.deltaTime);

        transform.rotation = initialRotation;
    }
}
