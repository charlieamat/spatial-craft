using Com.Infalliblecode;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;

public class UnityGameLogicConnector : DefaultWorkerConnector
{
    public const string WorkerType = "UnityGameLogic";
    public const string PlayerEntityType = "Player";
        
    private async void Start()
    {
        PlayerLifecycleConfig.CreatePlayerEntityTemplate = CreatePlayerEntityTemplate;
        await Connect(WorkerType, new ForwardingDispatcher()).ConfigureAwait(false);
    }

    protected override void HandleWorkerConnectionEstablished()
    {
        Worker.World.GetOrCreateManager<MetricSendSystem>();
        PlayerLifecycleHelper.AddServerSystems(Worker.World);
        TransformSynchronizationHelper.AddServerSystems(Worker.World);
        GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World);
    }

    private static EntityTemplate CreatePlayerEntityTemplate(string workerId, byte[] serializedArguments)
    {
        var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(workerId);
        var serverAttribute = WorkerType;

        var template = new EntityTemplate();
        template.AddComponent(new Position.Snapshot(), clientAttribute);
        template.AddComponent(new Metadata.Snapshot(PlayerEntityType), serverAttribute);
        template.AddComponent(new PlayerTransform.Snapshot(), clientAttribute);
        PlayerLifecycleHelper.AddPlayerLifecycleComponents(template, workerId, serverAttribute);

        template.SetReadAccess(UnityClientConnector.WorkerType, AndroidClientWorkerConnector.WorkerType, iOSClientWorkerConnector.WorkerType, serverAttribute);
        template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

        return template;
    }
}