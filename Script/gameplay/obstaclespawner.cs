using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
public class obstaclespawner : MonoBehaviour, ITickable
{

    [field: SerializeField] public int BeatBeforeSpawnMin { get; set; }
    [field: SerializeField] public CardinalPoint CardinalPoint { get; set; }
    [field: SerializeField] public int BeatBeforeSpawnMax { get; private set; }

    [field: SerializeField] public int BeatBeforeFirstUtil { get; private set; }
    [field: SerializeField] public List<GameObject> obstacles { get;  set; }
    [field: SerializeField] public List<GameObject> utilObject { get; set; }
    [field: SerializeField] public bool IsTicking { get; set; }=true;
    [field: SerializeField] public bool IsStartLane { get; set; } = false;
    [SerializeField] private Transform LinePivot;

    [SerializeField]private Transform _leftPos, _rightPos;
    private int _currentBeatsLast;
    private int _currentBeatBeforeFirstUtil;
    private Song _currentSong;

    [SerializeField] List<GameObject> _pool=new List<GameObject>();
    [SerializeField] int _numberToPool=10;
    [SerializeField] float _OutofPoolDuration = 10;


    private bool _isUnlocked;
    void Awake()
    {
        _currentBeatsLast = Random.Range(BeatBeforeSpawnMin, BeatBeforeSpawnMax+1);
        _currentBeatBeforeFirstUtil= BeatBeforeFirstUtil;
    }

    void Start()
    {
        PlayerMovement player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        player.OnLandLine.AddListener(OnPlayerLand);
        Metronome.instance.OnMusicStart.AddListener(SubToBeat);
        Metronome.instance.OnMusicStop.AddListener(UnSubToBeat);
        Metronome.instance.ToTick1.Add(this);


        for (int i = 0; i < _numberToPool; i++)
        {
            for (int j = 0; j < obstacles.Count; j++)
            {
                _pool.Add(Instantiate(obstacles[j],new Vector3(1000,1000,1000), transform.rotation));
            }
        }

        if(!IsStartLane)
        {
            LinePivot.localScale = Vector3.zero;
        }
    }
    private void OnPlayerLand(LineData ldt)
    {
        if (_isUnlocked) return;
        if(ldt.cardinalPoint!=CardinalPoint) return;

        _isUnlocked=true;
        IsTicking = true;
        LinePivot.DOScale(Vector3.one, .5f);


    }
    public void SpawnObstacle()
    {
        if (_pool.Count <= 0)
        {
            _pool.Add(Instantiate(obstacles[Random.Range(0, obstacles.Count)], new Vector3(1000, 1000, 1000), transform.rotation));
        }

        int indexRand = Random.Range(0, _pool.Count);
        
        if (_pool[indexRand].TryGetComponent(out Ipoolable pooled))
        {
            GameObject currenObject = _pool[indexRand];
            bool isleftPosition = Random.value < 0.5f;
            currenObject.transform.position = isleftPosition ? _rightPos.position : _leftPos.position;

            if (currenObject.TryGetComponent(out LaneButton gravtityButton))
                gravtityButton.SetButtonCardinalPoint(CardinalPoint, isleftPosition);

            pooled.UnPool();
            _pool.Remove((_pool[indexRand]));
            StartCoroutine(BackToPoolDelay(currenObject));
        }

        //GameObject newobstacle = Instantiate(obstacles[Random.Range(0, obstacles.Count)], indexRand == 0 ? _leftPos.position : _rightPos.position, Quaternion.identity);
        _currentBeatsLast = Random.Range(BeatBeforeSpawnMin, BeatBeforeSpawnMax + 1);
    }
    public void Tick()
    {
        if(!IsTicking) return;
        _currentBeatsLast--;
        _currentBeatBeforeFirstUtil--;
        if (_currentBeatBeforeFirstUtil <= 0)
        {
            _currentBeatBeforeFirstUtil= Random.Range(BeatBeforeSpawnMin*10, BeatBeforeSpawnMax*10);
            bool isAtLeftPosition = Random.value < 0.5f;
            GameObject go= Instantiate(utilObject[0], !isAtLeftPosition ? _rightPos.position : _leftPos.position, transform.rotation);

            if (go.TryGetComponent(out LaneButton gravtityButton))
            {
                gravtityButton.SetButtonCardinalPoint(CardinalPoint, !isAtLeftPosition);
            }
            _currentBeatsLast++;
            return;
        }
        else
            if (_currentBeatsLast <= 0)
            {
                SpawnObstacle();
                _currentBeatsLast = Random.Range(BeatBeforeSpawnMin, BeatBeforeSpawnMax + 1);
            }
    }
    public void SubTick()
    {

    }


    public void SubToBeat(Song song)
    {
        _currentSong = song;
        _currentSong.Beat1.AddListener(Tick);
    }

    public void UnSubToBeat()
    {
        _currentSong.Beat1.RemoveListener(Tick);
    }
        
    IEnumerator BackToPoolDelay(GameObject topool)
    {
        yield return new WaitForSeconds(_OutofPoolDuration);
        _pool.Add(topool);
        topool.GetComponent<Ipoolable>().Pool();
    }

}
