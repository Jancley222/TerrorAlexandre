using UnityEngine;

public class MenuRotation : MonoBehaviour
{
    public float angle = 3f;
    public float speed = 0.7f;

    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        float rot =
            Mathf.Sin(Time.time * speed) * angle +
            Mathf.Sin(Time.time * speed * 0.37f) * (angle * 0.35f);

        rect.localRotation = Quaternion.Euler(0, 0, rot);
    }
}