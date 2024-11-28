using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    private Vector2 lastDirection = Vector2.right;
    private float bulletOffset = 1f;
    private float BasicCooldown = 3.0f;
    private float BasicTimer = 0f;

    private float VoltCooldown = 2.0f;
    private float VoltTimer = 0f;

    private int RainProjectileCount = 10; // 투사체 개수를 20으로 변경
    private float RainAttackCooldown = 9f; // 9초 간격
    private float RainTimer = 0f;

    public void SetRainCount(int newCount)
    {
        RainProjectileCount = newCount;
    }

    public void SetRainCooldown(float newCooldown1)
    {
        RainAttackCooldown = newCooldown1;
    }

    private float BlackCooldown = 0.5f;
    private float BlackTimer = 0f;

    public void SetBlackCooldown(float newCooldown2)
    {
        BlackCooldown = newCooldown2;
    }

    private float MeteorTimer = 0f;
    private float MeteorCooldown = 5.0f;
    public void SetMeteorCooldown(float newCooldown)
    {
        MeteorCooldown = newCooldown;
    }


    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;


        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // 방향키 입력 처리
        if (horizontalInput != 0)
        {
            lastDirection = horizontalInput < 0 ? Vector2.left : Vector2.right; // 마지막 방향 저장
        }

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 6:
                transform.Rotate(Vector3.back * speed * Time.deltaTime * 0);
                break;
            case 11:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 7:
                VoltTimer += Time.deltaTime;
                if (VoltTimer > VoltCooldown)
                {
                    Volt();
                    VoltTimer = 0f;
                    SoundManager.Instance.PlayVoltSound();
                }
                break;
            case 8:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    FireWalk();
                    SoundManager.Instance.PlayFireworkSound();
                }
                break;
            case 9:
                RainTimer += Time.deltaTime;
                if (RainTimer > RainAttackCooldown)
                {
                    RainAttack();
                    RainTimer = 0f;
                    SoundManager.Instance.PlayBlizzardSound();
                }
                break;
            case 10:
                BlackTimer += Time.deltaTime;
                if (BlackTimer > BlackCooldown)
                {
                    Black();
                    BlackTimer = 0f;
                    SoundManager.Instance.PlayBlackBallSound();
                }
                break;

            case 12:
                BasicTimer += Time.deltaTime;
                if (BasicTimer > BasicCooldown)
                {
                    Basic();
                    BasicTimer = 0.5f;
                    SoundManager.Instance.PlayWandAttackSound();
                }
                break;

            default:
                timer += Time.deltaTime; // 모든 상황에서 사용하는 타이머 업데이트

                // id가 3인 경우에만 사용하는 타이머 업데이트
                if (prefabId == 5)
                {
                    MeteorTimer += Time.deltaTime;
                    if (MeteorTimer > MeteorCooldown)
                    {
                        MeteorTimer = 0f; // id가 3일 때만 타이머 초기화
                        Meteor();
                        SoundManager.Instance.PlayMeteorSound();
                    }
                }
                else if (timer > speed) // timer가 speed를 초과한 경우
                {
                    timer = 0f;
                    if (prefabId == 2)
                    {
                        // prefabId가 2일 때 Fire 호출
                    }
                    else
                    {
                        Fire(); // prefabId가 3이 아닐 때 Meteor 호출
                        SoundManager.Instance.PlayRangedSound();
                    }
                }
                break;
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            Batch();
            SoundManager.Instance.PlayMeleeSound();
        }
        if (id == 6)
        {
            BatchCircle();
            SoundManager.Instance.PlayIceCircleSound();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        //Basic Set
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.basecount;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projecttile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }
        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                SoundManager.Instance.PlayMeleeSound();
                break;
            case 5:
                speed = 5.0f;
                break;
            case 6:
                speed = 0;
                BatchCircle();
                SoundManager.Instance.PlayIceCircleSound();
                break;
            case 7:
                speed = 2.0f;
                break;
            case 8:
                speed = 2f;
                break;
            default:
                speed = 0.4f;
                break;
            case 11:
                speed = 50;
                BlackHole();
                SoundManager.Instance.PlayBlackHoleSound();
                break;
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 2f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero, 150); // -1 is Infinity Per.
        }
    }

    void BatchCircle()
    {
        Transform bullet;

        if (transform.childCount > 0)
        {
            bullet = transform.GetChild(0);
        }
        else
        {
            bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.parent = transform;
        }

        bullet.localPosition = Vector3.zero;
        bullet.localRotation = Quaternion.identity;

        // 스케일을 count에 비례하여 증가시키기
        float scaleFactor = 9 + (float)(count - 1) * 0.1f; // 여기서 0.1f는 증가율을 나타냅니다. 게임의 요구 사항에 맞게 조정하세요.
        bullet.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero, 0); // -1 is Infinity Per.
    }

    void Fire()
    {
        // 목표가 없으면 발사하지 않음
        if (!player.scanner.nearestTarget)
            return;

        // 목표 위치와 방향 계산
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = (targetPos - transform.position).normalized;

        // 총알 생성 및 초기화
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        // 총알 초기화
        bullet.GetComponent<Bullet>().Init(damage, count, dir, 15f);
    }

    void Meteor()
    {
        // 여러 개의 총알을 발사
        for (int index = 0; index < 10; index++)
        {
            // 총알 생성
            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.parent = transform;
            bullet.position = transform.position;

            // 무작위 방향 생성
            float angle = Random.Range(0f, 360f);
            Vector3 dirVec = Quaternion.Euler(0f, 0f, angle) * Vector3.up;

            // 총알 회전 설정
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dirVec);

            // 총알 초기화
            bullet.GetComponent<Bullet>().Init(damage, count, dirVec, 5f);
        }
    }

    void Volt()
    {
        UnityEngine.Debug.Log("Volt Attack!");

        // 플레이어가 누르고 있는 방향키 확인
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 shootDirection;
        float angle;

        // 발사 방향 및 각도 설정
        if (horizontalInput < 0)
        {
            shootDirection = Vector2.left;
            angle = 180f;
        }
        else if (horizontalInput > 0)
        {
            shootDirection = Vector2.right;
            angle = 0f;
        }
        else
        {
            // 플레이어가 아무 방향키도 누르지 않았을 때 발사하지 않음
            return; // 메서드를 여기서 종료
        }

        // 발사체 생성 및 초기화
        GameObject bullet = GameManager.instance.pool.Get(prefabId);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        bullet.GetComponent<Bullet>().Init(damage, count, shootDirection, 15f);
    }

    void FireWalk()
    {
        if (!player.scanner.nearestTarget)
            return;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.GetComponent<Bullet>().Init(damage, count, Vector3.zero, 0);
    }

    private void RainAttack()
    {
        UnityEngine.Debug.Log("Rain Attack!");

        // 화면 상단의 X 좌표 범위 설정
        float minX = Camera.main.transform.position.x - (Camera.main.aspect * Camera.main.orthographicSize);
        float maxX = Camera.main.transform.position.x + (Camera.main.aspect * Camera.main.orthographicSize);

        // 화면 상단의 Y 좌표 설정
        float y = Camera.main.transform.position.y + 5f;

        for (int i = 0; i < RainProjectileCount; i++)
        {
            // 랜덤한 X 좌표 생성
            float x = Random.Range(minX, maxX);

            // 투사체 생성 위치 설정
            Vector3 spawnPosition = new Vector3(x, y, transform.position.z);

            // 투사체의 발사 방향을 아래쪽으로 설정
            Vector2 shootDirection = Vector2.down;
            float angle = 270f; // 아래쪽으로 발사

            // 투사체의 속도를 랜덤하게 설정
            float speed = Random.Range(5f, 15f);

            // 발사체 생성 및 초기화
            GameObject bullet = GameManager.instance.pool.Get(prefabId);
            bullet.transform.position = spawnPosition;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            bullet.GetComponent<Bullet>().Init(damage, count, shootDirection, speed);
        }
    }

    void Black()
    {
        if (!player.scanner.nearestTarget)
            return;

        // count가 0인 경우 발사하지 않음
        //if (count <= 0)
            //return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        float angleStep = 10f; // 각 총알 간의 각도 (예: 10도)
        float startAngle = -((count - 1) * angleStep) / 2; // 첫 번째 총알의 시작 각도

        for (int index = 0; index < count; index++)
        {
            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.position = transform.position; // 총알의 위치를 발사 지점으로 설정

            // 각 총알의 회전을 계산
            float angle = startAngle + index * angleStep; // 각도를 계산
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle); // 회전 생성

            // 방향을 설정
            Vector3 bulletDir = bulletRotation * dir; // 회전된 방향
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, bulletDir); // 방향을 설정

            // 총알 초기화 (damage, count, direction)
            bullet.GetComponent<Bullet>().Init(damage, 0, bulletDir, 5f);
        }
    }

    void BlackHole()
    {
        // 발사체를 가져올 변수 선언
        Transform bullet;

        // 기존에 자식 발사체가 있는지 확인
        if (transform.childCount > 0)
        {
            bullet = transform.GetChild(0); // 첫 번째 자식 발사체 사용
        }
        else
        {
            // 새로운 발사체를 풀에서 가져옴
            bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.parent = transform; // 부모를 현재 오브젝트로 설정
        }

        // 발사체의 위치 및 회전 초기화
        bullet.localPosition = Vector3.zero;
        bullet.localRotation = Quaternion.identity;

        bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero, 50); // -1은 무한 발사 횟수

        // 발사체를 발사하는 방향을 설정 (예: 위쪽 방향으로 발사)
        bullet.Translate(bullet.up * 4f, Space.World); // 원하는 거리로 조정 가능
    }

    void Basic()
    {
        UnityEngine.Debug.Log("attack");

        // 아무 방향키도 누르지 않았을 때 마지막 방향 사용
        float horizontalInput = lastDirection == Vector2.left ? -1 : 1;

        // 발사 방향 및 각도 설정
        Vector2 shootDirection = horizontalInput < 0 ? Vector2.left : Vector2.right;
        float angle = horizontalInput < 0 ? 180f : 0f;

        // 총알을 풀에서 가져오기
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;

        // 플레이어의 위치에 따라 총알 생성 위치 조정
        Vector2 bulletPosition = (Vector2)transform.position + shootDirection * bulletOffset;
        bullet.position = bulletPosition;
        bullet.rotation = Quaternion.Euler(0f, 0f, angle);

        // Y축 스케일 설정 (상하 반전)
        bullet.localScale = new Vector3(bullet.localScale.x, horizontalInput < 0 ? -Mathf.Abs(bullet.localScale.y) : Mathf.Abs(bullet.localScale.y), bullet.localScale.z);

        // 총알 초기화
        bullet.GetComponent<Bullet>().Init(damage, count, shootDirection, 0);

        // 총알이 0.2초 후에 사라지도록 코루틴 시작
        StartCoroutine(DestroyBulletAfterTime(bullet.gameObject, 0.22f));
    }

    // 코루틴 메소드
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false); // 총알 비활성화
        UnityEngine.Debug.Log("delete");
    }
}
