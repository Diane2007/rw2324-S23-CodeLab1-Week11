using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCount1Fl : MonoBehaviour
{
    public GameObject jam, sink, flyer, chair, gun, basket;

    //get the position of the selected game object when called
    public void GetLocation(GameObject gameObject)
    {
        Vector2 objLocation = gameObject.transform.position;
    }
}
