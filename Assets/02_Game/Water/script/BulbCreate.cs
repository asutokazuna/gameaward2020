using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbCreate : MonoBehaviour
{
    private BlockTank script;
    private bool bFirst;

    private int nOldWater;

    // Start is called before the first frame update
    void Start()
    {
        bFirst = false;
        script = this.GetComponent<BlockTank>();
    }

    // Update is called once per frame
    void Update()
    {
        if(script._numWater == 0)
        {
            bFirst = false;

            if(nOldWater > 0)
            {
                GameObject obj = (GameObject)Resources.Load("WaterBulb_out");

                Vector3 pos = this.transform.position;
                pos.y += 0.4f;


                Instantiate(obj, pos, Quaternion.identity);
            }
        }

        if (script._numWater > 0)
        {
            if(!bFirst)
            {
                bFirst = true;

                GameObject obj = (GameObject)Resources.Load("WaterBulb_in");

                Vector3 pos = this.transform.position;
                pos.y -= 0.4f;


                Instantiate(obj, pos, Quaternion.identity);
            }
        }


        nOldWater = script._numWater;
    }
}
