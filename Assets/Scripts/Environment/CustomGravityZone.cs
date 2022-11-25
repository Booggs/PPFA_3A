using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravityZone : MonoBehaviour
{
    [SerializeField] private Vector3 _gravityDirection = new Vector3(0.0f, -1.0f, 0.0f);
    [SerializeField] private float _gravityStrength = 9.81f;

    private BoxCollider _boxCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponentInChildren<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_boxCollider == null) return;
        var customGravity = other.GetComponent<CustomGravity>();
        customGravity.ChangeGravity(new CustomGravity.GravityChangeArgs(_gravityStrength, _gravityDirection));
    }
}
