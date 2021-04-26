using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNotMoving : MonoBehaviour
{
    private Vector3 _latePosition;
    // Start is called before the first frame update
    void Start()
    {
        _latePosition = this.transform.position;
    }

    private IEnumerator TestIfMoved()
    {
        var transformPosition = this.transform.position;
        if (Vector3.Distance(_latePosition, transformPosition) <= 3f)
        {
            Destroy(this.gameObject);
        }

        this._latePosition = transformPosition;
        yield return new WaitForSeconds(10.0f);
    }
}
