using UnityEngine;

public class Seesaw : MonoBehaviour
{
    public float angle = 20f;
    public float speed = 2f;

    void Update()
    {
        float rotation = Mathf.Sin(Time.time * speed) * angle;
        transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }
}