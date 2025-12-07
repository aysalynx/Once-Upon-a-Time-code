using UnityEngine;

public class BunnyIdle : MonoBehaviour
{
    [Header("Float")]
    public float floatAmplitude = 0.03f;
    public float floatFrequency = 1.2f;

    [Header("Squash")]
    public float squashAmount = 0.04f;
    public float squashFrequency = 2.4f;

    Vector3 startPos;
    Vector3 baseScale;

    void Start()
    {
        startPos = transform.localPosition;
        baseScale = transform.localScale;
    }

    void Update()
    {
        
        float y = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.localPosition = startPos + new Vector3(0f, y, 0f);

        
        float s = Mathf.Sin(Time.time * squashFrequency) * squashAmount;
        float scaleX = baseScale.x * (1f + s);
        float scaleY = baseScale.y * (1f - s);
        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}
