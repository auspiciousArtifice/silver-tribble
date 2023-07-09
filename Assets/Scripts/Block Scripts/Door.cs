using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameObject doorScreen;

    void Awake(){
        doorScreen = GameObject.Find("Door Screen");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            Time.timeScale = 0;

            doorScreen.SetActive(true);
            doorScreen.GetComponent<DoorHandler>().setDoor(gameObject.transform.parent.gameObject);

            Movement m = other.gameObject.GetComponent<Movement>();
            if(m.jumpCount <= 0){
                doorScreen.transform.GetChild(1).gameObject.SetActive(false);
            }
            if(m.dashCount <=0){
                doorScreen.transform.GetChild(2).gameObject.SetActive(false);
            }

        }
    }
}
