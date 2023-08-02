using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

  [Export] private Node3D remoteTransform;
  [Export] private bool useGlobalTransform;

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    UpdatePosition();
    UpdateRotation();
    UpdateScale();
  }

  private void UpdatePosition()
  {
    if (this.useGlobalTransform)
    {
      remoteTransform.GlobalPosition = this.UpdateVector(remoteTransform.GlobalPosition, this.GlobalPosition, updatePosition);
    }
    else
    {
      remoteTransform.Position = this.UpdateVector(remoteTransform.Position, this.Position, updatePosition);
    }
  }

  private void UpdateRotation()
  {
    if (this.useGlobalTransform)
    {
      remoteTransform.GlobalRotation = this.UpdateVector(remoteTransform.GlobalRotation, this.GlobalRotation, updateRotation);
    }
    else
    {
      remoteTransform.Rotation = this.UpdateVector(remoteTransform.Rotation, this.Rotation, updateRotation);
    }
  }

  private void UpdateScale()
  {
    remoteTransform.Scale = this.UpdateVector(remoteTransform.Scale, this.Scale, updateScale);
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
    if (this.remoteTransform == null)
    {
      return new string[] { "Warning: No remote transform selected" };
    }
    return Array.Empty<string>();
  }

}
