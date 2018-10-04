using UnityEngine;
using System.Collections;


public class Unit : MonoBehaviour
{


    public Transform target;
    public Object beacon;
    float speed = 1;
    Vector3[] path;
    int targetIndex;
    public Animator anim;

    private Transform _myTransform;
    private Vector3 _lastPosition;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    private void Awake()
    {
        _myTransform = transform;
        _lastPosition = _myTransform.position;
    }

    public void Move()
    {
        StartCoroutine(FollowPath());
    }


    void Update()
    {
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        if (_lastPosition == _myTransform.position)
        {
            Debug.Log("Did not move");
            anim.SetBool("Run", false);

        }
        else
        {
            Debug.Log("Moved");
           anim.SetBool("Run", true);
        }
        _lastPosition = _myTransform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            DestroyObject(other);
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
                
            }

            transform.LookAt(currentWaypoint);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
    public void Path()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }
}
