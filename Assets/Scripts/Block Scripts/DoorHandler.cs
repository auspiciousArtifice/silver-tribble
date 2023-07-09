using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public GameObject doorScreen;
    public GameObject door;

    public void openDoor(int i){
        if(i == 1){
            GameObject.FindWithTag("Player").GetComponent<Movement>().jumpCount--;
        }
        else if(i == 2){
            GameObject.FindWithTag("Player").GetComponent<Movement>().dashCount--;
        }
        GameObject.FindWithTag("Respawn").transform.position = transform.position;
        doorScreen.SetActive(false);
        door.SetActive(false);
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            Time.timeScale = 0;
            doorScreen.SetActive(true);
        }
    }
}
