using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    public List<int> excludedIndices;
    private List<int> availableIndices;

    

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
        excludedIndices = new List<int>();
        availableIndices = new List<int>(); // 초기화
    }

    void Start()
    {
        InitializeAvailableIndicesFromAchievements(); // 게임 시작 시 업적에 따라 인덱스 초기화
    }

    public void InitializeAvailableIndicesFromAchievements()
    {
        AchieveManager achieveManager = FindObjectOfType<AchieveManager>();
        if (achieveManager != null)
        {
            foreach (int achievedIndex in achieveManager.GetUnlockedAchievementsIndices())
            {
                InitializeAvailableIndices(achievedIndex); // 업적에 따라 인덱스 추가
            }
        }
    }

    public void InitializeAvailableIndices(int achievedIndex)
    {
        // 업적에 따라 availableIndices에 인덱스 추가
        if (!availableIndices.Contains(achievedIndex))
        {
            availableIndices.Add(achievedIndex);
            UnityEngine.Debug.Log("Added to availableIndices: " + achievedIndex);
        }
    }

    //레벨업시 팝업 화면
    public void Show()
    {
        // 초기 제외 인덱스 목록
        excludedIndices.Clear();
        excludedIndices = new List<int> { 5, 6, 7, 8, 9, 10, 11, 12 }; // 기본 제외 목록

        // 업적에 따라 제외 목록 업데이트는 하지 않음
        // AchieveManager achieveManager = FindObjectOfType<AchieveManager>();
        // excludedIndices.AddRange(achieveManager.GetUnlockedAchievementsIndices()); // 이 부분도 제거

        // 중복된 인덱스 제거
        excludedIndices = new List<int>(new HashSet<int>(excludedIndices));

        UnityEngine.Debug.Log("Excluded Indices: " + string.Join(", ", excludedIndices));

        List<int> additionalIndices = new List<int>();

        Next(additionalIndices); // excludedIndices는 Show 메소드에서만 사용
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        // 레벨업 사운드 재생
        SoundManager.Instance.PlayLevelUpSound();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        // 선택 시 사운드 재생
        SoundManager.Instance.PlaySelectSound();
    }

    public void Select(int index)
    {
        if (index >= 0 && index < items.Length)
        {
            items[index].onclick();
        }
        else
        {
            UnityEngine.Debug.LogError("Index is out of range: " + index);
        }
    }

    public void Next(List<int> additionalIndices)
    {
        // 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 이전 availableIndices에 추가된 인덱스와 새로운 인덱스 합치기
        foreach (var index in additionalIndices)
        {
            if (!availableIndices.Contains(index))
            {
                availableIndices.Add(index); // 새로운 인덱스를 추가
            }
        }

        // 랜덤하게 선택할 아이템 인덱스를 저장할 리스트
        List<int> currentAvailableIndices = new List<int>();

        // 제외할 인덱스를 제외하고 나머지 인덱스 추가
        for (int i = 0; i < items.Length; i++)
        {
            if (!excludedIndices.Contains(i) || i < 3) // 0, 1, 2 인덱스는 항상 포함
            {
                currentAvailableIndices.Add(i);
            }
        }

        // availableIndices에 있는 인덱스 추가
        currentAvailableIndices.AddRange(availableIndices);

        UnityEngine.Debug.Log("additionalIndices Indices: " + string.Join(", ", additionalIndices));
        UnityEngine.Debug.Log("available Indices: " + string.Join(", ", currentAvailableIndices));

        // 무작위로 3개 선택
        HashSet<int> randomIndices = new HashSet<int>();
        while (randomIndices.Count < 3 && randomIndices.Count < currentAvailableIndices.Count)
        {
            int randomIndex = UnityEngine.Random.Range(0, currentAvailableIndices.Count);
            randomIndices.Add(currentAvailableIndices[randomIndex]);
        }

        foreach (int index in randomIndices)
        {
            Item ranItem = items[index];
            // 만렙 아이템의 경우 소비아이템으로 대체
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }

    public void AddExcludedIndex(int index)
    {
        // 이미 제외 목록에 있는지 확인
        if (!excludedIndices.Contains(index))
        {
            excludedIndices.Add(index);
            UnityEngine.Debug.Log("Excluded index added: " + index);
        }
        else
        {
            UnityEngine.Debug.Log("Index already excluded: " + index); // 이미 제외된 인덱스 로그
        }
    }
}