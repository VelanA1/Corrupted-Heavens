using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPLayer : MonoBehaviour
{
    public Transform target;
    

    void Start()
    {

    }

    void Update()
    {
        transform.LookAt(target);
    }
}
