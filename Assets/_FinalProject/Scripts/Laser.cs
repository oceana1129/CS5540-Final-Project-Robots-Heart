using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);   
            }
        }
        else
        {
            lr.SetPosition(1, transform.forward*5000);
        }
    }
}
