using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class revolution : MonoBehaviour
{
    public int PlanetID;
    public float _orbitalDistance;
    public float _orbitalSpeed;
    public float _startAngle;

    private CameraMove _script = null;

    private float _angle;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(_orbitalDistance * Mathf.Sin(_startAngle),
            0.0f,_orbitalDistance * Mathf.Cos(_startAngle));

        _angle = _startAngle;
        _orbitalSpeed *= 0.001f;

        _script = GameObject.Find("CameraObj").GetComponent<CameraMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_script._isOrbital)
        {

        }
        else
        {
            this.transform.position = new Vector3(_orbitalDistance * Mathf.Sin(_angle),
           0.0f, _orbitalDistance * Mathf.Cos(_angle));
            _angle += _orbitalSpeed;





            if (_angle < 360.0f)
            {
                _angle += 360.0f;
            }
            if (_angle > 360.0f)
            {
                _angle -= 360.0f;
            }
        }
       
    }
}
