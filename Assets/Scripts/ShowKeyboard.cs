using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class ShowKeyboard : MonoBehaviour
{
    private TMP_InputField inputField;

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();

        // 입력 필드 선택 시 키보드 열기
        inputField.onSelect.AddListener(x => OpenKeyboard()); //?

        // 엔터 입력 시 줄바꿈
        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
    }

    public void OpenKeyboard()
    {
        NonNativeKeyboard.Instance.InputField = inputField;
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);

        // 위치 재배치 제거 (기존 위치 유지)
        // NonNativeKeyboard.Instance.RepositionKeyboard(...); ← 제거

        SetCaretColorAlpha(1);

        // 키보드 닫힘 방지 (이벤트 무시 or 제거)
        NonNativeKeyboard.Instance.OnClosed -= Instance_OnClosed;
        // NonNativeKeyboard.Instance.OnClosed += Instance_OnClosed; ← 주석 처리 또는 제거
    }

    private void Instance_OnClosed(object sender, System.EventArgs e)
    {
        // 닫히지 않도록 아무 것도 하지 않음
        // 또는 아예 등록하지 않음
    }

    public void SetCaretColorAlpha(float value)
    {
        inputField.customCaretColor = true;
        Color caretColor = inputField.caretColor;
        caretColor.a = value;
        inputField.caretColor = caretColor;
    }
}
