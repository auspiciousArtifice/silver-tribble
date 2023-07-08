using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneAxisFollow : MonoBehaviour
{
    [SerializeField] private GameObject followObject;
    [SerializeField] private Axis followAxis;

    // Update is called once per frame
    void Update()
    {
        Vector3 objectPos = followObject.transform.position;
        Vector3 setPosition = transform.position;
        setPosition.x = followAxis == Axis.X ? objectPos.x : setPosition.x;
        setPosition.y = followAxis == Axis.Y ? objectPos.y : setPosition.y;
        transform.position = setPosition;
    }
}

enum Axis
{
    X, Y
}
