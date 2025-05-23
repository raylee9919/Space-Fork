using System.Collections;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class AutoFocusKeyboard : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    void Start()
    {
        StartCoroutine(FocusAndShowKeyboardNextFrame());
    }

    private IEnumerator FocusAndShowKeyboardNextFrame()
    {
        yield return null;

        if (inputField == null)
        {
            Debug.LogError("[AutoFocusKeyboard] inputField 연결 안됨");
            yield break;
        }

        // 엔터 입력 시 줄바꿈
        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;

        inputField.Select();
        inputField.ActivateInputField();

        NonNativeKeyboard.Instance.InputField = inputField;
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);

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
