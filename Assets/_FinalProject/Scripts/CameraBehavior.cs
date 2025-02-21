using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [Header("Camera")]
    public Transform player;     // reference to the player's transform
    public Vector3 offset = new Vector3(0, 2, -5);  // camera offset
    public float smoothSpeed = 0.125f; // smoothing speed for the camera movement
    
    [Header("Camera Level Boundaries")]
    public bool useBounds = false;
    public Vector2 minBounds;  // min X and Y camera position
    public Vector2 maxBounds;  // max X and Y camera position

    void Start()
    {
        if (!player)                                                // player not assigned in inspector
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");  // find the player in the scene
            if (playerObj)                                                      // if player object exists
                player = playerObj.transform;                                       // get the transform
        }

        if (!player)                                                // player is still not found
        {
            Debug.LogError("Player not found in scene!");
            enabled = false;                                        // disable script
        }

        offset = transform.position - player.position;
    }

    // fixed update for physics based movement
    void FixedUpdate()  
    {
        if (!player) return;                                        // if there is no player then return

        Vector3 desiredPosition = player.position + offset;         // find the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);  // smooth the camera position

        
        if (useBounds)                                              // if level boundaries are provided then clamp movement
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);     // clamp x
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);     // clamp y
        }
        
        transform.position = smoothedPosition;                      // set camera to the smoothed position
    }

    void OnDrawGizmos()
    {
        if (useBounds)  // if boundaries are enabled
        {
            Gizmos.color = Color.red;  // set boundary color to red

            // draw a rectangle in the scene for camera bounds
            Vector3 bottomLeft = new Vector3(minBounds.x, minBounds.y, transform.position.z);
            Vector3 bottomRight = new Vector3(maxBounds.x, minBounds.y, transform.position.z);
            Vector3 topLeft = new Vector3(minBounds.x, maxBounds.y, transform.position.z);
            Vector3 topRight = new Vector3(maxBounds.x, maxBounds.y, transform.position.z);

            Gizmos.DrawLine(bottomLeft, bottomRight);  // bottom edge
            Gizmos.DrawLine(bottomRight, topRight);    // right edge
            Gizmos.DrawLine(topRight, topLeft);        // top edge
            Gizmos.DrawLine(topLeft, bottomLeft);      // left edge
        }
    }
}
