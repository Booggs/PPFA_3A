using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGravityZone : MonoBehaviour
{
    [SerializeField]
    private bool _zoneEnabled = false;

    private BoxCollider _boxCollider = null;

    public bool ZoneEnabled => _zoneEnabled;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponentInChildren<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_boxCollider == null || _zoneEnabled == false) return;
        var customGravity = other.GetComponentInParent<CustomPlayerGravity>();
        customGravity.EnableInvertGravity(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_boxCollider == null || _zoneEnabled == false) return;
        var customGravity = other.GetComponentInParent<CustomPlayerGravity>();
        customGravity.EnableInvertGravity(false);
    }

    public void ToggleZone()
    {
        _zoneEnabled = !_zoneEnabled;
        Collider[] overlappingColliders = Physics.OverlapBox(_boxCollider.transform.position, _boxCollider.size / 2);
        
        foreach (var collider in overlappingColliders)
        {
            CustomPlayerGravity customGravity;
            if ((customGravity = collider.GetComponentInParent<CustomPlayerGravity>()) != null)
            {
                Debug.Log("Found custom gravity");
                customGravity.EnableInvertGravity(_zoneEnabled);
            }
        }
    }
}
