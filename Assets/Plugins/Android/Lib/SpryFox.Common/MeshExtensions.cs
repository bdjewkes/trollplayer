using UnityEngine;

public static class MeshExtensions {
    public static void SetVertexColor(this Mesh mesh, Color color) {
        Color[] colors = new Color[mesh.vertexCount];
        for (int i = 0; i < mesh.vertexCount; i++) {
            colors[i] = color;
        }
        mesh.colors = colors;
    }
}