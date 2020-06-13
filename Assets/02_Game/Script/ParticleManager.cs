/**
 *@fire         PrticleManager.cs
 * @brief      パーティクルを加算合成で光らせる用
 * 
 * @author   kondou katsutoshi
 * @date      2020/06/13　作成  
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private ParticleSystem _particleSys;
    private ParticleSystem.Particle[] _particle;
    public int _MaxBrightness = 2;
    ParticleSystem.EmitParams _emitParams;
    ParticleSystem.EmitParams _temp;

    Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _particleSys = GetComponent<ParticleSystem>();
        _emitParams = default;
        _temp = default;
        //_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        int _maxParticle  = _particleSys.main.maxParticles;

        if (_particle == null || _particle.Length < _maxParticle)
        {
            _particle = new ParticleSystem.Particle[_maxParticle];
        }
        int _particleNum = _particleSys.GetParticles(_particle);

        for (int i = 0; i < _particleNum; i++)
        {
            if (_particle[i].lifetime <= 0.1f && !_emitParams.particle.Equals(_temp.particle))
            {
                _emitParams.particle = _particle[i];
                //_emitParams.
                _particleSys.Emit(_emitParams,(int)Random.Range(0,_MaxBrightness));
            }
            _temp.particle = _particle[i];
        }

    }
}
