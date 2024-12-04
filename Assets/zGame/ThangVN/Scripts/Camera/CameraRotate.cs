using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Transform target; 
    public float orbitSpeed = 1.5f; 

    //void Update()
    //{
    //    transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);

    //    transform.LookAt(target);
    //}

    public void RotateUp()
    {
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);

        transform.LookAt(target);
    }

    public void RotateDown()
    {
        transform.RotateAround(target.position, Vector3.down, orbitSpeed * Time.deltaTime);

        transform.LookAt(target);
    }
}
