using UnityEngine;
public class MoveCube : MonoBehaviour
{
    [Header("Movement")]
    public float multiplier;
    public float add;
    public float range;

    [Header("Rotation")]
    public float speed;
    private void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Sin(Time.time * multiplier) * range;

        transform.position = pos;

        Vector3 rot = transform.localEulerAngles;
        rot.x = Time.time * speed;
        rot.y = Time.time * speed;
        transform.localEulerAngles = rot;
    }
}