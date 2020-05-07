using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorkEmit : MonoBehaviour
{
    
   
    private Map ClearScript;

    [SerializeField]
    private float EmitDistance;

    private GameObject FireWorkObj;

    private float _timer = 0;

    [SerializeField]
    private float Delay_min;
    [SerializeField]
    private float Delay_max;

    // Start is called before the first frame update
    void Start()
    {
        ClearScript = GameObject.FindWithTag("Map").GetComponent<Map>();
        FireWorkObj = (GameObject)Resources.Load("FireWork");
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

                Instantiate(FireWorkObj, _pos, Quaternion.identity);
                _timer = Random.Range(Delay_min, Delay_max);
            }
            
        }
    }
}
