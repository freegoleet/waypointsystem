
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class Waypoints : MonoBehaviour
{
    public List<GameObject> waypoints = new List<GameObject>();
    public GameObject waypoint;
    private bool play = true;
    private bool repeat = true;
    //private bool pause = false;

    public float speed = 5f;

    private float fraction = 0;
    private float distance = 0;
    private int points = 0;

    private Vector3 nextPos;
    private Vector3 currentPos;

    private GameObject sphere = null;

    public void Update() {
        LerpEntity();
    }

    public void AddNewWaypoint()
    {
        GameObject newWaypoint = Instantiate(waypoint, waypoints[waypoints.Count() - 1].transform.position, waypoints[waypoints.Count() - 1].transform.rotation, transform);
        waypoints.Add(newWaypoint);
    }


    private void LerpEntity()
    {
        if (play == true)
        {
            if (sphere == null)
            {
                if (GameObject.Find("Sphere"))
                {
                    sphere = GameObject.Find("Sphere");
                }
                else
                {
                    sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                }
                currentPos = waypoints[0].transform.position;
                nextPos = waypoints[1].transform.position;
            }
            

            if (sphere.transform.position != nextPos && currentPos != null && nextPos != null)
            {
                if (repeat == true)
                {
                    distance = Vector3.Distance(currentPos, nextPos);
                    repeat = false;
                }
                if (fraction < 1)
                {
                    fraction += Time.deltaTime / distance * speed;
                    sphere.transform.position = Vector3.Lerp(currentPos, nextPos, fraction);
                }
            }
            else if (waypoints.ElementAtOrDefault(points + 1) != null)
            {
                currentPos = waypoints[points].transform.position;
                nextPos = waypoints[points + 1].transform.position;
                fraction = 0;
                points++;
                repeat = true;
            }
            else
            {
                currentPos = waypoints[points].transform.position;
                nextPos = waypoints[0].transform.position;
                fraction = 0;
                points = 0;
                repeat = true;
            }
        }
    }

    private void OnDrawGizmos() {
        for (int i = 0; i < waypoints.Count; i++) {
            if (waypoints.ElementAtOrDefault(i+1) != null) {
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
            }
            else {
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[0].transform.position);
            }
        }
    }
}
