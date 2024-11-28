using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 5 * 10f; /*testtime*/
    [Header("# Player Info")]
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int bossKillCount;
    public int gameClearCount;
    public int totalKillCount;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 }; //text nextExp 나중에 조정 필요
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;
    public GamePause uiGamePause;


    void Awake()
    {
        instance = this;

        gameClearCount = PlayerPrefs.GetInt("GameClearCount", 0); // 기본값 0
        bossKillCount = PlayerPrefs.GetInt("BossKillCount", 0); // 기본값 0
        totalKillCount = PlayerPrefs.GetInt("TotalKillCount", 0);

    }


    public void GameStart()
    {
        health = maxHealth;
        UnityEngine.Debug.Log($"게임 시작! 현재 게임 클리어 횟수: {gameClearCount}, 보스 처치 횟수: {bossKillCount}, 누적 킬 수: { totalKillCount}, 킬 수 {kill}");
        
        //text용
        uiLevelUp.Select(12);

        Resume();

        SoundManager.Instance.PlaySelectSound();
        SoundManager.Instance.PlayBackgroundMusic();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();

        Stop();

        SoundManager.Instance.PlayDefeatSound();
    }
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        gameClearCount++;

        PlayerPrefs.SetInt("GameClearCount", gameClearCount);
        PlayerPrefs.Save();

        SoundManager.Instance.PlayVictorySound();
    }
    public void GameRetry()
    {
        SceneManager.LoadScene(0); //파일> 빌드설정 확인시 0번 있음
    }

   

    public void OnBossDefeated()
    {
        bossKillCount++;

        PlayerPrefs.SetInt("BossKillCount", bossKillCount);
        PlayerPrefs.Save();
    }


  

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiGamePause.ToggleGamePause(); // GamePause 상태 전환
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

    public void ShowBossDefeatedNotification()
    {
        UnityEngine.Debug.Log("보스 처치 알림을 표시합니다!"); // 디버그 출력을 통해 확인 가능
    }
}
