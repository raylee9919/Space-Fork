using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using System.Collections;

public class XRInitializer : MonoBehaviour
{
    [Header("����")]
    public bool enableOnStart = true;

    void Start()
    {
        // �� ��ȯ �ÿ��� XR ���� ����
        DontDestroyOnLoad(gameObject);

        if (enableOnStart)
        {
            InitializeXRSync();
        }
    }

    public void InitializeXRSync()
    {
        Debug.Log("XR �ʱ�ȭ ����");

        var xrManager = XRGeneralSettings.Instance?.Manager;
        if (xrManager == null)
        {
            Debug.LogError("XRGeneralSettings.Instance.Manager�� null�Դϴ�.");
            return;
        }

        // �̹� �ʱ�ȭ�ǰ� Ȱ��ȭ�Ǿ� �ִٸ� ��ŵ
        if (xrManager.isInitializationComplete && XRSettings.isDeviceActive)
        {
            Debug.Log("XR�� �̹� �ʱ�ȭ�ǰ� Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
            return;
        }

        Debug.Log("XR Device Active (�ʱ�ȭ ��): " + XRSettings.isDeviceActive);
        Debug.Log("XR Device Name (�ʱ�ȭ ��): " + XRSettings.loadedDeviceName);

        // ���� XR �ʱ�ȭ
        if (!xrManager.isInitializationComplete)
        {
            Debug.Log("XR Manager not initialized. Initializing now...");
            xrManager.InitializeLoaderSync();
        }

        if (xrManager.activeLoader != null)
        {
            Debug.Log("Active Loader: " + xrManager.activeLoader.name);

            // ����ý��� ����
            xrManager.StartSubsystems();

            // �߰�: Display ����ý��� ���� ����
            var displaySubsystem = xrManager.activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
            if (displaySubsystem != null)
            {
                if (!displaySubsystem.running)
                {
                    displaySubsystem.Start();
                    Debug.Log("Display Subsystem ���۵�");
                }
            }

            // �߰�: Input ����ý��� ���� ����  
            var inputSubsystem = xrManager.activeLoader.GetLoadedSubsystem<XRInputSubsystem>();
            if (inputSubsystem != null)
            {
                if (!inputSubsystem.running)
                {
                    inputSubsystem.Start();
                    Debug.Log("Input Subsystem ���۵�");
                }
                inputSubsystem.TryRecenter();
                Debug.Log("XR Recentered.");
            }
        }
        else
        {
            Debug.LogError("Active Loader�� null�Դϴ�.");
        }

        // ���� ���� Ȯ��
        Debug.Log("XR Device Active (�ʱ�ȭ ��): " + XRSettings.isDeviceActive);
        Debug.Log("XR Device Name (�ʱ�ȭ ��): " + XRSettings.loadedDeviceName);
        Debug.Log("XR Manager Initialized: " + xrManager.isInitializationComplete);

        if (XRSettings.isDeviceActive)
        {
            Debug.Log("XR ����̽� Ȱ��ȭ ����!");
            ConfigureXRSettings();
        }
        else
        {
            Debug.LogError("XR ����̽� Ȱ��ȭ ����");

            // �߰� ���� �õ�
            Debug.Log("���� XR Ȱ��ȭ �õ� ��...");
            StartCoroutine(ForceActivateXR());
        }
    }

    private void ConfigureXRSettings()
    {
        // ������ ������ ���� (���� ����ȭ)
        XRSettings.eyeTextureResolutionScale = 1.0f;

        // ���� ����Ʈ ������ ����
        XRSettings.renderViewportScale = 1.0f;

        // �߰�: XR ī�޶� ���� Ȱ��ȭ
        var mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // TrackedPoseDriver ������Ʈ Ȯ��/�߰�
            var trackedPoseDriver = mainCamera.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
            if (trackedPoseDriver == null)
            {
                trackedPoseDriver = mainCamera.gameObject.AddComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
                Debug.Log("TrackedPoseDriver �߰���");
            }

            // ���� ����
            trackedPoseDriver.SetPoseSource(UnityEngine.SpatialTracking.TrackedPoseDriver.DeviceType.GenericXRDevice,
                                          UnityEngine.SpatialTracking.TrackedPoseDriver.TrackedPose.Center);
            trackedPoseDriver.trackingType = UnityEngine.SpatialTracking.TrackedPoseDriver.TrackingType.RotationAndPosition;
            trackedPoseDriver.updateType = UnityEngine.SpatialTracking.TrackedPoseDriver.UpdateType.UpdateAndBeforeRender;

            Debug.Log("XR ī�޶� ���� ���� �Ϸ�");
        }

        Debug.Log("XR ���� ���� �Ϸ�");
    }

    // �������� XR �ʱ�ȭ�ϴ� �޼��� (��ư Ŭ�� � ���)
    public void ManualInitializeXR()
    {
        InitializeXRSync();
    }

    // XR ���� �޼���
    public void DeinitializeXR()
    {
        var xrManager = XRGeneralSettings.Instance?.Manager;
        if (xrManager != null && xrManager.isInitializationComplete)
        {
            Debug.Log("XR ���� ��...");
            xrManager.StopSubsystems();
            xrManager.DeinitializeLoader();
            Debug.Log("XR ���� �Ϸ�");
        }
    }

    // ���� XR Ȱ��ȭ �õ�
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

                Debug.Log("���� ����� �õ� " + (i + 1) + "/10 - XR Device Active: " + XRSettings.isDeviceActive);

                if (XRSettings.isDeviceActive)
                {
                    Debug.Log("���� Ȱ��ȭ ����!");
                    ConfigureXRSettings();
                    yield break;
                }
            }
        }
        Debug.LogError("���� Ȱ��ȭ�� �����߽��ϴ�.");
    }
}