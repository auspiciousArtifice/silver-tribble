using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    GameObject winScreen;

    private void Awake(){
        winScreen = GameObject.Find("Win Screen");
        winScreen.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            Time.timeScale = 0;
            winScreen.SetActive(true);
        }
    }
}
