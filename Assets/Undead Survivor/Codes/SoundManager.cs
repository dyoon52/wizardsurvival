using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // 싱글톤 인스턴스
    public AudioSource backgroundSource; // 배경음
    public List<AudioSource> effectSources; // 효과음 리스트

    private void Awake()
    {
        // 인스턴스가 이미 존재하는지 확인
        if (Instance == null)
        {
            Instance = this; // 인스턴스 설정
            InitializeAudioSources(); // 오디오 소스 초기화
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 현재 객체 파괴
        }
    }

    private void InitializeAudioSources()
    {
        // 배경음 볼륨 초기화
        if (backgroundSource != null)
        {
            backgroundSource.volume = 0.5f; // 배경음 초기 볼륨
        }

        // 효과음 소스 볼륨 초기화
        foreach (AudioSource source in effectSources)
        {
            source.volume = 0.5f; // 각 효과음 초기 볼륨
        }
    }

    public void SetBackgroundVolume(float volume)
    {
        backgroundSource.volume = volume; // 배경음 볼륨 조절
    }

    public void SetEffectsVolume(float volume)
    {
        foreach (AudioSource source in effectSources)
        {
            source.volume = volume; // 각 효과음 볼륨 조절
        }
    }

    public void AddEffectSource(AudioSource newSource)
    {
        if (!effectSources.Contains(newSource))
        {
            effectSources.Add(newSource);
            newSource.volume = 0.5f; // 새로 추가된 효과음 소스 볼륨 초기화
        }
    }

    public void RemoveEffectSource(AudioSource sourceToRemove)
    {
        if (effectSources.Contains(sourceToRemove))
        {
            effectSources.Remove(sourceToRemove);
        }
    }


    // BGM 사운드 재생 메서드
    public void PlayBackgroundMusic()
    {
        if (!backgroundSource.isPlaying) // BGM이 재생 중이지 않은 경우
        {
            backgroundSource.Play(); // BGM 재생
        }
    }

    // 피격 사운드 재생 메서드 (몬스터, 플레이어 둘다 사용)
    public void PlayMonsterHitSound()
    {
        if (effectSources.Count > 0) // 리스트에 사운드가 있는지 확인
        {
            effectSources[0].Play(); // 0번 인덱스 사운드 재생
        }
    }

    // 몬스터 죽음 사운드 재생 메서드
    public void PlayMonsterDeathSound()
    {
        if (effectSources.Count > 1) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[1].Play(); // 1번 인덱스 사운드 재생
        }
    }

    // 레벨업 사운드 재생 메서드
    public void PlayLevelUpSound()
    {
        if (effectSources.Count > 2 && !effectSources[2].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[2].Play(); // 2번 인덱스 사운드 재생
        }
    }

    // 선택 사운드 재생 메서드
    public void PlaySelectSound()
    {
        if (effectSources.Count > 3 && !effectSources[3].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[3].Play(); // 3번 인덱스 사운드 재생
        }
    }

    // 승리 사운드 재생 메서드
    public void PlayVictorySound()
    {
        if (effectSources.Count > 4 && !effectSources[4].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[4].Play(); // 4번 인덱스 사운드 재생
        }
    }

    // 패배 사운드 재생 메서드
    public void PlayDefeatSound()
    {
        if (effectSources.Count > 5 && !effectSources[5].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[5].Play(); // 5번 인덱스 사운드 재생
        }
    }

    // 마법 사운드
    // 근접 공격 사운드 재생 메서드
    public void PlayMeleeSound()
    {
        if (effectSources.Count > 6 && !effectSources[6].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[6].Play(); // 6번 인덱스 사운드 재생
        }
    }

    // 원거리 공격 사운드 재생 메서드
    public void PlayRangedSound()
    {
        if (effectSources.Count > 7) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[7].Play(); // 7번 인덱스 사운드 재생
        }
    }

    // 메테오 사운드 재생 메서드
    public void PlayMeteorSound()
    {
        if (effectSources.Count > 8 && !effectSources[8].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[8].Play(); // 8번 인덱스 사운드 재생
        }
    }

    // 얼음 마법진 사운드 재생 메서드
    public void PlayIceCircleSound()
    {
        if (effectSources.Count > 9 && !effectSources[9].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[9].Play(); // 9번 인덱스 사운드 재생
        }
    }

    // 볼트 사운드 재생 메서드
    public void PlayVoltSound()
    {
        if (effectSources.Count > 10 && !effectSources[10].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[10].Play(); // 10번 인덱스 사운드 재생
        }
    }

    // 파이어워크 사운드 재생 메서드
    public void PlayFireworkSound()
    {
        if (effectSources.Count > 11 && !effectSources[11].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[11].Play(); // 11번 인덱스 사운드 재생
        }
    }

    // 블리자드 사운드 재생 메서드
    public void PlayBlizzardSound()
    {
        if (effectSources.Count > 12 && !effectSources[12].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[12].Play(); // 12번 인덱스 사운드 재생
        }
    }

    // 블랙볼 사운드 재생 메서드
    public void PlayBlackBallSound()
    {
        if (effectSources.Count > 13) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[13].Play(); // 13번 인덱스 사운드 재생
        }
    }

    // 블랙홀 사운드 재생 메서드
    public void PlayBlackHoleSound()
    {
        if (effectSources.Count > 14 && !effectSources[14].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[14].Play(); // 14번 인덱스 사운드 재생
        }
    }

    // 완드 어택 사운드 재생 메서드
    public void PlayWandAttackSound()
    {
        if (effectSources.Count > 15 && !effectSources[15].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[15].Play(); // 15번 인덱스 사운드 재생
        }
    }

    // 텔레포트 사운드 재생 메서드
    public void PlayTeleportSound()
    {
        if (effectSources.Count > 16 && !effectSources[16].isPlaying) // 리스트에 사운드가 있고, 재생 중이지 않은 경우
        {
            effectSources[16].Play(); // 16번 인덱스 사운드 재생
        }
    }
}