using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    private Vector2 direction; // 이동 방향

    void Start()
    {
        // 플레이어의 현재 위치를 가져와서 방향 설정
        Transform target = GameManager.instance.player.transform;
        direction = (target.position - transform.position).normalized; // 방향 계산
    }

    void Update()
    {
        // 방향으로 이동
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
}