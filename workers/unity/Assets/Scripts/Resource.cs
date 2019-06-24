using Com.Infalliblecode;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Resource : MonoBehaviour
{
    [Require] private EntityId _entityId;
    [Require] private ResourceCommandSender _sender;
    [Require] private ResourceReader _reader;

    private Renderer _renderer;

    public void Collect()
    {
        if (_reader.Data.Collected) return;
        
        Debug.Log($"Collecting a Resource {_entityId}");
        
        _sender.SendCollectCommand(_entityId, new CollectRequest(),
            response => Debug.Log($"Collected a Resource {_entityId}"));
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {

        _renderer.enabled = !_reader.Data.Collected;
    }
}
