using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ItemResult : MonoBehaviour
{
    public ItemData data;
    public int level;

    Text textLevel;

    void Awake()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
    }

    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.MeLee:
            case ItemData.ItemType.Range:
            case ItemData.ItemType.Shotgun:
            case ItemData.ItemType.Circle:
            case ItemData.ItemType.Volt:
            case ItemData.ItemType.Rain:
            case ItemData.ItemType.FireWalk:
            case ItemData.ItemType.Black:
            case ItemData.ItemType.BlackHole:
            case ItemData.ItemType.Basic:
            case ItemData.ItemType.test:
            case ItemData.ItemType.test2:
                break;
        }
    }

    public void onclick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.MeLee:
            case ItemData.ItemType.Range:
            case ItemData.ItemType.Shotgun:
            case ItemData.ItemType.Circle:
            case ItemData.ItemType.Volt:
            case ItemData.ItemType.FireWalk:
            case ItemData.ItemType.Rain:
            case ItemData.ItemType.Black:
            case ItemData.ItemType.BlackHole:
            case ItemData.ItemType.Basic:
            case ItemData.ItemType.test:
            case ItemData.ItemType.test2:

                level++;
                break;
        }

        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }

    void LateUpdate()
    {
        textLevel.text = "Lv." + (level);
    }
}