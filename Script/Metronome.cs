using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//class used as a metrome for the non dynamics related objects
public class Metronome : MonoBehaviour
{
    public static Metronome instance;
    [field: SerializeField] public AudioSource Source { get; private set; }


    [field: SerializeField] public List<Song> Songs { get; private set; } = new List<Song>();
    public Song _currentSong;
    private int _songIndex=0;
    [SerializeField] private bool _isTicking;
    [SerializeField] private bool _tickSound;
    [SerializeField] private List<List<ITickable>> Container;
    [field: SerializeField] public List<ITickable> ToTick1 = new List<ITickable>();
    [field: SerializeField] public List<ITickable> ToTick2 = new List<ITickable>();
    [SerializeField] private AudioClip _ticksound, _subtickSound;

    public UnityEvent<Song> OnMusicStart;
    public UnityEvent OnMusicStop;

    private List<Coroutine> _coroutines = new List<Coroutine>();
    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        _currentSong = Songs[0];
    }
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        StartSound();
    }
    IEnumerator musicDuration()
    {
        yield return new WaitForSeconds(_currentSong.Duration);
        Stopsound();
        if(_songIndex<Songs.Count-1)
            _songIndex++;
        else
            _songIndex=0;
        _currentSong = Songs[_songIndex];
        StartSound();
    }
    public void StartSound()
    {
        StartCoroutine(musicDuration());
        _currentSong.SetupBPM();
        Source.resource = _currentSong.Music;
        Source.Play();
        OnMusicStart.Invoke(_currentSong);
        if (_currentSong.BPM.Length > 0) _currentSong._beatCoroutines.Add(StartCoroutine(_currentSong.Beat(_currentSong.BPM[0], _currentSong.Beat1)));
        if (_currentSong.BPM.Length > 1) _currentSong._beatCoroutines.Add(StartCoroutine(_currentSong.Beat(_currentSong.BPM[1], _currentSong.Beat2)));
        if (_currentSong.BPM.Length > 2) _currentSong._beatCoroutines.Add(StartCoroutine(_currentSong.Beat(_currentSong.BPM[2], _currentSong.Beat3)));
    }
    private void Stopsound()
    {
        foreach (var coroutine in _currentSong._beatCoroutines)
        {
            if (coroutine != null) StopCoroutine(coroutine);
        }
        Source.Stop();
    }
}