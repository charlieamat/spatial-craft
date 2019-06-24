using System.Collections;
using Com.Infalliblecode;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

public class ResourceCollectionHandler : MonoBehaviour
{
    private static readonly float RespawnTimeout = 3;
    
    [Require] private ResourceCommandReceiver _receiver;
    [Require] private ResourceWriter _writer;

    private void OnEnable()
    {
        _receiver.OnCollectRequestReceived += OnCollect;
    }

    private void OnCollect(Com.Infalliblecode.Resource.Collect.ReceivedRequest request)
    {
        UpdateCollected(true);
        StartCoroutine(ReSpawnTimer());
    }

    private IEnumerator ReSpawnTimer()
    {
        yield return new WaitForSeconds(RespawnTimeout);
        UpdateCollected(false);
    }

    private void UpdateCollected(bool value)
    {
        _writer.SendUpdate(
            new Com.Infalliblecode.Resource.Update
            {
                Collected = new Option<BlittableBool>(value)
            });
    }
}