using UnityEngine;

public class Float : MonoBehaviour
{
    public float amplitude = 0.05f;  
    public float frequency = 1.2f;   
    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(0f, y, 0f);
    }
}
