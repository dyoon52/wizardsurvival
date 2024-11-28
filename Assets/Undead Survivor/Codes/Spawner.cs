using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData; // 일반 몬스터의 능력치 데이터
    public SpawnData[] bossSpawnData; // 보스 몬스터의 능력치 데이터
    public SpawnData[] bulletSpawnData; // 투사체 몬스터의 능력치 데이터
    public float levelTime;

    private int level;
    private float timer;
    private int bossSpawnCount; // 소환된 보스 수

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        bossSpawnCount = 0; // 보스 초기 소환 횟수
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.FloorToInt(GameManager.instance.gameTime / levelTime);

        // 일반 몬스터 소환
        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }

        // 보스 몬스터 소환 로직
        if (bossSpawnCount < bossSpawnData.Length && GameManager.instance.gameTime >= BossLevelTime(bossSpawnCount))
        {
            SpawnBoss(bossSpawnCount);
            bossSpawnCount++; // 보스 소환 횟수 증가
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }

    void SpawnBoss(int bossIndex)
    {
        GameObject boss = GameManager.instance.pool.Get(1); // 보스 몬스터를 풀에서 가져옴

        // 랜덤한 소환 위치 설정
        int randomIndex = UnityEngine.Random.Range(1, spawnPoint.Length); // 1부터 spawnPoint.Length - 1까지의 랜덤 인덱스 선택
        boss.transform.position = spawnPoint[randomIndex].position; // 랜덤 위치에 소환

        // 보스의 능력치 초기화
        boss.GetComponent<Enemy>().Init(bossSpawnData[bossIndex]); // 보스의 능력치 설정

        // 보스가 소환될 때 Bullet Enemy를 소환
        StartCoroutine(SpawnBulletEnemies(boss.transform));
    }

    private IEnumerator SpawnBulletEnemies(Transform bossTransform)
    {
        Enemy bossEnemy = bossTransform.GetComponent<Enemy>(); // 보스의 Enemy 컴포넌트 가져오기

        while (bossEnemy.isLive)
        {
            UnityEngine.Debug.Log("Boss is alive: " + bossEnemy.isLive); // 상태 로그

            for (int i = 0; i < 3; i++)
            {
                GameObject bulletEnemy = GameManager.instance.pool.Get(2);

                if (bulletEnemy == null)
                {
                    UnityEngine.Debug.LogWarning("Bullet enemy is null. Check the pool.");
                    yield break; // 코루틴 종료
                }

                bulletEnemy.transform.SetParent(bossTransform);
                bulletEnemy.transform.position = bossTransform.position;

                Vector2 directionToPlayer = (GameManager.instance.player.transform.position - bulletEnemy.transform.position).normalized;
                bulletEnemy.GetComponent<Rigidbody2D>().velocity = directionToPlayer * 10;

                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(2f);
        }

        UnityEngine.Debug.Log("Boss has died."); // 적이 죽었을 때 로그
    }

    float BossLevelTime(int index)
    {
        return GameManager.instance.maxGameTime / bossSpawnData.Length * (index + 1);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}