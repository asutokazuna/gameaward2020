using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbCreate : MonoBehaviour
{
    private BlockTank script;
    private bool bFirst;
    private bool bFill;
    private float fTimer;
    public float WAIT_TIME;

    private int nOldWater;

    public int nTargetCnt;

    // Start is called before the first frame update
    void Start()
    {
        bFirst = false;
        bFill = false;
        script = this.GetComponent<BlockTank>();
        this.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    // Update is called once per frame
    void Update()
    {
        if(script._numWater == 0)
        {
            bFirst = false;
            bFill = false;
            this.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.GetComponent<ParticleSystem>().Clear(true);

            if (nOldWater > 0)
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

        if (script._numWater > nTargetCnt)
        {
            if(!bFirst)
            {
                bFirst = true;

                GameObject obj = (GameObject)Resources.Load("WaterBulb_in");

                Vector3 pos = this.transform.position;
                pos.y -= 0.2f;


                Instantiate(obj, pos, Quaternion.Euler(-90, 0, 0));
            }
        }


        /*
         * 中上メモ
         * 変数削除にともない、ここの処理を一時的に飛ばします
         */
        if (false)
        {
            if (!bFill)
            {
                bFill = true;
                this.GetComponent<ParticleSystem>().Play(true);
                

            }
        }


        nOldWater = script._numWater;
    }
}
