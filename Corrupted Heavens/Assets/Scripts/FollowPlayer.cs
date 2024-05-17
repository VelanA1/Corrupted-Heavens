using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public float moveSpeed;
    public GameObject player;

    //rotate
    public Transform target;
    //public static Vector3 Point;
    //public float rotateSpeed;

    void Update()
    {
        //rotate
        transform.LookAt(target);


        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    /*void FixedUpdate()
    {
        Vector3 direction = Point - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
    }//*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
