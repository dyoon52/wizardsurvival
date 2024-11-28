using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    public bool isLive; // public으로 변경
    public bool isBulletEnemy; // bullet enemy 여부
    public bool isBoss; // 보스 여부

    private static int bossDefeatedCount = 0; // 처치된 보스 몬스터 수
    private NoticeArtifactManager artifactManager;

    public GameObject hudDamageText;

    Animator anim;
    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
        artifactManager = FindObjectOfType<NoticeArtifactManager>(); // NoticeArtifactManager 찾기

    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

        // 이동 로직
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // 이동 후 속도를 0으로 설정
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive) return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLive)
            return;

        if (isBulletEnemy && collision.CompareTag("Enemy"))
            return;

        if (collision.CompareTag("Bullet"))
        {
            if (!isBulletEnemy) // 일반 몬스터의 경우 체력 감소
            {
                float damage = collision.GetComponent<Bullet>().damage; // float로 가져오기
                TakeDamage((int)damage); // int로 변환하여 전달

                // 체력이 0 이하가 아닐 경우에만 KnockBack 코루틴 시작
                if (isLive)
                {
                    StartCoroutine(KnockBack());
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isLive) return; // isLive가 false인 경우 메서드 종료

        health -= damage; // 여기서 damage는 int 타입입니다.

        // 몬스터의 현재 위치
        Vector3 spawnPosition = transform.position; // 몬스터의 위치

        // HUD 데미지 텍스트 생성
        GameObject hudText = Instantiate(hudDamageText, spawnPosition, Quaternion.identity);
        DamageText damageText = hudText.GetComponent<DamageText>();
        damageText.damage = (int)damage; // float를 int로 변환하여 할당
        UnityEngine.Debug.Log(damage);

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            SoundManager.Instance.PlayMonsterHitSound();
        }
        else
        {
            Dead(); // 사망 처리 함수 호출
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    private void Dead()
    {
        isLive = false; // 보스가 죽은 상태로 설정
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = 1;
        anim.SetBool("Dead", true);

        if (isBoss) // 보스 몬스터인지 확인
        {
            artifactManager.ActivateRelic(bossDefeatedCount); // 유물 활성화
            bossDefeatedCount++; // 처치된 보스 수 증가
            GameManager.instance.OnBossDefeated();
        }

        GameManager.instance.kill++;
        GameManager.instance.totalKillCount++; // 누적 킬 수 증가

        PlayerPrefs.SetInt("TotalKillCount", GameManager.instance.totalKillCount); // PlayerPrefs에 누적 킬 수 저장
        PlayerPrefs.Save();

        GameManager.instance.GetExp();
        SoundManager.Instance.PlayMonsterDeathSound();

        StartCoroutine(DeactivateAfterAnimation());
    }

    // 애니메이션이 끝난 후 비활성화하는 코루틴
    private IEnumerator DeactivateAfterAnimation()
    {
        // 애니메이션의 길이 만큼 대기
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // 애니메이션이 완료된 후 비활성화
        gameObject.SetActive(false);
    }
}
