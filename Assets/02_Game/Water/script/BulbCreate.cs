using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbCreate : MonoBehaviour
{
    private BlockTank script;
    private bool bFirst;

    // Start is called before the first frame update
    void Start()
    {
        bFirst = false;
        script = this.GetComponent<BlockTank>();
    }

    // Update is called once per frame
    void Update()
    {
        if(script._fullWater)
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
    }
}
