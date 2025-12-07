using UnityEngine;

public class MushroomWiggle : MonoBehaviour
{
    public float amplitude = 0.03f;     
    public float frequency = 1.5f;     
    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float x = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(x, 0f, 0f);
    }
}
