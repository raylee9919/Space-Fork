using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    public Light[] lights;

    public void ActivateLights()
    {
        Debug.Log("[LightController] ActivateLights »£√‚µ ");

        StartCoroutine(SequentialLightOn());
    }

    private IEnumerator SequentialLightOn()
    {
        foreach (var light in lights)
        {
            light.enabled = true;
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void Start()
    {
        foreach (var light in lights)
        {
            light.enabled = false;
        }
    }
}
