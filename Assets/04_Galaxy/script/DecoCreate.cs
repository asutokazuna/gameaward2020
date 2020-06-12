using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoCreate : MonoBehaviour
{
    public string _decoName;

    private GameObject _targetObj;
    private GameObject _deco;

    private bool _isFirst = true;

    private SceneMgr _sceneManager = default;

    [SerializeField]
    private E_SCENE _sceneID;
        // Start is called before the first frame update
    void Start()
    {
        _targetObj = GetComponent<RayTarget>()._targetObj;
        _deco = (GameObject)Resources.Load("deco/" + _decoName);

        _sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isFirst && _sceneManager.GetStageClear(_sceneID))
        {
            _isFirst = false;
            GameObject _decoObj;
            _decoObj = Instantiate(_deco, transform.position, _targetObj.transform.rotation);
            _decoObj.transform.parent = _targetObj.transform;
        }

#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.P))
        {
            GameObject _decoObj;
            _decoObj = Instantiate(_deco, transform.position, _targetObj.transform.rotation);
            _decoObj.transform.parent = _targetObj.transform;
        }
#endif

    }
}
