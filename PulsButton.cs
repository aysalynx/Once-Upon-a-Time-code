#if PLAYABLE_AD
using UnityEngine;

public class PulseButton : MonoBehaviour
{
    public float scaleMin = 0.9f;
    public float scaleMax = 1.1f;
    public float speed = 2f;

    Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        float scale = Mathf.Lerp(scaleMin, scaleMax, t);
        transform.localScale = baseScale * scale;
    }
}
#endif
