using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public float teleportDistance = 8f; // 순간이동 거리
    private float teleportCooldown = 3f; // 순간이동 쿨타임
    private float lastTeleportTime;
    private bool isTeleporting = false;
    public bool canTeleport = false;
    public Scanner scanner;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer> ();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        //UnityEngine.Debug.Log($"Input Vector: {inputVec}");

        if (Input.GetKeyDown(KeyCode.Space) && canTeleport)
        {
            UnityEngine.Debug.Log("Teleport triggered");
            teleport();
            SoundManager.Instance.PlayTeleportSound();
        }
    }
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (isTeleporting)
        {
            isTeleporting = false; // 다음 프레임에서 다시 일반 이동 가능하도록 설정
            return;
        }

        // 위치 이동
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

     void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude); 

       if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive) // islive가 꺼져있으면 안된다.
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            for(int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");

            GameManager.instance.GameOver();
        }
    }

    void teleport()
    {
        if (Time.time < lastTeleportTime + teleportCooldown)
        {
            UnityEngine.Debug.Log("Teleport on cooldown");
            return; // 쿨타임 중이면 순간이동하지 않음
        }

        // 입력된 방향 벡터를 가져옵니다.
        Vector2 teleportDirection = inputVec.normalized; // 현재 입력 방향

        // 입력이 없으면 기본 방향을 플레이어가 바라보는 방향으로 설정
        if (teleportDirection == Vector2.zero)
        {
            teleportDirection = transform.up; // 기본적으로 플레이어가 바라보는 방향
        }

        // 순간이동
        Vector2 newPosition = (Vector2)transform.position + teleportDirection * teleportDistance; // 새로운 위치 계산

        UnityEngine.Debug.Log($"New Position: {newPosition}");

        rigid.MovePosition(newPosition); // Rigidbody2D를 사용하여 이동

        isTeleporting = true; // 순간이동 상태 플래그 설정

        lastTeleportTime = Time.time; // 마지막 순간이동 시간 업데이트
    }
}
