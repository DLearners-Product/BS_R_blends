using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningClouds : MonoBehaviour
{
    public Transform _endPoint;
    Vector3 endPosition;
    float moveTime = 0;

    void Start()
    {
        Invoke(nameof(MoveCloud), moveTime);
    }

    void MoveCloud()
    {
        endPosition = new Vector3(_endPoint.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        Utilities.Instance.ANIM_MoveLocal(transform, endPosition, 50f, () => { Destroy(gameObject); });
    }

    public void SetEndPoint(Transform endPoint, float _moveTime = -1f)
    {
        moveTime = (_moveTime == -1) ? Random.Range(1f, 5f) : _moveTime;
        _endPoint = endPoint;
    }
}
