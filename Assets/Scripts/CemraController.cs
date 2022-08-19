
using UnityEngine;

public class CemraController : MonoBehaviour
{
    public Transform player;
    float smoothSpeed = 0.125f;
    public float minX, maxX, minY, maxY;
    //public GameObject camera;
    private void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void LateUpdate()
    {
        Vector3 desiredPosiiton = new Vector3(player.position.x  , player.position.y , transform.position.z);
        Vector3 smoothedPosiiton = Vector3.Lerp(transform.position, desiredPosiiton, smoothSpeed);
        if (smoothedPosiiton.x < minX)
            smoothedPosiiton.x = minX;
        if (smoothedPosiiton.x > maxX)
            smoothedPosiiton.x = maxX;
       
        if(smoothedPosiiton.y < minY)
            smoothedPosiiton.y = minY;
        if (smoothedPosiiton.y > maxY)
            smoothedPosiiton.y = maxY;
        transform.position = smoothedPosiiton;

    }
}
