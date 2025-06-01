using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using System.Collections;

public class XRInitializer : MonoBehaviour
{
    [Header("설정")]
    public bool enableOnStart = true;

    void Start()
    {
        // 씬 전환 시에도 XR 상태 유지
        DontDestroyOnLoad(gameObject);

        if (enableOnStart)
        {
            InitializeXRSync();
        }
    }

    public void InitializeXRSync()
    {
        Debug.Log("XR 초기화 시작");

        var xrManager = XRGeneralSettings.Instance?.Manager;
        if (xrManager == null)
        {
            Debug.LogError("XRGeneralSettings.Instance.Manager가 null입니다.");
            return;
        }

        // 이미 초기화되고 활성화되어 있다면 스킵
        if (xrManager.isInitializationComplete && XRSettings.isDeviceActive)
        {
            Debug.Log("XR이 이미 초기화되고 활성화되어 있습니다.");
            return;
        }

        Debug.Log("XR Device Active (초기화 전): " + XRSettings.isDeviceActive);
        Debug.Log("XR Device Name (초기화 전): " + XRSettings.loadedDeviceName);

        // 강제 XR 초기화
        if (!xrManager.isInitializationComplete)
        {
            Debug.Log("XR Manager not initialized. Initializing now...");
            xrManager.InitializeLoaderSync();
        }

        if (xrManager.activeLoader != null)
        {
            Debug.Log("Active Loader: " + xrManager.activeLoader.name);

            // 서브시스템 시작
            xrManager.StartSubsystems();

            // 추가: Display 서브시스템 강제 시작
            var displaySubsystem = xrManager.activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
            if (displaySubsystem != null)
            {
                if (!displaySubsystem.running)
                {
                    displaySubsystem.Start();
                    Debug.Log("Display Subsystem 시작됨");
                }
            }

            // 추가: Input 서브시스템 강제 시작  
            var inputSubsystem = xrManager.activeLoader.GetLoadedSubsystem<XRInputSubsystem>();
            if (inputSubsystem != null)
            {
                if (!inputSubsystem.running)
                {
                    inputSubsystem.Start();
                    Debug.Log("Input Subsystem 시작됨");
                }
                inputSubsystem.TryRecenter();
                Debug.Log("XR Recentered.");
            }
        }
        else
        {
            Debug.LogError("Active Loader가 null입니다.");
        }

        // 최종 상태 확인
        Debug.Log("XR Device Active (초기화 후): " + XRSettings.isDeviceActive);
        Debug.Log("XR Device Name (초기화 후): " + XRSettings.loadedDeviceName);
        Debug.Log("XR Manager Initialized: " + xrManager.isInitializationComplete);

        if (XRSettings.isDeviceActive)
        {
            Debug.Log("XR 디바이스 활성화 성공!");
            ConfigureXRSettings();
        }
        else
        {
            Debug.LogError("XR 디바이스 활성화 실패");

            // 추가 강제 시도
            Debug.Log("강제 XR 활성화 시도 중...");
            StartCoroutine(ForceActivateXR());
        }
    }

    private void ConfigureXRSettings()
    {
        // 렌더링 스케일 설정 (성능 최적화)
        XRSettings.eyeTextureResolutionScale = 1.0f;

        // 렌더 뷰포트 스케일 설정
        XRSettings.renderViewportScale = 1.0f;

        // 추가: XR 카메라 추적 활성화
        var mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // TrackedPoseDriver 컴포넌트 확인/추가
            var trackedPoseDriver = mainCamera.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
            if (trackedPoseDriver == null)
            {
                trackedPoseDriver = mainCamera.gameObject.AddComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
                Debug.Log("TrackedPoseDriver 추가됨");
            }

            // 추적 설정
            trackedPoseDriver.SetPoseSource(UnityEngine.SpatialTracking.TrackedPoseDriver.DeviceType.GenericXRDevice,
                                          UnityEngine.SpatialTracking.TrackedPoseDriver.TrackedPose.Center);
            trackedPoseDriver.trackingType = UnityEngine.SpatialTracking.TrackedPoseDriver.TrackingType.RotationAndPosition;
            trackedPoseDriver.updateType = UnityEngine.SpatialTracking.TrackedPoseDriver.UpdateType.UpdateAndBeforeRender;

            Debug.Log("XR 카메라 추적 설정 완료");
        }

        Debug.Log("XR 설정 구성 완료");
    }

    // 수동으로 XR 초기화하는 메서드 (버튼 클릭 등에 사용)
    public void ManualInitializeXR()
    {
        InitializeXRSync();
    }

    // XR 종료 메서드
    public void DeinitializeXR()
    {
        var xrManager = XRGeneralSettings.Instance?.Manager;
        if (xrManager != null && xrManager.isInitializationComplete)
        {
            Debug.Log("XR 종료 중...");
            xrManager.StopSubsystems();
            xrManager.DeinitializeLoader();
            Debug.Log("XR 종료 완료");
        }
    }

    // 강제 XR 활성화 시도
    IEnumerator ForceActivateXR()
    {
        var xrManager = XRGeneralSettings.Instance?.Manager;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.5f);

            if (xrManager != null && xrManager.activeLoader != null)
            {
                xrManager.StopSubsystems();
                yield return new WaitForSeconds(0.1f);
                xrManager.StartSubsystems();

                Debug.Log("강제 재시작 시도 " + (i + 1) + "/10 - XR Device Active: " + XRSettings.isDeviceActive);

                if (XRSettings.isDeviceActive)
                {
                    Debug.Log("강제 활성화 성공!");
                    ConfigureXRSettings();
                    yield break;
                }
            }
        }
        Debug.LogError("강제 활성화도 실패했습니다.");
    }
}