using System.Linq;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Unity.Entities;

public static class EntityExtensions
{
    private const string _workerAttributeFormat = "workerId:{0}";

    public static bool HasMetadataValue(this SpatialOSEntity entity, string value) =>
        entity.HasComponent<Metadata.Component>() &&
        entity.GetComponent<Metadata.Component>().EntityType == value;

    public static bool HasWriteAuthority<T>(this SpatialOSEntity entity, Worker worker)
        where T : struct, ISpatialComponentData, IComponentData
    {
        if (!entity.HasComponent<T>()) return false;

        var positionComponent = entity.GetComponent<Position.Component>();
        var acl = entity.GetComponent<EntityAcl.Component>();

        acl.ComponentWriteAcl.TryGetValue(positionComponent.ComponentId, out var requirementSet);

        return requirementSet.AttributeSet.Any(set =>
            set.Attribute.Contains(string.Format(_workerAttributeFormat, worker.WorkerId)));
    }
}