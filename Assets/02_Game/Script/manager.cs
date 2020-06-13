using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        bool _isDebug;

        _isDebug = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().isDebug();

        if(_isDebug)
        {
            this.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
