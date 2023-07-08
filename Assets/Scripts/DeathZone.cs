using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Zone : MonoBehaviour
{
    public Vector2 SpawnPoint;

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
            other.gameObject.transform.position = SpawnPoint;
            other.attachedRigidbody.velocity = Vector2.zero;
            //other.attachedRigidbody.angularVelocity = 0;
        }
    }
}
