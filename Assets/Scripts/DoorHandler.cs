using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    private GameObject door;

    public void setDoor(GameObject d){
        door = d;
    }

    void Start(){
        gameObject.SetActive(false);
    }

    public void openDoor(int i){
        GameObject player = GameObject.FindWithTag("Player");
        if(i == 1){
            player.GetComponent<Movement>().jumpCount--;
        }
        else if(i == 2){
            player.GetComponent<Movement>().dashCount--;
        }
        GameObject.FindWithTag("Respawn").transform.position = door.transform.GetChild(1).position;
        door.SetActive(false);
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

}
