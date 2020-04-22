using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    public bool ClearSwitch;
    public float WaitTime;

    private Map FlgScript;
    private Clear ClearScript;
    private bool _isFirst;
    private float _timer;

    private GameObject ParticleLeft;
    private GameObject ParticleRight;
    private GameObject ParticleTop;

    // Start is called before the first frame update
    void Start()
    {
        FlgScript = GameObject.Find("Map").GetComponent<Map>();
        ClearScript = GameObject.Find("Clear").GetComponent<Clear>();
        _isFirst = true;
        _timer = WaitTime;

        ParticleLeft = GameObject.Find("ParticleLeft");
        ParticleRight = GameObject.Find("ParticleRight");
        ParticleTop = GameObject.Find("ParticleTop");

        ParticleLeft.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        ParticleLeft.GetComponent<ParticleSystem>().Clear(true);

        ParticleRight.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        ParticleRight.GetComponent<ParticleSystem>().Clear(true);

        ParticleTop.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        ParticleTop.GetComponent<ParticleSystem>().Clear(true);



    }

    // Update is called once per frame
    void Update()
    {
        if(ClearSwitch && FlgScript._gameClear)
        {
            if(_timer <= 0.0f)
            {
                SetClear();
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }
       
    }

    private void SetClear()
    {
        if (_isFirst)
        {
            ClearScript.StartClear();
            _isFirst = false;

            ParticleLeft.GetComponent<ParticleSystem>().Play(true);
            ParticleRight.GetComponent<ParticleSystem>().Play(true);
            ParticleTop.GetComponent<ParticleSystem>().Play(true);
        }
    }
}
