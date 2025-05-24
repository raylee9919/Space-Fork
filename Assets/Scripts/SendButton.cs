using UnityEngine;

public class SendButton : MonoBehaviour
{
    [Header("Evaluate() 함수가 붙은 오브젝트")]
    [SerializeField] private CodeEvaluator codeEvaluator;

    public void SendCode()
    {
        if (codeEvaluator == null)
        {
            return;
        }

        codeEvaluator.Evaluate();
    }
}
