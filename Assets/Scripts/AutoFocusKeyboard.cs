using System.Collections;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class AutoFocusKeyboard : MonoBehaviour
{
    [Header("연결 대상 TMP InputField")]
    [SerializeField] private TMP_InputField inputField;

    [Header("VR 키보드 기준 위치 (예: Main Camera)")]
    [SerializeField] private Transform positionSource;

    [Header("키보드 거리/높이 설정")]
    [SerializeField] private float distance = 0.5f;
    [SerializeField] private float verticalOffset = -0.5f;

    void Start()
    {
        StartCoroutine(FocusAndShowKeyboardNextFrame());
    }

    private IEnumerator FocusAndShowKeyboardNextFrame()
    {
        yield return null; // 한 프레임 대기 후 실행

        if (inputField == null)
        {
            Debug.LogError("[AutoFocusKeyboard] inputField 연결 안됨");
            yield break;
        }

        if (positionSource == null && Camera.main != null)
        {
            positionSource = Camera.main.transform;
        }

        // 입력 필드에 포커스 설정
        inputField.Select();
        inputField.ActivateInputField();

        // 키보드 표시
        NonNativeKeyboard.Instance.InputField = inputField;
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);

        /*
        // 키보드 위치 재설정 (optional)
        if (positionSource != null)
        {
            Vector3 direction = positionSource.forward;
            direction.y = 0;
            direction.Normalize();
            Vector3 targetPosition = positionSource.position + direction * distance + Vector3.up * verticalOffset;

            NonNativeKeyboard.Instance.RepositionKeyboard(targetPosition);
        }
        */

        // 커서 표시
        SetCaretAlpha(1);
    }

    private void SetCaretAlpha(float alpha)
    {
        inputField.customCaretColor = true;
        var caretColor = inputField.caretColor;
        caretColor.a = alpha;
        inputField.caretColor = caretColor;
    }
}
