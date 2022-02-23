using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DashObject : MonoBehaviour
{
    protected Transform _owner;
    public Transform Owner => _owner;

    public virtual void Initialize(Transform owner)
    {
        this._owner = owner;
    }
}
