using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCountHidden : MonoBehaviour
{
    public GameObject cabinet;

    public void GetLocation(GameObject gameObject)
    {
        Vector2 objLocation = gameObject.transform.position;
    }
}
