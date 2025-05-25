using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticTrigger : MonoBehaviour
{
    [Header("���� ��Ʈ�ѷ�")]
    public XRBaseController leftController;

    [Header("������ ��Ʈ�ѷ�")]
    public XRBaseController rightController;

    [Header("���� ����")]
    public float intensity = 0.5f;       // ���� (0.0 ~ 1.0)
    public float duration = 0.2f;        // ���� �ð� (��)

    public void TriggerHaptic()
    {
        if (leftController != null)
        {
            leftController.SendHapticImpulse(intensity, duration);
        }

        if (rightController != null)
        {
            rightController.SendHapticImpulse(intensity, duration);
        }
    }
}
