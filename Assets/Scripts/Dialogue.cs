using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            DLM.Load("Foo");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            DLM.Print();
        }
    }
}
