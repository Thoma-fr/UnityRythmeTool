using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
//contain the data for the song (seted in editor for modularity)
public enum SongType
{
    Normal,
    Slow,
    Fast,
}
[Serializable]
public class Song
{
    
    [field: SerializeField] public AudioClip Music { get; set; }
    [SerializeField] private SongType _type;
    [Tooltip("more than 3 won't work")]
    [field: SerializeField] public float[] BPM = {0, 0, 0};
    [field: SerializeField] public float SongBPM { get; private set; }
    [field: SerializeField] public float Duration { get; set; }

    public List<Coroutine> _beatCoroutines = new List<Coroutine>();
    public UnityEvent Beat1;
    public UnityEvent Beat2;
    public UnityEvent Beat3;

    public void SetupBPM() //this really need some refacto
    {
        if (_type == SongType.Normal)
        {
            BPM[0] = ((((60 * 1000) / SongBPM) * 0.001f) * 4f) / 2;
            BPM[1] = BPM[0] * 2;
            BPM[2] = BPM[0] / 2;
        }
        else if (_type == SongType.Slow)
        {
            BPM[0] = ((((60 * 1000) / SongBPM) * 0.001f) * 4f);
            BPM[1] = BPM[0];
            BPM[2] = BPM[0];
        }
        else
        {
            BPM[0] = ((((60 * 1000) / SongBPM) * 0.001f) * 4f) / 4;
            BPM[1] = BPM[0] * 2;
            BPM[2] = BPM[0] * 4;
        }
    }
    
    public IEnumerator Beat(float interval, UnityEvent beatEvent)//event called at every beat of the song
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            beatEvent.Invoke();
        }
    }
}
