using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage; // 데미지
    public int per; // 관통력
    Rigidbody2D rigid;
    private float speed; // 총알 속도
    private Vector3 direction; // 총알의 방향
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Init(float damage, int per, Vector3 dir, float speed) // 속도 파라미터 추가
    {
        this.damage = damage;
        this.per = per;

        if (per > -1)
        {
            rigid.velocity = dir.normalized * speed; // dir을 정규화하고 속도를 적용
        }
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized; // 새로운 방향 저장
        rigid.velocity = direction * speed; // 새로운 방향으로 속도 설정
    }

    private void DeactivateBullet()
    {
        rigid.velocity = Vector2.zero; // 속도 0으로 설정
        gameObject.SetActive(false); // 비활성화
    }

    public void ResetBullet()
    {
        rigid.velocity = Vector2.zero; // 속도 0으로 설정
        gameObject.SetActive(true); // 활성화
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        per--;

        if (per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
