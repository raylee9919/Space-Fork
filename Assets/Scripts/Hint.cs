using UnityEngine;

public class Hint1 : MonoBehaviour
{
    public GameObject hintText;

    void Start()
    {
        if (hintText != null)
            hintText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger 발생: " + other.name);
        if (hintText != null)
            hintText.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger 종료: " + other.name);
        if (hintText != null)
            hintText.SetActive(false);
    }
}
