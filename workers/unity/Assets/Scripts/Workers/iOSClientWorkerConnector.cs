using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
using Improbable.Gdk.PlayerLifecycle;
using UnityEngine;

public class iOSClientWorkerConnector : MobileWorkerConnector
{
    public const string WorkerType = "iOSClient";

    [SerializeField] private string ipAddress;
    [SerializeField] private bool shouldConnectLocally;

    private async void Start()
    {
        await Connect(WorkerType, new ForwardingDispatcher()).ConfigureAwait(false);
    }

    protected override ConnectionService GetConnectionService()
    {
        return shouldConnectLocally ? ConnectionService.Receptionist : ConnectionService.AlphaLocator;
    }

    protected override void HandleWorkerConnectionEstablished()
    {
        PlayerLifecycleHelper.AddClientSystems(Worker.World);
    }

    protected override string GetHostIp()
    {
#if UNITY_IOS
            if (!string.IsNullOrEmpty(ipAddress))
            {
                return ipAddress;
            }

            return RuntimeConfigDefaults.ReceptionistHost;
#else
        throw new PlatformNotSupportedException(
            $"{nameof(iOSClientWorkerConnector)} can only be used for the iOS platform. Please check your build settings.");
#endif
    }
}