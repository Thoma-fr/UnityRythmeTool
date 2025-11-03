using UnityEngine;

public class AudioSync : MonoBehaviour
{

    [field:SerializeField] public float bias { get; set; }
    [field: SerializeField] public float timestep { get; set; }
    [field: SerializeField] public float timeToBeat { get; set; }
    [field: SerializeField] public float restSmoothTime { get; set; }

    private float _previousAudioValue;
    private float _audioValue;
    private float _timer;

    protected bool _isBeat;

    void Update()
    {
        OnUpdate(); 
    }
    public virtual void OnBeat()
    {
        Debug.Log("beat");
        _timer = 0;
        _isBeat = true;
    }

    public virtual void OnUpdate()
    {
        _previousAudioValue = _audioValue;
        _audioValue = AudioSpectrum.SpectrumValue;

        if (_previousAudioValue > bias && _audioValue <= bias)
        {
            if (_timer > timestep)
            {
                OnBeat();
            }
        }
        if (_previousAudioValue <= bias && _audioValue > bias)
        {
            if (_timer > timestep)
                OnBeat();
        }
        _timer += Time.deltaTime;
    }
}
