using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningClouds : MonoBehaviour
{
    public Transform spawnPoint, _endPoint;

    void Start()
    {
        Invoke(nameof(MoveCloud), Random.Range(1f, 10f));
    }

    void MoveCloud()
    {
        Utilities.Instance.ANIM_Move(transform, new Vector3(_endPoint.position.x, transform.position.y, transform.position.z), 20f);
    }

    public void SetEndPoint(Transform endPoint)
    {
        _endPoint = endPoint;
    }
}
