using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class TriggerCollider : MonoBehaviour
{
    public UnityEvent onTrigger;
    public string triggerTag;
    public string triggerName;

    private void OnCollisionEnter(Collision collision)
    {
        print("Collision with " + collision.collider.name + ", tag: " + collision.collider.tag);
        if (collision.collider.CompareTag(triggerTag) || collision.collider.name == triggerName)
        {
            GetComponent<Collider>().enabled = false;
            onTrigger?.Invoke();
        }
    }
}
