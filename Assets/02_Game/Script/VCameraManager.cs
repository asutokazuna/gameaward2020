using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class VCameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera vcamera;

    GameObject gameObject;
    void Start()
    {
        gameObject = GameObject.Find("Peacock_Blue");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            vcamera.Priority = 100;
            // 有効化・無効化
            GetComponent<CinemachineVirtualCamera>().enabled =
                !GetComponent<CinemachineVirtualCamera>().enabled;//カメラ追跡のオンオフ
           
           
        }
        vcamera.Follow = gameObject.transform;//追跡対象の設定
        vcamera.LookAt = gameObject.transform;//追跡対象の設定
    }
}


