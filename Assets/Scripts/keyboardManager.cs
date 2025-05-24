using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System.Collections;

public class KeyboardManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    void Start()
    {
        if (inputField == null)
            inputField = GetComponent<TMP_InputField>();

        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;

        inputField.onSelect.AddListener(_ => OpenKeyboard());

        StartCoroutine(FocusAndShowKeyboardNextFrame());
    }

    private IEnumerator FocusAndShowKeyboardNextFrame()
    {
        yield return null;
        OpenKeyboard();
    }

    private void OpenKeyboard()
    {
        if (inputField == null) return;

        NonNativeKeyboard.Instance.InputField = inputField;
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);
        NonNativeKeyboard.Instance.OnClosed -= DoNothingOnKeyboardClosed;

        SetCaretAlpha(1);
    }

    private void DoNothingOnKeyboardClosed(object sender, System.EventArgs e)
    {
        // 아무 것도 하지 않음
    }

    private void SetCaretAlpha(float alpha)
    {
        inputField.customCaretColor = true;
        var caretColor = inputField.caretColor;
        caretColor.a = alpha;
        inputField.caretColor = caretColor;
    }

}
