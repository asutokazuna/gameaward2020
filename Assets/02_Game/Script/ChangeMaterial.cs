using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] GameObject _changeObject;
    [SerializeField] Material[] _materials;
    private int         _count;
    private Animator    _signBoardAnim;
    private MainCamera  _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _count = -1;
        _signBoardAnim = GetComponent<Animator>();
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_mainCamera._startMove)
        {
            _signBoardAnim.SetBool("Start", true);
        }
    }

    void SetMaterial()
    {
        _count++;

        // 配列外を参照しないように
        if (_count < 0)
        {
            _count = 0;
        }
        else if (_count >= _materials.Length)
        {
            _count = _materials.Length - 1;
        }

        //Debug.Log(_count);
        //Debug.Log(_materials.Length);
        // マテリアルの変更
        _changeObject.GetComponent<Renderer>().material = _materials[_count];
    }
}
