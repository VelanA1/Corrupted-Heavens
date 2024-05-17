using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private PlayerController player;
    private bool canRespawn;

    public float respawnCountdown;
    

    void Start()
    {
        canRespawn = true;
        player = FindObjectOfType<PlayerController>();
                
    }


    void Update()
    {

    }

    public void Respawn()
    {
        Debug.Log("Respawn");
        canRespawn = false;
        StartCoroutine("RespawnCoroutine");
    }

    public IEnumerator RespawnCoroutine()
    {
        //want this to handle our respawning

        //specifically working with the playercontroller, so if we want to impact the entire player object, we need to say gameObject so we are not just the script
        player.gameObject.SetActive(false);

        //before we do movements, we have 2 options
        //move the player, then wait
        //or wait, then move the player and reactivate <-



        yield return new WaitForSeconds(respawnCountdown);

        //want to move the player to the respawn pos
        player.transform.position = player.respawnPos;


        canRespawn = true;

        //we can now reactivete ourselves since we have moved to the correct position
        player.gameObject.SetActive(true);


        yield return new WaitForSeconds(1);

    }

}
