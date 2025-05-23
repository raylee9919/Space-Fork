using UnityEngine;

public class SendButton : MonoBehaviour
{
    [Header("Evaluate() 함수가 붙은 오브젝트")]
    [SerializeField] private CodeEvaluator codeEvaluator;

    public void SendCode()
    {
        if (codeEvaluator == null)
        {
            Debug.LogError("[SendButton] CodeEvaluator가 연결되어 있지 않습니다.");
            return;
        }

        codeEvaluator.Evaluate();
    }
}
