

using Godot;

public static class MeshDataToolExtensions
{

  public static Vector3[] GetFaceVertices(this MeshDataTool m, int faceIndex)
  {
    var vertices = new Vector3[3];
    for (int x = 0; x < 3; x++)
    {
      vertices[x] = m.GetVertex(m.GetFaceVertex(faceIndex, x));
    }
    return vertices;
  }

  public static Vector3[] GetVertices(this MeshDataTool m)
  {
    var vertices = new Vector3[m.GetVertexCount()];
    for (int i = 0; i < m.GetVertexCount(); i++)
    {
      vertices[i] = m.GetVertex(i);
    }
    return vertices;
  }

  public static Vector2[] GetVerticesUvs(this MeshDataTool m, int faceIndex)
  {
    var vertices = new Vector2[3];
    for (int x = 0; x < 3; x++)
    {
      vertices[x] = m.GetVertexUV(m.GetFaceVertex(faceIndex, x));
    }
    return vertices;
  }

  public static Vector3 BarycentricCoordinates(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
  {
    var v0 = b - a;
    var v1 = c - a;
    var v2 = p - a;
    var d00 = v0.Dot(v0);
    var d01 = v0.Dot(v1);
    var d11 = v1.Dot(v1);
    var d20 = v2.Dot(v0);
    var d21 = v2.Dot(v1);

    var denom = d00 * d11 - d01 * d01;
    var v = (d11 * d20 - d01 * d21) / denom;
    var w = (d00 * d21 - d01 * d20) / denom;
    var u = 1.0 - v - w;
    return new Vector3((float)u, (float)v, (float)w);
  }


  public static bool IsPointInTriangle(this MeshDataTool m, Vector3 point, int faceId)
  {
    var vertices = m.GetFaceVertices(faceId);
    var bc = BarycentricCoordinates(point, vertices[0], vertices[1], vertices[2]);
    if (bc.X < 0 || bc.X > 1)
    {
      return false;
    }

    if (bc.Y < 0 || bc.Y > 1)
    {
      return false;
    }

    if (bc.Z < 0 || bc.Z > 1)
    {
      return false;
    }
    return true;
  }



}