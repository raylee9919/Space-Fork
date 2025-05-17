using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class XRCharacterMover : MonoBehaviour
{
    public InputActionProperty moveAction; // XR Input의 2D Vector

    public float moveSpeed = 1.5f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y);

        // HMD 카메라 방향을 기준으로 이동 방향 변환
        Transform cameraTransform = Camera.main.transform;
        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

        Vector3 move = (forward * direction.z + right * direction.x) * moveSpeed;

        // 중력 적용 안 하면 공중에 뜰 수 있으므로 아래 보정 추가 가능
        move += Physics.gravity * Time.deltaTime;

        controller.Move(move * Time.deltaTime);
    }
}
