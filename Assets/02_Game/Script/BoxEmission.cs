using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEmission : MonoBehaviour
{
    [SerializeField]float _emissionSpeed = 0.03f;
    MeshRenderer mesh;
    private AudioSource audioSource;
    public AudioClip _SEBoxBreak;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.material.SetFloat("_Brightness", 0);
        audioSource = GetComponent<AudioSource>();
    }

    public void StartEmission()
    {
        StartCoroutine("Emission");
    }
    public IEnumerator Emission()
    {
        for (float i = 0.5f; i <= Mathf.PI; i += _emissionSpeed)
        {
            mesh.material.SetFloat("_Brightness", Mathf.Clamp(Mathf.Sin(i), 0, 1.0f));
            audioSource.PlayOneShot(_SEBoxBreak);
            yield return null;
        }
    }
}