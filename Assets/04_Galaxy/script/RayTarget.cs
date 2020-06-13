using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTarget : MonoBehaviour
{
    public GameObject _targetObj;
    public Vector3 _dirVector;
    [SerializeField]
    private Vector3 _startPos;
    private Vector3 _movePos;

    private bool _isSelect = false;
    [SerializeField]
    private float _count;

    public Quaternion _rotate;
    // Start is called before the first frame update
    void Start()
    {
        _dirVector = transform.parent.parent.transform.position - _targetObj.transform.position;
        _startPos = _targetObj.transform.localPosition;

        _movePos = Vector3.MoveTowards(_targetObj.transform.localPosition, 
                            _targetObj.transform.localRotation * new Vector3(0, 1, 0), 0.04f);
        _rotate = _targetObj.transform.localRotation;
    }


    

    // Update is called once per frame
    void Update()
    {
        if (_isSelect)
        {
            _targetObj.transform.localPosition = Vector3.Lerp(_startPos, _movePos, Mathf.Sin(_count));
            _count += 0.1f;
        }
        else
        {
            _targetObj.transform.localPosition = Vector3.Lerp(_movePos, _startPos, 1.0f - Mathf.Sin(_count));
            _count -= 0.1f;
        }

        if(_count > 0.5f)
        {
            _count = 0.5f;
        }
        if (_count < 0.0f)
        {
            _count = 0.0f;
        }
    }

    public void LandMove(bool flg)
    {
        _isSelect = flg;
    }
}
