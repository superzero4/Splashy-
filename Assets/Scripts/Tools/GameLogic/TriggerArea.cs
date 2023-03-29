using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerArea : MonoBehaviour
{
    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }
    [SerializeField] private LayerMask _layers=1;
    [SerializeField] private UnityEvent<Transform> OnEnter;
    [SerializeField] private UnityEvent<Transform> OnStay;
    [SerializeField] private UnityEvent<Transform> OnLeave;
    [SerializeField, Tooltip("If set to false, componenent will be destroyed after first exit of the collider")] private bool repeatable;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if ((_layers.value & 1 << other.gameObject.layer) != 0)
            OnEnter.Invoke(transform);
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if ((_layers.value & 1 << other.gameObject.layer) != 0)
            OnStay.Invoke(transform);
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if ((_layers.value & 1 << other.gameObject.layer) != 0)
        {
            OnLeave.Invoke(transform);
            if (!repeatable)
                Destroy(this);
        }
    }
}
