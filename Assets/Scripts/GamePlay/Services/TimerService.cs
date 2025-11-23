using UnityEngine;

public class TimerService : MonoBehaviour
{
    public float CurrentTime { get; private set; } = 0f;
    public bool IsRunning { get; private set; } = false;

    private void Start()
    {
        IsRunning = true;
    }

    void Update()
    {
        if (IsRunning)
        {
            CurrentTime += Time.deltaTime;
        }
    }
}
