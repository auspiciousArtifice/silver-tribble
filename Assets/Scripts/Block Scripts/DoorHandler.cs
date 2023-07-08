using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public GameObject doorScreen;
    public GameObject player;
    public GameObject door;

    public void openDoor(int i){
        if(i == 1){
            player.GetComponent<Movement>().jumpCount--;
        }
        else if(i == 2){
            player.GetComponent<Movement>().dashCount--;
        }
        doorScreen.SetActive(false);
        door.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            Time.timeScale = 0;
            doorScreen.SetActive(true);
        }
    }
}
