using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticTrigger : MonoBehaviour
{
    [Header("왼쪽 컨트롤러")]
    public XRBaseController leftController;

    [Header("오른쪽 컨트롤러")]
    public XRBaseController rightController;

    [Header("진동 설정")]
    public float intensity = 0.5f;       // 세기 (0.0 ~ 1.0)
    public float duration = 0.2f;        // 지속 시간 (초)

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
