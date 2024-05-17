using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{

    public GameObject monster;


    // Start is called before the first frame update
    void Start()
    {
        
        monster.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            monster.SetActive(true);
        }
    }

}
