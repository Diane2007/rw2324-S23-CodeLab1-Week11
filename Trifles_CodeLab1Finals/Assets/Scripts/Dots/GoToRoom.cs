using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToRoom : MonoBehaviour
{
    public static GoToRoom instance;
    
    [Header("First Floor")] 
    public GameObject livingRoom;
    public GameObject frontDoor;
    public GameObject kitchen;
    public GameObject stairs_1;

    void Awake()
    {
        if (!instance)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GetLocation(GameObject obj)
    {
        //GameManager.instance.roomLocation = gameObject.transform.position;
    }

}
