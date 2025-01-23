using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    private Dictionary<ITimer, Coroutine> runningTimers = new Dictionary<ITimer, Coroutine>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartTimer(ITimer timer, bool autoStart = true)
    {
        if (runningTimers.ContainsKey(timer))
        {
            StopTimer(timer);
        }

        if (autoStart)
        {
            Coroutine timerCoroutine = StartCoroutine(TimerRoutine(timer));
            runningTimers[timer] = timerCoroutine;
        }
    }

    private IEnumerator TimerRoutine(ITimer timer)
    {
        BasicTimer basicTimer = timer as BasicTimer;
        basicTimer.Start();

        while (!timer.IsCompleted)
        {
            if (basicTimer.IsRunning && !basicTimer.IsPaused)
            {
                basicTimer.AddTime(Time.deltaTime);
            }
            yield return null;
        }
        runningTimers.Remove(timer);
    }

    public void StopTimer(ITimer timer)
    {
        if (runningTimers.TryGetValue(timer, out Coroutine timerCoroutine))
        {
            StopCoroutine(timerCoroutine);
            runningTimers.Remove(timer);
            timer.Stop();
        }
    }
}
