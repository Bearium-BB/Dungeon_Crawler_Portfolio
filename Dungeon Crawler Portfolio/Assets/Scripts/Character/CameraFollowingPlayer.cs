using UnityEngine;

public class CameraFollowingPlayer : MonoBehaviour
{
    public Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x,transform.position.y, player.position.z + -7);
    }
}
