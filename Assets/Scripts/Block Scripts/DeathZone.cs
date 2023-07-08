using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            GameObject respawnPoint = GameObject.FindWithTag("Respawn");
            other.gameObject.transform.position = ((respawnPoint != null) ? respawnPoint.transform.position : new Vector2(0,1));
            other.attachedRigidbody.velocity = Vector2.zero;
            //other.attachedRigidbody.angularVelocity = 0;
        }
    }
}
