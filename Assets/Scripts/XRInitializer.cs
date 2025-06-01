using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using System.Collections;

public class XRInitializer : MonoBehaviour
{
    public bool enableOnStart = true;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (enableOnStart)
        {
            InitializeXRSync();
        }
    }

    public void InitializeXRSync()
    {
        var xrManager = XRGeneralSettings.Instance?.Manager;
        
        if (xrManager.isInitializationComplete && XRSettings.isDeviceActive)
        {
            return;
        }

        if (!xrManager.isInitializationComplete)
        {
            xrManager.InitializeLoaderSync();
        }

        if (xrManager.activeLoader != null)
        {
            xrManager.StartSubsystems();

            var displaySubsystem = xrManager.activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
            if (displaySubsystem != null)
            {
                if (!displaySubsystem.running)
                {
                    displaySubsystem.Start();
                }
            }

            var inputSubsystem = xrManager.activeLoader.GetLoadedSubsystem<XRInputSubsystem>();
            if (inputSubsystem != null)
            {
                if (!inputSubsystem.running)
                {
                    inputSubsystem.Start();
                }
                inputSubsystem.TryRecenter();
            }
        }

        if (XRSettings.isDeviceActive)
        {
            ConfigureXRSettings();
        }
        else
        {
            StartCoroutine(ForceActivateXR());
        }
    }

    private void ConfigureXRSettings()
    {
        XRSettings.eyeTextureResolutionScale = 1.0f;
        XRSettings.renderViewportScale = 1.0f;
        var mainCamera = Camera.main;
        if (mainCamera != null)
        {
            var trackedPoseDriver = mainCamera.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
            if (trackedPoseDriver == null)
            {
                trackedPoseDriver = mainCamera.gameObject.AddComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
            }

            trackedPoseDriver.SetPoseSource(UnityEngine.SpatialTracking.TrackedPoseDriver.DeviceType.GenericXRDevice,
                                          UnityEngine.SpatialTracking.TrackedPoseDriver.TrackedPose.Center);
            trackedPoseDriver.trackingType = UnityEngine.SpatialTracking.TrackedPoseDriver.TrackingType.RotationAndPosition;
            trackedPoseDriver.updateType = UnityEngine.SpatialTracking.TrackedPoseDriver.UpdateType.UpdateAndBeforeRender;

        }
    }
    
    public void ManualInitializeXR()
    {
        InitializeXRSync();
    }
    
    public void DeinitializeXR()
    {
        var xrManager = XRGeneralSettings.Instance?.Manager;
        if (xrManager != null && xrManager.isInitializationComplete)
        {
            xrManager.StopSubsystems();
            xrManager.DeinitializeLoader();
        }
    }

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


                if (XRSettings.isDeviceActive)
                {
                    ConfigureXRSettings();
                    yield break;
                }
            }
        }
    }
}