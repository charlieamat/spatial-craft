using Improbable;
using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using UnityEngine;

internal static class SnapshotGenerator
{
    public struct Arguments
    {
        public string OutputPath;
    }

    public static void Generate(Arguments arguments)
    {
        Debug.Log("Generating snapshot.");
        var snapshot = CreateSnapshot();

        Debug.Log($"Writing snapshot to: {arguments.OutputPath}");
        snapshot.WriteToFile(arguments.OutputPath);
    }

    private static Snapshot CreateSnapshot()
    {
        var snapshot = new Snapshot();

        AddPlayerSpawner(snapshot);
        AddResources(snapshot);
        return snapshot;
    }

    private static void AddPlayerSpawner(Snapshot snapshot)
    {
        var serverAttribute = UnityGameLogicConnector.WorkerType;
            
        var template = new EntityTemplate();
        template.AddComponent(new Position.Snapshot(), serverAttribute);
        template.AddComponent(new Metadata.Snapshot { EntityType = "PlayerCreator" }, serverAttribute);
        template.AddComponent(new Persistence.Snapshot(), serverAttribute);
        template.AddComponent(new PlayerCreator.Snapshot(), serverAttribute);

        template.SetReadAccess(UnityClientConnector.WorkerType, UnityGameLogicConnector.WorkerType, AndroidClientWorkerConnector.WorkerType, iOSClientWorkerConnector.WorkerType);
        template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

        snapshot.AddEntity(template);
    }

    private static void AddResources(Snapshot snapshot)
    {
        var serverAttribute = UnityGameLogicConnector.WorkerType;
        
        var template = new EntityTemplate();
        template.AddComponent(new Position.Snapshot(), serverAttribute);
        template.AddComponent(new Metadata.Snapshot { EntityType = "Resource" }, serverAttribute);
        template.AddComponent(new Persistence.Snapshot(), serverAttribute);
        template.AddComponent(new Com.Infalliblecode.Resource.Snapshot(), serverAttribute);
        
        template.SetReadAccess(UnityClientConnector.WorkerType, UnityGameLogicConnector.WorkerType, AndroidClientWorkerConnector.WorkerType, iOSClientWorkerConnector.WorkerType);
        template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

        snapshot.AddEntity(template);
    }
}