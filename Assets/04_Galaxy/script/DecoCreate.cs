using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoCreate : MonoBehaviour
{
    public string _decoName;

    private GameObject _targetObj;
    private GameObject _deco;

    private bool _test;
        // Start is called before the first frame update
    void Start()
    {
        _targetObj = GetComponent<RayTarget>()._targetObj;
        _deco = (GameObject)Resources.Load("deco/" + _decoName);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_test)
        {
            _test = true;
            GameObject _decoObj;
            _decoObj = Instantiate(_deco, transform.position, _targetObj.transform.rotation);
            _decoObj.transform.parent = _targetObj.transform;
        }
    }
}
