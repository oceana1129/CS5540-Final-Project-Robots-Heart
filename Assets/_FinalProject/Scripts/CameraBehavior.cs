using UnityEngine;
using UnityEngine.EventSystems;

public class CameraBehavior : MonoBehaviour
{
    public GameObject sun;                              // sun gameobject
    public float revolutionSpeed = 5.0f;                // revolution speed of sun
    public float movementSpeed = 0.5f;                  // movement between panning in and out
    public float distanceMin = 20.0f;                   // minimum distance away from sun
    public float distanceMax = 100.0f;                  // maximum distance away from sun
    private float step;                                 // calcuated step based on time
    private bool sunFollow = false;                     // whether camera is currently following the sun
    private Vector3 targetPosition;                     // where camera needs to move next
    private Vector3 originPosition;                     // camera starting position at start of game

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (sun == null || sun.CompareTag("Sun") == false) {                            // sun gameObject was added and has appropriate tag
            sun = GameObject.FindGameObjectWithTag("Sun");                                  // assign appropriate sun to variable
        }

        if (sun == null) {                                                              // verify sun was assigned
            Debug.LogError("Sun object not found in scene");                                // log error
            return;                                                                         // stop running
        }

        Debug.Log("Sun object found and assigned to " + gameObject.name);               // log sun was verified

        Vector3 direction = (transform.position - sun.transform.position).normalized;   // max distance from the sun
        transform.position = sun.transform.position + direction * distanceMax;

        transform.LookAt(sun.transform);                                                // looking at the sun

        originPosition = transform.position;                                            // set the origin position
        targetPosition = originPosition;                                                // set the target position
    }

    // Update is called once per frame
    void Update()
    {
        step = Time.deltaTime;                                                          // calculate step

        // user inputs
        if (Input.GetKeyDown("space")) {                                                // if space key was pressed
            Debug.Log("Space key was pressed");                                             // log
            pressSpaceKey();                                                                // space key method
        }

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI()) {                        // if left button clicked
            Debug.Log("Mouse clicked at position: " + Input.mousePosition);                 // log
            pressSpaceKey();                                                                // space key method
        }

        if (Input.touchCount > 0 && !IsPointerOverUI()) {                               // if mobile tapped on screen
                Touch touch = Input.GetTouch(0);                                                // get touch input
                if (touch.phase == TouchPhase.Began) {                                          // if touch phase started
                    Debug.Log("Screen tap detected at position: " + touch.position);                // log
                    pressSpaceKey();                                                                // space key method
            }
        }

        // move the camera to target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        transform.LookAt(sun.transform);                                                // look at sun

        if (sunFollow) {                                                                // if camera is following the sun
            PlanetaryRevolution();                                                          // have camera revolve
        }
    }

    void pressSpaceKey() {
        sunFollow = !sunFollow;                                                         // toggle follow state
        Debug.Log(sunFollow ? "Camera is following the sun" : "Camera is not following the sun");
        UpdateTargetPosition();                                                         // update target position
    }

     private bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {                            // is pointer over the UI
            return true;
        }

        if (Input.touchCount > 0) {                                                     // is pointer over the UI in mobile
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
                return true;
            }
        }

        return false;
    }

    // used to revolve around the sun
    void PlanetaryRevolution() {
        transform.RotateAround(sun.transform.position, Vector3.up, revolutionSpeed * step); // rotate around the sun object based on step
        UpdateTargetPosition();                                                             // update position
    }


    void UpdateTargetPosition()
    {
        Vector3 direction = (transform.position - sun.transform.position).normalized;   // direction away from Sun
                                                                                        // note normalizing ensures the vectors magnitude is always 1
        if (sunFollow) {                                                                // if following the sun
            targetPosition = sun.transform.position + direction * distanceMin;              // move towards the sun and stops at min distance
        } else {                                                                        // not following the sun
            targetPosition = originPosition;                                                // move to origin position
        }
    }
}
