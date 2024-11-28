using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class NoticeArtifactManager : MonoBehaviour
{
    public GameObject[] Artifact; // 유물 오브젝트 배열
    public GameObject playerObject; // Player 컴포넌트를 포함하는 GameObject

    private void Start()
    {
        // 모든 유물 오브젝트 비활성화
        foreach (GameObject artifact in Artifact)
        {
            artifact.SetActive(false);
        }
    }

    public void ActivateRelic(int bossIndex)
    {
        if (bossIndex >= 0 && bossIndex < Artifact.Length)
        {
            Artifact[bossIndex].SetActive(true);
            UnityEngine.Debug.Log($"유물 {bossIndex}가 활성화되었습니다.");
            OnArtifactActivated(bossIndex);
            StartCoroutine(DeactivateRelicAfterDelay(Artifact[bossIndex], 2f));
        }
        else
        {
            UnityEngine.Debug.LogError($"유효하지 않은 bossIndex: {bossIndex}");
        }
    }

    private IEnumerator DeactivateRelicAfterDelay(GameObject relic, float delay)
    {
        yield return new WaitForSeconds(delay);
        relic.SetActive(false);
        UnityEngine.Debug.Log($"유물 {relic.name}가 비활성화되었습니다.");
    }

    public void OnArtifactActivated(int bossIndex)
    {
        UnityEngine.Debug.Log("OnArtifactActivated 호출됨");

        if (bossIndex == 0) // 0번 유물 활성화 시
        {
            // Player 객체에서 Weapon5를 찾기
            Transform weaponTransform = playerObject.transform.Find("Weapon5");
            if (weaponTransform != null)
            {
                Weapon weapon = weaponTransform.GetComponent<Weapon>();
                if (weapon != null)
                {
                    weapon.SetMeteorCooldown(3.0f);
                    UnityEngine.Debug.Log("MeteorCooldown이 3.0f로 변경되었습니다.");
                }
                else
                {
                    UnityEngine.Debug.LogError("Weapon 컴포넌트를 찾을 수 없습니다.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Weapon5 이름을 가진 GameObject를 찾을 수 없습니다.");
            }
        }
        else if (bossIndex == 1) // 1번 유물 활성화 시
        {
            // Player 객체에서 Weapon10을 찾기
            Transform weaponTransform = playerObject.transform.Find("Weapon10");
            if (weaponTransform != null)
            {
                Weapon weapon = weaponTransform.GetComponent<Weapon>();
                if (weapon != null)
                {
                    weapon.SetBlackCooldown(3.0f);
                    UnityEngine.Debug.Log("Weapon10의 발사간격이 30% 감소했습니다.");
                }
                else
                {
                    UnityEngine.Debug.LogError("Weapon 컴포넌트를 찾을 수 없습니다.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Weapon10 이름을 가진 GameObject를 찾을 수 없습니다.");
            }
        }
        else if (bossIndex == 2) // 2번 유물 활성화 시
        {
            // Player 객체에서 Weapon9을 찾기
            Transform weaponTransform = playerObject.transform.Find("Weapon9");
            if (weaponTransform != null)
            {
                Weapon weapon = weaponTransform.GetComponent<Weapon>();
                if (weapon != null)
                {
                    // 데미지를 50% 증가
                    weapon.SetRainCooldown(10.0f);
                    weapon.SetRainCount(20);
                    UnityEngine.Debug.Log("Weapon9의 발사 간격이 1초 증가, 투사체 개수 2배증가");
                }
                else
                {
                    UnityEngine.Debug.LogError("Weapon 컴포넌트를 찾을 수 없습니다.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Weapon0 이름을 가진 GameObject를 찾을 수 없습니다.");
            }
        }
        else if (bossIndex == 3) // 3번 유물 활성화 시
        {
            // Player 객체에서 Weapon1을 찾기
            Transform weaponTransform = playerObject.transform.Find("Weapon1");
            if (weaponTransform != null)
            {
                Weapon weapon = weaponTransform.GetComponent<Weapon>();
                if (weapon != null)
                {
                    // 데미지를 50% 증가
                    weapon.damage *= 1.5f; // damage가 float 타입이라고 가정
                    UnityEngine.Debug.Log("Weapon1의 데미지가 50% 증가했습니다.");
                }
                else
                {
                    UnityEngine.Debug.LogError("Weapon 컴포넌트를 찾을 수 없습니다.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Weapon1 이름을 가진 GameObject를 찾을 수 없습니다.");
            }
        }
    }
}