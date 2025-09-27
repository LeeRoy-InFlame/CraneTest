using UnityEngine;

public class Cable : MonoBehaviour
{
    [SerializeField] private Transform _startPoint; // газоанализатор
    [SerializeField] private Transform _endPoint;   // зонд
    private LineRenderer _lr;

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.positionCount = 2;
    }

    void Update()
    {
        if (_startPoint != null && _endPoint != null)
        {
            _lr.SetPosition(0, _startPoint.position);
            _lr.SetPosition(1, _endPoint.position);
        }
    }
}

