using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Worker.CInterop;

public class UnityClientConnector : DefaultWorkerConnector
{
    public const string WorkerType = "UnityClient";
        
    private async void Start()
    {
        await Connect(WorkerType, new ForwardingDispatcher()).ConfigureAwait(false);
    }

    protected override void HandleWorkerConnectionEstablished()
    {
        PlayerLifecycleHelper.AddClientSystems(Worker.World);
        TransformSynchronizationHelper.AddClientSystems(Worker.World);
        GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World);
    }

    protected override string SelectDeploymentName(DeploymentList deployments)
    {
        return deployments.Deployments[0].DeploymentName;
    }
}