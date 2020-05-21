using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorks : MonoBehaviour
{
    [SerializeField]
    private float EXP_TIME = 0.0f;
    [SerializeField]
    private float EXP_timer =0.0f;

    [SerializeField]
    private float _speed = 0.0f;

    [SerializeField]
    private float EXP_min = 0.0f;
    [SerializeField]
    private float EXP_max = 0.0f;

    public Color[] _color;
    

    // Start is called before the first frame update
    void Start()
    {
        EXP_timer = EXP_TIME;
        EXP_timer += Random.Range(EXP_min, EXP_max);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(EXP_timer);
        if (EXP_timer > 0)
        {
            EXP_timer -= Time.deltaTime;

            Vector3 _pos = transform.position;
            _pos.y += _speed * Time.deltaTime;

            Debug.Log(_speed * Time.deltaTime);

            transform.position = _pos;

        }
        else
        {
            GameObject obj;
            GameObject Instant;
            obj = (GameObject)Resources.Load("FireWork_FX");
            Instant = Instantiate(obj, transform.position, Quaternion.identity);

            var _setColor0 = Instant.GetComponent<ParticleSystem>().main;
            _setColor0.startColor = _color[0];
            var _setColor1 = Instant.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            _setColor1.startColor = _color[1];
            var _setColor2 = Instant.transform.GetChild(1).GetComponent<ParticleSystem>().main;
            _setColor2.startColor = _color[2];

            Destroy(gameObject);
        }
    }
}
