using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.Subscriptions;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerGameObjectCreator : IEntityGameObjectCreator
{
    private readonly string _clientPlayerPrefabPath =
        $"Prefabs/UnityClient/Client/{UnityGameLogicConnector.PlayerEntityType}";

    private readonly Worker _worker;
    private readonly IEntityGameObjectCreator _nonPlayerCreator;
    private readonly GameObject _clientPlayerPrefab;
    private readonly Dictionary<EntityId, GameObject> _entityGameObjects = new Dictionary<EntityId, GameObject>();

    public PlayerGameObjectCreator(Worker worker, IEntityGameObjectCreator nonPlayerCreator = null)
    {
        _nonPlayerCreator = nonPlayerCreator ??
                            new GameObjectCreatorFromMetadata(worker.WorkerType, worker.Origin, worker.LogDispatcher);
        _worker = worker;
        _clientPlayerPrefab = Resources.Load<GameObject>(_clientPlayerPrefabPath);
    }

    public void OnEntityCreated(SpatialOSEntity entity, EntityGameObjectLinker linker)
    {
        if (!entity.HasComponent<Metadata.Component>())
        {
            Debug.LogWarning("Entity doesn't have metadata. Skipping GameObject creation.");
            return;
        }

        if (IsClientPlayerEntity(entity))
        {
            var gameObject = Object.Instantiate(_clientPlayerPrefab, _worker.Origin, Quaternion.identity);
            gameObject.name = GetGameObjectName(entity);
            
            _entityGameObjects.Add(entity.SpatialOSEntityId, gameObject);
            
            linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, gameObject);
        }
        else
        {
            _nonPlayerCreator.OnEntityCreated(entity, linker);
        }
    }

    public void OnEntityRemoved(EntityId entityId)
    {
        Debug.Log($"Deleting GameObject for Entity {entityId.Id}.");
        
        if (_entityGameObjects.TryGetValue(entityId, out var gameObject))
        {
            _entityGameObjects.Remove(entityId);
            Object.Destroy(gameObject);
        }
        else
        {
            _nonPlayerCreator.OnEntityRemoved(entityId);
        }
        Debug.Log($"Deleted GameObject for Entity {entityId}");
    }

            private string GetGameObjectName(SpatialOSEntity entity)
    {
        return $"{_clientPlayerPrefab.name}(SpatialOS: {entity.SpatialOSEntityId}, Worker: {_worker.WorkerType})";
    }

    private bool IsClientPlayerEntity(SpatialOSEntity entity)
    {
        return entity.HasMetadataValue(UnityGameLogicConnector.PlayerEntityType) &&
               entity.HasWriteAuthority<Position.Component>(_worker);
    }
}