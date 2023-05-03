using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(
    fileName = "New Location",
    menuName = "ScriptableObject/SpecialObject",
    order = 1
)]

public class LocationSpecialScriptableObject : LocationScriptableObject
{
    public Button button;
    public TextAsset triggeredText;

}
