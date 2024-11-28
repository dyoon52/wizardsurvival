using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenManager : MonoBehaviour
{
    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen; // 전체 화면 상태 토글
        
        SoundManager.Instance.PlaySelectSound();
    }
}
