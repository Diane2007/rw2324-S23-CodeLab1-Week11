using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Kitchen Object",
    menuName = "ScriptableObject/Location/KitchenObject",
    order = 0
)]
public class KitchenScriptableObject : ScriptableObject
{
    public string objectName;
    public TextAsset objectDescription;

}
