using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Button exploreButton;
    public string exploreQuestion;

}
