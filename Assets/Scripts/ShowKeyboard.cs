using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class ShowKeyboard : MonoBehaviour
{
    // 텍스트 입력 필드 변수
    private TMP_InputField inputField;

    // 키보드와 사용자 사이의 거리
    public float distance = 0.5f;

    // 키보드의 수직 위치 오프셋
    public float verticalOffset = -0.5f;

    // 기준이 될 위치 (보통 카메라나 컨트롤러)
    public Transform positionSource;

    void Start()
    {
        // 컴포넌트에서 TMP_InputField 가져오기
        inputField = GetComponent<TMP_InputField>();

        // 입력 필드 선택 시 키보드 열기 이벤트 등록
        inputField.onSelect.AddListener(x => OpenKeyboard());
    }

    // 키보드를 여는 함수
    public void OpenKeyboard()
    {
        // NonNativeKeyboard에 현재 입력 필드 연결
        NonNativeKeyboard.Instance.InputField = inputField;

        // 현재 텍스트 상태로 키보드 표시
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);

        // 카메라나 기준 방향에서 키보드 위치 계산
        Vector3 direction = positionSource.forward;
        direction.y = 0; // 수평 방향만 사용
        direction.Normalize();

        // 거리와 오프셋을 적용하여 키보드 위치 설정
        Vector3 targetPosition = positionSource.position + direction * distance + Vector3.up * verticalOffset;

        // 키보드 위치 재배치
        NonNativeKeyboard.Instance.RepositionKeyboard(targetPosition);

        // 커서(캐럿)의 알파값을 1로 설정하여 표시
        SetCaretColorAlpha(1);

        // 키보드가 닫힐 때 호출될 이벤트 등록
        NonNativeKeyboard.Instance.OnClosed += Instance_OnClosed;
    }

    // 키보드 닫힘 이벤트 처리 함수
    private void Instance_OnClosed(object sender, System.EventArgs e)
    {
        // 커서(캐럿) 감추기 < 왜 감추지?
        SetCaretColorAlpha(1);

        // 이벤트 핸들러 제거
        NonNativeKeyboard.Instance.OnClosed -= Instance_OnClosed;
    }

    // 커서(캐럿)의 투명도 설정 함수
    public void SetCaretColorAlpha(float value)
    {
        inputField.customCaretColor = true; // 사용자 정의 캐럿 색상 사용
        Color caretColor = inputField.caretColor;
        caretColor.a = value; // 알파값 변경
        inputField.caretColor = caretColor; // 색상 적용
    }
}
