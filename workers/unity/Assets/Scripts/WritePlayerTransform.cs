using Com.Infalliblecode;
using Improbable;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

public class WritePlayerTransform : MonoBehaviour
{
    [Require] private PlayerTransformWriter _writer;

    private void Update()
    {
        var update = new PlayerTransform.Update
        {
            Position = Vector3f.FromUnityVector(transform.position),
            Rotation = Vector3f.FromUnityVector(transform.rotation.eulerAngles)
        };
        
        _writer.SendUpdate(update);
    }
}