using Godot;
using System;
using System.Diagnostics;

namespace rosthouse.sharpest.addon;

[Tool]
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public partial class ExtendedRemoteTransform3D : Node3D
{
  [Flags]
  public enum TransformFlags
  {
    X = 1 << 1,
    Y = 1 << 2,
    Z = 1 << 3,
  }

  [Export] private TransformFlags updatePosition;
  [Export] private TransformFlags updateRotation;
  [Export] private TransformFlags updateScale;

  [Export] private Node3D remoteTransform = null!;
  [Export] private bool useGlobalTransform;
  [Export] private bool runInEditor = false;

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (Engine.IsEditorHint() && !runInEditor)
    {
      return;
    }

    if (remoteTransform == null)
    {
      GD.PrintErr($"[{Name}] No remoteTransform is set");
      return;
    }

    UpdatePosition();
    UpdateRotation();
    UpdateScale();
  }

  private void UpdatePosition()
  {
    if (useGlobalTransform)
    {
      remoteTransform.GlobalPosition = UpdateVector(remoteTransform.GlobalPosition, GlobalPosition, updatePosition);
    }
    else
    {
      remoteTransform.Position = UpdateVector(remoteTransform.Position, Position, updatePosition);
    }
  }

  private void UpdateRotation()
  {
    if (useGlobalTransform)
    {
      remoteTransform.GlobalRotation = UpdateVector(remoteTransform.GlobalRotation, GlobalRotation, updateRotation);
    }
    else
    {
      remoteTransform.Rotation = UpdateVector(remoteTransform.Rotation, Rotation, updateRotation);
    }
  }

  private void UpdateScale()
  {
    remoteTransform.Scale = UpdateVector(remoteTransform.Scale, Scale, updateScale);
  }

  private Vector3 UpdateVector(Vector3 slave, Vector3 master, TransformFlags flags)
  {
    var l = slave;
    if ((flags & TransformFlags.X) == TransformFlags.X)
    {
      l[0] = master[0];
    }

    if ((flags & TransformFlags.Y) == TransformFlags.Y)
    {
      l[1] = master[1];
    }

    if ((flags & TransformFlags.Z) == TransformFlags.Z)
    {
      l[2] = master[2];
    }
    return l;
  }

  private string GetDebuggerDisplay()
  {
    return ToString();
  }

  public override string[] _GetConfigurationWarnings()
  {
    if (remoteTransform == null)
    {
      return ["Warning: No remote transform selected"];
    }

    return [];
  }

}
