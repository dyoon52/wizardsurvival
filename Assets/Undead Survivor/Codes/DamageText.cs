using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float moveSpeed; // 텍스트 이동속도
    public float alphaSpeed; // 투명도 변환속도
    public float destroyTime;
    private TextMeshPro text;
    private Color alpha;
    public int damage;

    private bool isDamageDisplayActive = true; // 피해량 표시 활성화 여부

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();
        alpha = text.color;

        // 피해량 표시가 활성화된 경우에만 텍스트를 보이게 설정
        if (!isDamageDisplayActive)
        {
            text.gameObject.SetActive(false);
        }

        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDamageDisplayActive) // 피해량 표시가 활성화된 경우에만 업데이트
        {
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
            alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
            text.color = alpha;
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    // 피해량 표시 활성화/비활성화 설정 메서드
    public void SetDamageDisplayActive(bool isActive)
    {
        isDamageDisplayActive = isActive;
        text.gameObject.SetActive(isActive); // 텍스트 활성화/비활성화
    }
}