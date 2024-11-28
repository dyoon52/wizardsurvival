using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scripttble Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType {  MeLee, Range, Glove, Shoe, Heal, 
        Shotgun, Circle, Volt, FireWalk, Rain, Black, BlackHole, Basic, test, test2 }

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;


    [Header("# Level Data")]
    public float baseDamage;
    public int basecount;
    public float[] damages;
    public int[] count;


    [Header("# Weapon")]
    public GameObject projecttile;
}
