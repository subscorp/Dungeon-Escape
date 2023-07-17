using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    private static FrameRateManager _instance;
    public static FrameRateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                //Debug.LogError("FrameRateManager is NULL");
            }
            return _instance;
        }
    }

    [Header("Frame Settings")]
    //int maxRate = 9999;
    int targetFrameRate = 45; //60;

    public float CurrentFrameRate { get; private set; }
    private float[] frameRates;
    private int frameRateIndex;
    private float measurementInterval = 1f; // Measurement interval in seconds
    float avg = 0F;

    void Awake()
    {
        _instance = this;
        QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = maxRate;
        Application.targetFrameRate = targetFrameRate;

        /*
        if (CanSupportFrameRate(targetFrameRate))
        {
            Debug.Log("in 60 fps");
            Application.targetFrameRate = targetFrameRate;
        }
        else
        {
            Debug.Log("in 40 fps");
            targetFrameRate = 40;
            Application.targetFrameRate = targetFrameRate;
        }
        */
    }

    void Update()
    {
        avg += ((Time.deltaTime / Time.timeScale) - avg) * 0.03f; //run this every frame
        CurrentFrameRate = (1F / avg); //display this value
    }

    bool CanSupportFrameRate(int frameRate)
    {
        return Screen.currentResolution.refreshRate >= frameRate;
    }
}




/*
using System.Collections;
using System.Threading;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    private static FrameRateManager _instance;
    public static FrameRateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("FrameRateManager is NULL");
            }
            return _instance;
        }
    }

    [Header("Frame Settings")]
    int MaxRate = 9999;
    int[] targetFrameRates = { 30, 40, 60 };
    int targetFrameRateIndex;
    public float CurrentFrameTime { get; private set; }
    public float CurrentFrameRate { get; private set; }
    int frameCount;
    float elapsedTime;

    void Awake()
    {
        _instance = this;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        CurrentFrameTime = Time.realtimeSinceStartup;
        targetFrameRateIndex = GetTargetFrameRateIndex();
        SetTargetFrameRate();
        StartCoroutine("WaitForNextFrame");
    }

    void SetTargetFrameRate()
    {
        Application.targetFrameRate = targetFrameRates[targetFrameRateIndex];
    }

    int GetTargetFrameRateIndex()
    {
        int refreshRate = Screen.currentResolution.refreshRate;
        int targetIndex = 0;
        if (refreshRate >= 60)
            targetIndex = 2;
        else if (refreshRate >= 40)
            targetIndex = 1;
        return targetIndex;
    }

    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            CurrentFrameTime += 1.0f / targetFrameRates[targetFrameRateIndex];
            var t = Time.realtimeSinceStartup;
            var sleepTime = CurrentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < CurrentFrameTime)
                t = Time.realtimeSinceStartup;

            frameCount++;
            elapsedTime += Time.unscaledDeltaTime;
            if (elapsedTime >= 1.0f)
            {
                CurrentFrameRate = frameCount / elapsedTime;
                frameCount = 0;
                elapsedTime = 0f;
                UpdateTargetFrameRate();
            }
        }
    }

    void UpdateTargetFrameRate()
    {
        int targetIndex = GetTargetFrameRateIndex();
        if (targetIndex != targetFrameRateIndex)
        {
            targetFrameRateIndex = targetIndex;
            SetTargetFrameRate();
        }
    }
}
*/