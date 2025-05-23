using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    float speed = 1;
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
