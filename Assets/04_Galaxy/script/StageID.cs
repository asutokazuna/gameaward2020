using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageID : MonoBehaviour
{
    public E_SCENE _stageID = 0;//!<例外処理シーンほしい
    E_SCENE _stageId = 0;
    public int _level = 1;

    // Start is called before the first frame update
    void Start()
    {
        _stageId = GameObject.Find("CameraObj").GetComponent<RaySystem>().GetID();

    }

    // Update is called once per frame
    void Update()
    {
        _stageId = GameObject.Find("CameraObj").GetComponent<RaySystem>().GetID();
        //Debug.Log(_stageId);
    }
}
