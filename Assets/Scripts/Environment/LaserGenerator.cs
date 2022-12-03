using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGenerator : MonoBehaviour
{
    [SerializeField] 
    private float _laserDistance = 100.0f;

    [SerializeField] 
    private LineRenderer _lineRenderer = null;

    [SerializeField] 
    private BoxCollider _laserCollider = null;

    private LayerMask _layerMask;
    
    private void Start()
    {
        _lineRenderer.SetPosition(0, transform.position);
        _layerMask = (1 << 6);
        _layerMask |= (1 << 8);
        _layerMask = ~_layerMask;
    }

    private void Update()
    {
        _lineRenderer.SetPosition(0, transform.position);
        bool wallHit;
        wallHit = Physics.Raycast(transform.position, transform.forward * _laserDistance, out RaycastHit hitInfo, _laserDistance, _layerMask);

        _lineRenderer.SetPosition(1, wallHit ? hitInfo.point : transform.position + transform.forward * _laserDistance);
        _laserCollider.transform.position = transform.position + transform.forward * (hitInfo.distance / 2);
        _laserCollider.size = new Vector3(0.25f, 1.0f, hitInfo.distance);
    }
}
