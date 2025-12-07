using UnityEngine;

public class SquashStretch : MonoBehaviour
{
    public float amplitude = 0.05f;    
    public float frequency = 2.0f;     
    Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float s = Mathf.Sin(Time.time * frequency) * amplitude;

        float scaleX = baseScale.x * (1f + s * 0.7f);  
        float scaleY = baseScale.y * (1f - s * 0.7f);  

        transform.localScale = new Vector3(scaleX, scaleY, baseScale.z);
    }
}
