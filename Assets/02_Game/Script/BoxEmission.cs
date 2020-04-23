using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEmission : MonoBehaviour
{
    [SerializeField] float EmissionTimer;
    MeshRenderer mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.material.SetFloat("_Emission", 0);
    }


    public void BoxEmissionOn()
    {
        mesh.material.SetFloat("_Emission", 1);
        Invoke("BoxEmissionOff", EmissionTimer);
    }

    public void BoxEmissionOff()
    {
        mesh.material.SetFloat("_Emission", 0);
    }
}
