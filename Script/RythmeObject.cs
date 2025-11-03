using UnityEngine;
//To sub the object to the dynamic song event (sync to the wave)
public class RythmeObject : MonoBehaviour,ITickable
{
    protected Song _currentSong;
    [SerializeField]protected int _mainBeatIndex = 0;
    void Start()
    {
        Metronome.instance.OnMusicStart.AddListener(SubToBeat);
        SubToBeat(Metronome.instance._currentSong);
        Metronome.instance.OnMusicStop.AddListener(UnSubToBeat);
    }
    public virtual void Tick(){ }

    public virtual void SubTick(){ }

    public virtual void SubToBeat(Song song)
    {
        _currentSong = song;
        switch (_mainBeatIndex)
        {
            case 0:
                _currentSong.Beat1.AddListener(Tick);
                break;
            case 1:
                _currentSong.Beat2.AddListener(Tick);
                break;
            case 2:
                _currentSong.Beat3.AddListener(Tick);
                break;
            default:
                break;
        }
        
    }
    public void UnSubToBeat()
    {
        _currentSong.Beat1.RemoveListener(Tick);
    }
}
