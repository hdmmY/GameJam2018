using UnityEngine;

public static class TimerUtils
{
    public static bool AddTime (this PlayerCoreECS.Timer timer, float addTime)
    {
        timer.CurrentTime += addTime * timer.Speed;

        return timer.CurrentTime > timer.EndTime;
    }
}