using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask resourceLayer;
    [SerializeField] private int distance = 3;

    private void Update()
    {
        if (!Input.GetButtonDown("Fire1")) return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, distance, resourceLayer.value))
        {
            var resource = hit.transform.GetComponent<Resource>();
            Assert.IsNotNull(resource, $"GameObjects on the {resourceLayer.ToString()} layer must have a Resource component.");
            resource.Collect();
        }
    }
}