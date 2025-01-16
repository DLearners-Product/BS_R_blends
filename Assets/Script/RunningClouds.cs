using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningClouds : MonoBehaviour
{
    public Transform spawnPoint, _endPoint;
    Vector3 endPosition;

    void Start()
    {
        Invoke(nameof(MoveCloud), Random.Range(1f, 5f));
    }

    void MoveCloud()
    {
        endPosition = new Vector3(_endPoint.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        Utilities.Instance.ANIM_MoveLocal(transform, endPosition, 20f, () => { Destroy(gameObject); });
    }

    public void SetEndPoint(Transform endPoint)
    {
        _endPoint = endPoint;
    }
}
