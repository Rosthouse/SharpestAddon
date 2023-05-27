

using System;
using Godot;

public static class PhysicsDirectSpaceState3DExtensions
{

  public struct PhysicsRayQueryResult3D
  {
    public readonly Variant collider; // The colliding object.
    public readonly int colliderId; // The colliding object's ID.
    public readonly Vector3 normal; // The object's surface normal at the intersection point, or Vector3(0, 0, 0) if the ray starts inside the shape and Godot.PhysicsRayQueryParameters3D.HitFromInside is true.
    public readonly Vector3 position; // The intersection point.
    public readonly Rid rid; // The intersecting object's Godot.Rid.
    public readonly int shape;// The shape index of the colliding shape.

    public PhysicsRayQueryResult3D(Variant collider, int colliderId, Vector3 normal, Vector3 position, Rid rid, int shape)
    {
      this.collider = collider;
      this.colliderId = colliderId;
      this.normal = normal;
      this.position = position;
      this.rid = rid;
      this.shape = shape;
    }


    public CollisionObject3D GetCollisionObject3D()
    {
      return this.collider.AsGodotObject() as CollisionObject3D;
    }
  }
  public static Nullable<PhysicsRayQueryResult3D> IntersectRayResult(this PhysicsDirectSpaceState3D dss, PhysicsRayQueryParameters3D parameters)
  {
    var result = dss.IntersectRay(parameters);
    if (result.Count == 0)
    {
      return null;
    }
    return new PhysicsRayQueryResult3D(
      collider: result["collider"],
      colliderId: result["collider_id"].AsInt32(),
      normal: result["normal"].AsVector3(),
      position: result["position"].AsVector3(),
      rid: result["rid"].AsRid(),
      shape: result["shape"].AsInt32()
    );
  }

  public static Nullable<PhysicsRayQueryResult3D> CastRayFromCamera(this PhysicsDirectSpaceState3D dss, uint collisionMask = uint.MaxValue, Godot.Collections.Array<Rid> exclude = null, float rayLength = 1000, bool collideWithAreas = false)
  {
    var st = (SceneTree)Engine.GetMainLoop();
    var vp = st.CurrentScene.GetViewport();
    var p = vp.GetMousePosition();
    PhysicsRayQueryParameters3D query = new PhysicsRayQueryParameters3D();
    query.From = vp.GetCamera3D().ProjectRayOrigin(p);
    query.To = query.From + vp.GetCamera3D().ProjectRayNormal(p) * rayLength;
    query.CollisionMask = collisionMask;
    query.Exclude = exclude;
    query.CollideWithAreas = collideWithAreas;
    var result = dss.IntersectRayResult(query);
    return result;
  }
}

