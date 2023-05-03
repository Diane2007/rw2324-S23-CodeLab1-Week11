using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextAsset triggeredText;

    public LocationScriptableObject forwardLocation, backwardLocation, leftLocation, rightLocation;
    
    public int buttonInt = 0;
    public string buttonText;
    public string exploreQuestion;

}
