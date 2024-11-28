using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public Camera mainCamera;
    public EdgeCollider2D topBorder;
    public EdgeCollider2D bottomBorder;
    public EdgeCollider2D leftBorder;
    public EdgeCollider2D rightBorder;

    void Start()
    {
        UpdateBorders();
    }

    void UpdateBorders()
    {
        if (mainCamera == null) return;

        // 카메라의 월드 좌표에서의 모서리 포인트 계산
        Vector2 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector2 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        Vector2 topLeft = new Vector2(bottomLeft.x, topRight.y);
        Vector2 bottomRight = new Vector2(topRight.x, bottomLeft.y);

        // EdgeCollider의 포인트를 업데이트하여 화면 테두리를 설정
        topBorder.SetPoints(new List<Vector2> { topLeft, topRight });
        bottomBorder.SetPoints(new List<Vector2> { bottomLeft, bottomRight });
        leftBorder.SetPoints(new List<Vector2> { bottomLeft, topLeft });
        rightBorder.SetPoints(new List<Vector2> { bottomRight, topRight });
    }
}

