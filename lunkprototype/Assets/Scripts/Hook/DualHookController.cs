using UnityEngine;

public class DualHookController : MonoBehaviour
{
    public GameObject leftHookPrefab;
    public GameObject rightHookPrefab;
    public Transform leftHookPosition;
    public Transform rightHookPosition;
    public float hookSpeed = 20f;
    public float maxDistance = 100f;
    public LayerMask hookableLayer;
    public LayerMask Obstacles;

    private GameObject leftHook;
    private GameObject rightHook;
    private Transform hookTransform;
    private bool hookAttached;
    private Rigidbody hookedRigidbody;
    private Vector3 hookedPoint;

    private KeyCode leftHookKey = KeyCode.Q;
    private KeyCode rightHookKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(leftHookKey))
        {
            Hook(leftHookKey, leftHookPosition, ref leftHook);
        }
        else if (Input.GetKeyDown(rightHookKey))
        {
            Hook(rightHookKey, rightHookPosition, ref rightHook);
        }

        if (hookAttached)
        {
            if (hookedRigidbody)
            {
                hookedRigidbody.MovePosition(hookedPoint);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, hookedPoint, hookSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, hookedPoint) < 0.1f)
            {
                ResetHook();
            }
        }
    }

    void Hook(KeyCode hookKey, Transform hookPosition, ref GameObject hook)
    {
        if (hookAttached)
        {
            ResetHook();
            return;
        }

        Vector3 hookDirection;
        if (hookKey == leftHookKey)
        {
            hookDirection = -transform.right;
        }
        else
        {
            hookDirection = transform.right;
        }

        RaycastHit hit;
        if (Physics.Raycast(hookPosition.position, hookDirection, out hit, maxDistance, hookableLayer))
        {
            hookTransform = hit.transform;
            hookAttached = true;
            hookedRigidbody = hit.collider.GetComponent<Rigidbody>();
            hookedPoint = hit.point;

            if (hookedRigidbody)
            {
                hookedRigidbody.isKinematic = true;
            }
        }
        else
        {
            // Spawn hook game object if there's no hookable object in range
            if (hook == null)
            {
                hook = Instantiate(hookKey == leftHookKey ? leftHookPrefab : rightHookPrefab, hookPosition.position, Quaternion.identity);
                hook.GetComponent<Rigidbody>().velocity = hookDirection * hookSpeed;
            }
        }
    }

    void ResetHook()
    {
        hookAttached = false;

        if (hookedRigidbody)
        {
            hookedRigidbody.isKinematic = false;
            hookedRigidbody = null;
        }

        hookTransform = null;
    }
}
