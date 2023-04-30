using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Location",
    menuName = "ScriptableObject/Location",
    order = 0
    )]

public class LocationScriptableObject : ScriptableObject
{
    public string locationName;
    public TextAsset locationDescription;

    public LocationScriptableObject forwardLocation, backwardLocation, leftLocation, rightLocation;
}
