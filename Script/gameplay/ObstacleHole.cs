using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class ObstacleHole : RythmeObject,IDestructible,Ipoolable
{
    [SerializeField] private GameObject _plank;
    [SerializeField] private GameObject _holeObject;
    private Vector3 _PlankScale;
    private bool _isTicking;
    [SerializeField] private GameObject obstacleHurtPlayer;
    [Button]
    public void Destroy()
    {
        _plank.transform.DOScale(_PlankScale,0.5f).SetEase(Ease.OutBounce);
        obstacleHurtPlayer.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _PlankScale = _plank.transform.localScale;
        _plank.transform.localScale = Vector3.zero;
    }

    void Start()
    {
        Metronome.instance.OnMusicStart.AddListener(SubToBeat);
        SubToBeat(Metronome.instance._currentSong);
        Metronome.instance.OnMusicStop.AddListener(UnSubToBeat);
    }

    public override void Tick()
    {
        if (!_isTicking)
            return;
        transform.DOLocalMoveZ(transform.localPosition.z - 5, 0.5f);
        _holeObject.transform.DOPunchScale(new Vector3(1.1f, 0f, 1.1f), 0.2f).OnComplete(() => _holeObject.transform.localScale = Vector3.one);

    }

    public void Pool()
    {
        //throw new System.NotImplementedException();
        _isTicking = false;

    }
    public void UnPool()
    {
        _holeObject.transform.localScale = Vector3.one;
        _plank.transform.localScale= Vector3.zero;
        _isTicking = true;
        obstacleHurtPlayer.SetActive(true);
        //throw new System.NotImplementedException();
    }
}
