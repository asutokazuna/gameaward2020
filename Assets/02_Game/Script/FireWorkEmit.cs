using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorkEmit : MonoBehaviour
{
    
   
    private Map ClearScript;

    [SerializeField]
    private float EmitDistance = 0.0f;

    private GameObject FireWorkObj;

    private float _timer = 0;

    [SerializeField]
    private float Delay_min = 0.0f;
    [SerializeField]
    private float Delay_max = 0.0f;

    public Color[] _color1 = new Color[3];
    public Color[] _color2 = new Color[3];
    public Color[] _color3 = new Color[3];

    // Start is called before the first frame update
    void Start()
    {
        ClearScript = GameObject.FindWithTag("Map").GetComponent<Map>();
        FireWorkObj = (GameObject)Resources.Load("FireWork");
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(ClearScript._gameClear)
        {
            if(_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                int _angle;
                _angle = Random.Range(0, 360);

                Vector3 _pos = transform.position;
                _pos.x = EmitDistance * Mathf.Sin(_angle);
                _pos.z = EmitDistance * Mathf.Cos(_angle);

                GameObject _obj;
                _obj = Instantiate(FireWorkObj, _pos, Quaternion.identity);

                switch(Random.Range(0,3))
                {
                    case 0:
                        _obj.GetComponent<FireWorks>()._color = _color1;
                        break;

                    case 1:
                        _obj.GetComponent<FireWorks>()._color = _color2;
                        break;

                    case 2:
                        _obj.GetComponent<FireWorks>()._color = _color3;
                        break;
                }
                
               
               

                _timer = Random.Range(Delay_min, Delay_max);
            }
            
        }
    }
}
