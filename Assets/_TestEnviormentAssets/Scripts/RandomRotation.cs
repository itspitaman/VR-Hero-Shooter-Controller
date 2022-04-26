using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public float rotationSpeed;
    public float rotationX, rotationY, rotationZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationX, rotationY, rotationZ, Space.Self);
    }
}
