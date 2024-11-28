using UnityEngine;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    public GameObject pauseMenu; // 일시 정지 메뉴
    private bool isPaused = false;

    void Start()
    {
        // 게임 시작 시 일시 정지 메뉴 비활성화
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape 키 입력됨"); // 로그 추가
            ToggleGamePause();
        }
    }

    public void ToggleGamePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0; // 게임 일시 정지
        pauseMenu.SetActive(true); // 메뉴 활성화
        Debug.Log("게임이 일시 정지되었습니다."); // 로그 추가
    }

    private void ResumeGame()
    {
        Time.timeScale = 1; // 게임 재개
        pauseMenu.SetActive(false); // 메뉴 비활성화
    }
}
