using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirection : MonoBehaviour
{
   

    private GameObject CameraObj;
    // Start is called before the first frame update
    void Start()
    {
        CameraObj = GameObject.Find("CameraObj");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
