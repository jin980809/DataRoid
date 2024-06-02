using UnityEngine;

public class RotateOnTrigger : MonoBehaviour
{
    public GameObject targetObject;
    public float rotationSpeed = 10f;
    public bool rotateLeft = true;

    private bool shouldRotate = false;

    void Update()
    {
        if (shouldRotate)
        {
            float direction = rotateLeft ? 1 : -1;
            float rotationAmount = direction * rotationSpeed * Time.deltaTime;
            targetObject.transform.Rotate(0, rotationAmount, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldRotate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldRotate = false;
        }
    }
}