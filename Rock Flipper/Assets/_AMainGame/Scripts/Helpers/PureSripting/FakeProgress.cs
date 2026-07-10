using UnityEngine;
using Random = System.Random;

[System.Serializable]
public class FakeProgress
{
    [SerializeField, Min(0)]
    private float minSpeed = 0.1f;
    [SerializeField, Min(0)]
    private float maxSpeed = 0.5f;
    [SerializeField]
    private float speedScale = 1;

    [Space]
    [SerializeField, Min(0)]
    private float minSpeedChangeThreshold = 0.1f;
    [SerializeField, Min(0)]
    private float maxSpeedChangeThreshold = 0.5f;

    private float progress = 0;
    private float currentSpeed = -1;
    private float currentSpeedChangeThreshold = -1;
    private float timeAtLastSpeedChange = 0;
    private float time;
    private Random random = null;

    [field: System.NonSerialized]
    public bool Inited { get; private set; } = false;

    public float Progress => progress;

    public float SpeedScale { get => speedScale; set => speedScale = value; }

    public void AddValue(float value)
    {
        ///
        progress += value;

        ///
        progress = Mathf.Clamp01(progress);
    }

    public void Update(float deltaTime)
    {
        ///
        if (!Inited)
        {
            return;
        }

        ///
        time += deltaTime;

        // currentSpeedChangeThreshold
        if (currentSpeedChangeThreshold < 0)
        {
            currentSpeedChangeThreshold = random.Range(minSpeedChangeThreshold, maxSpeedChangeThreshold);
        }

        // speed
        if (currentSpeed <= 0 || currentSpeedChangeThreshold <= (time - timeAtLastSpeedChange))
        {
            ///
            currentSpeed = random.Range(minSpeed, maxSpeed);

            ///
            timeAtLastSpeedChange = time;
            currentSpeedChangeThreshold = random.Range(minSpeedChangeThreshold, maxSpeedChangeThreshold);
        }

        ///
        progress += currentSpeed * deltaTime * speedScale;
        progress = Mathf.Clamp01(progress);
    }

    public void Reset(int randomSeed)
    {
        ///
        Inited = true;

        ///
        random = new Random(randomSeed);

        ///
        progress = 0;
        currentSpeed = -1;
        currentSpeedChangeThreshold = -1;
        timeAtLastSpeedChange = 0;
        time = 0;
    }

    public void Reset()
    {
        Reset(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
    }
}
