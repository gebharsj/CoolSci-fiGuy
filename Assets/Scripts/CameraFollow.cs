using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float cameraSpeed;
    public Vector3 cameraOffset;
    
	void Start ()
    {
        player = GameObject.Find("Player");
	}
	void FixedUpdate ()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x + cameraOffset.x, player.transform.position.y + cameraOffset.y, player.transform.position.z + cameraOffset.z), Time.deltaTime * cameraSpeed);
    }
}
