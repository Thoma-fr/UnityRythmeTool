using System;
//can be replace by a class since some method a not always use
public interface ITickable
{
    
    void SubToBeat(Song song);
    void UnSubToBeat();
    void Tick();
    void SubTick();
}
