using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGravityZone : MonoBehaviour
{
    private BoxCollider _boxCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponentInChildren<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_boxCollider == null) return;
        var customGravity = other.GetComponentInParent<CustomPlayerGravity>();
        customGravity.EnableInvertGravity(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_boxCollider == null) return;
        var customGravity = other.GetComponentInParent<CustomPlayerGravity>();
        customGravity.EnableInvertGravity(false);
    }
}
