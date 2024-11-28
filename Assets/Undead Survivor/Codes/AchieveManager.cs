
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockMagic;
    public GameObject[] unlockMagic;

    enum Achieve { UnlockShotgun, UnlockCircle, UnlockVolt, UnlockFireWalk, UnlockRain, UnlockTeleport, UnlockBlack, UnlockBlackHole }
    Achieve[] achieves;

    void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockMagic();
        LoadAchievements(); // 게임 시작 시 업적을 로드
    }

    void UnlockMagic()
    {
        for (int index = 0; index < lockMagic.Length; index++)
        {
            string achieveName = achieves[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            lockMagic[index].SetActive(!isUnlock);
            unlockMagic[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach (Achieve achieve in achieves)
        {
            // 업적이 이미 달성된 경우 체크하지 않음
            if (PlayerPrefs.GetInt(achieve.ToString()) == 0)
            {
                CheckAchieve(achieve);
            }
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchive = false;

        switch (achieve)
        {
            case Achieve.UnlockCircle:
                isAchive = GameManager.instance.kill >= 100;
                break;
            case Achieve.UnlockRain:
                isAchive = GameManager.instance.gameClearCount >= 5;
                break;
            case Achieve.UnlockFireWalk:
                isAchive = GameManager.instance.totalKillCount >= 500;
                break;
            case Achieve.UnlockShotgun:
                isAchive = GameManager.instance.bossKillCount >= 1;
                break;
            case Achieve.UnlockVolt:
                isAchive = GameManager.instance.bossKillCount >= 5;
                break;
            case Achieve.UnlockTeleport:
                isAchive = GameManager.instance.gameClearCount >= 1;
                break;
            case Achieve.UnlockBlack:
                isAchive = GameManager.instance.kill >= 500;
                break;
            case Achieve.UnlockBlackHole:
                isAchive = GameManager.instance.totalKillCount >= 3000;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);
            UnityEngine.Debug.Log("Achievement unlocked: " + achieve.ToString());
        }
    }

    private void LoadAchievements()
    {
        LevelUp levelUp = FindObjectOfType<LevelUp>(); // LevelUp 객체를 찾아서
        Player player = FindObjectOfType<Player>();

        foreach (Achieve achieve in achieves)
        {
            if (PlayerPrefs.GetInt(achieve.ToString()) == 1)
            {
                int indexToAdd = -1; // 추가할 인덱스 초기화

                // 업적에 따라 추가할 인덱스 결정
                switch (achieve)
                {
                    case Achieve.UnlockShotgun:
                        indexToAdd = 5; // Shotgun 업적에 대해 인덱스 5 추가
                        break;
                    case Achieve.UnlockCircle:
                        indexToAdd = 6; // Circle 업적에 대해 인덱스 6 추가
                        break;
                    case Achieve.UnlockVolt:
                        indexToAdd = 7; // Volt 업적에 대해 인덱스 7 추가
                        break;
                    case Achieve.UnlockFireWalk:
                        indexToAdd = 8; // FireWalk 업적에 대해 인덱스 8 추가
                        break;
                    case Achieve.UnlockRain:
                        indexToAdd = 9; // Rain 업적에 대해 인덱스 9 추가
                        break;
                    case Achieve.UnlockTeleport:
                        if (player != null)
                        {
                            player.canTeleport = true; // 순간이동 기능 활성화
                        }
                        break;
                    case Achieve.UnlockBlack:
                        indexToAdd = 10; // Black 업적에 대해 인덱스 11 추가
                        break;
                    case Achieve.UnlockBlackHole:
                        indexToAdd = 11; // BlackHole 업적에 대해 인덱스 12 추가
                        break;
                }

                // 추가할 인덱스가 결정되었다면 LevelUp에서 처리
                if (indexToAdd != -1 && levelUp != null)
                {
                    levelUp.InitializeAvailableIndices(indexToAdd); // 업적에 따라 인덱스 추가
                }
            }
        }
    }

    public List<int> GetUnlockedAchievementsIndices()
    {
        List<int> unlockedIndices = new List<int>();

        foreach (Achieve achieve in achieves)
        {
            if (PlayerPrefs.GetInt(achieve.ToString()) == 1)
            {
                unlockedIndices.Add((int)achieve);
            }
        }

        return unlockedIndices;
    }
}
