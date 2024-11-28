/*using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI; // UnityEngine.UI 네임스페이스 추가

public class Resultsum : MonoBehaviour
{
    RectTransform rect;
    public Item[] items;
    public int itemIndex; // 컴포넌트에서 설정할 아이템 번호
    public UnityEngine.UI.Image itemImage; // UnityEngine.UI.Image로 명시적으로 지정

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
        UpdateItemDisplay();
    }

    void UpdateItemDisplay()
    {
        // 아이템 인덱스가 범위 내인지 확인
        if (itemIndex >= 0 && itemIndex < items.Length)
        {
            Item selectedItem = items[itemIndex];

            // 해당 아이템이 레벨업 되었는지 확인
            if (selectedItem.level > 0) // 예시로 레벨이 0보다 크면 레벨업했다고 가정
            {
                itemImage.gameObject.SetActive(true);
            }
            else
            {
                itemImage.gameObject.SetActive(false);
            }
        }
        else
        {
            itemImage.gameObject.SetActive(false);
        }
    }
}
*/