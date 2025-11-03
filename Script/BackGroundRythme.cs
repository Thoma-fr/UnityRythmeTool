using DG.Tweening;
using UnityEngine;

public class BackGroundRythme : RythmeObject
{
    [SerializeField] private float _beatScale=5;
    [SerializeField] private float _restScale=1;
    public void SubTick(){ }
    public override void Tick()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScaleY(_beatScale, _currentSong.BPM[_mainBeatIndex]/2));
        mySequence.Append(transform.DOScaleY(_restScale, _currentSong.BPM[_mainBeatIndex] / 2));
    }

}
