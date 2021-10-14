using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopLocation", menuName ="Archetypes/ShopLocation")]
public class ShopLocationArchetype : ScriptableObject
{
    public List<ItemBase> Items;
    public Sprite ShopKeeperIcon;
    public string Title;
}
