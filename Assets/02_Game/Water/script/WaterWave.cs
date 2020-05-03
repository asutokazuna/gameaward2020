using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWave : MonoBehaviour
{
    private MeshRenderer _meshRend;
    private GameObject _parent;
    private WaterFlow _myScript;        //コンポーネント取得用


    // Start is called before the first frame update
    void Start()
    {
        _meshRend = GetComponent<MeshRenderer>();
        _parent = transform.parent.gameObject;
        _myScript = _parent.GetComponent<WaterFlow>();
        _meshRend.material.SetFloat("_Distortion", 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (_myScript._currentWater >= _myScript._maxWater - 35)
        {
            _meshRend.material.SetFloat("_Distortion", 1);
        }
        else if (_myScript._currentWater <= 0)
        {
            _meshRend.material.SetFloat("_Distortion", 0);
        }

    }
}
