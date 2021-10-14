using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRegionLocation", menuName = "Custom/RegionLocation")]
public class RegionLocation : ScriptableObject
{
    public string Region;
    public string Location;
    public SceneAsset Scene;
}
