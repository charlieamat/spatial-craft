using Com.Infalliblecode;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

public class ReadPlayerTransform : MonoBehaviour
{
    [Require] private PlayerTransformReader _reader;

    private void Update()
    {
        transform.position = _reader.Data.Position.ToUnityVector();
        transform.rotation = Quaternion.Euler(_reader.Data.Rotation.ToUnityVector());
    }
}