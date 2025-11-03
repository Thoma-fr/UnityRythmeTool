using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    [SerializeField]private float[] _audioSpectrum;
    [field:SerializeField] public static float SpectrumValue {  get; private set; }
    [SerializeField] private float BiggestSpectrumValue;
    void Start()
    {
        _audioSpectrum = new float[256];
    }

    void Update()
    {
        AudioListener.GetSpectrumData(_audioSpectrum,0,FFTWindow.Hamming);
        if(_audioSpectrum != null && _audioSpectrum.Length>0)
        {
            SpectrumValue = _audioSpectrum[0]*100;
            if(SpectrumValue > BiggestSpectrumValue)
                BiggestSpectrumValue = SpectrumValue;
        }
    }
}
