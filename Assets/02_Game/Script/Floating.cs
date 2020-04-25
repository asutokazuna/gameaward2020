using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float _distance;
    public float _speed;
    public float _startAngle;
    private float _angle;
    private Transform _currentTransform;

    // Start is called before the first frame update
    void Start()
    {
        _angle = _startAngle;
        _currentTransform = this.transform;
        _distance = _distance / 200.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _angle += _speed;

        if(_angle < 360)
        {
            _angle += 360;
        }
        if (_angle > 360)
        {
            _angle -= 360;
        }

        Vector3 _pos = _currentTransform.position;
        _pos.y += _distance * Mathf.Sin(_angle);

        this.transform.position = _pos;
    }
}
