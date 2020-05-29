using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbCreate : MonoBehaviour
{
    private WaterFlow script;
    private bool bFirst;
    private bool bFill;
    private float fTimer;
    public float WAIT_TIME;

    private int nOldWater;

    public int nTargetCnt;

    public bool ParticleSwitch;

    // Start is called before the first frame update
    void Start()
    {
        bFirst = false;
        bFill = false;
        script = this.GetComponent<WaterFlow>();
        this.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    // Update is called once per frame
    void Update()
    {
        if(script._currentWater == 0)
        {
            bFirst = false;
            bFill = false;
            

            foreach (Transform childTransform in this.transform)
            {
                if (childTransform.tag == "WaterLeak")
                {
                    childTransform.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
                    childTransform.GetComponent<ParticleSystem>().Clear(true);
                }
            }

            if (nOldWater > 10)
            {
                fTimer = WAIT_TIME;
               
            }

            if(fTimer > 0.0f)
            {
                fTimer -= Time.deltaTime;
                if(fTimer <= 0.0f)
                {
                    fTimer = -1.0f;
                    GameObject obj = (GameObject)Resources.Load("WaterCube");

                    Vector3 pos = this.transform.position;
                    pos.y += 0.0f;

                    
                    Instantiate(obj, pos, Quaternion.Euler(-90, 0, 0));
                   
                }
            }
        }

        if (script._currentWater > nTargetCnt)
        {
            if (!bFirst && gameObject.tag == "WaterBlock")
            {
                bFirst = true;

                GameObject obj = (GameObject)Resources.Load("WaterBulb_in");

                Vector3 pos = this.transform.position;
                pos.y -= 0.2f;

                GameObject _instantObj;
                _instantObj = Instantiate(obj, pos, Quaternion.Euler(-90, 0, 0));
                _instantObj.GetComponent<BulbDestroy>()._parent = this.gameObject;
            }
        }

        if (script._isFullWater)
        {
            if (!bFill)
            {
                bFill = true;

                if(ParticleSwitch)
                {
                    this.GetComponent<ParticleSystem>().Play(true);
                }
                
                

            }
        }


        nOldWater = script._currentWater;
    }
}
