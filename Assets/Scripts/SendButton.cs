using UnityEngine;

public class SendButton : MonoBehaviour
{
    [Header("Evaluate() �Լ��� ���� ������Ʈ")]
    [SerializeField] private CodeEvaluator codeEvaluator;
    public HapticTrigger hapticTrigger;

    public void SendCode()
    {
        if (codeEvaluator == null)
        {
            return;
        }

        hapticTrigger.TriggerHaptic();
        codeEvaluator.Evaluate();
    }
}
