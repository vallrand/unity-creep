using UnityEngine;

public class TerrainPainter : MonoBehaviour {
    [SerializeField] private Terrain terrain;
    [SerializeField] private RenderTexture renderTexture;
    [Header("Brush")]
    [SerializeField, Range(1f, 1000f)] private float brushSize;
    [SerializeField] private Material brushMaterial;
    [SerializeField] private bool clearRenderTexture;

    enum BrushMode {
        None,
        Paint,
        Erase
    }

    void Update(){
        BrushMode brushMode;
        if(Input.GetMouseButton(0)) brushMode = BrushMode.Paint;
        else if(Input.GetMouseButton(1)) brushMode = BrushMode.Erase;
        else return;
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hit)){
            var local = terrain.transform.worldToLocalMatrix.MultiplyPoint3x4(hit.point);
            local = new Vector3(
                local.x / terrain.terrainData.size.x,
                local.z / terrain.terrainData.size.z,
                local.y / terrain.terrainData.size.y
            );
            var size = new Vector2(
                brushSize / terrain.terrainData.size.x,
                brushSize / terrain.terrainData.size.z
            );

            brushMaterial.SetColor("_Color", brushMode switch {
                BrushMode.Erase => Color.black,
                BrushMode.Paint => Color.white,
                _ => Color.clear
            });
            RenderBrush(local, size);
        }
    }

    private void RenderBrush(Vector2 position, Vector2 size){
        var restoreRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, 1, 0, 1);

        if(clearRenderTexture) GL.Clear(true, true, Color.clear);
        Graphics.DrawTexture(
            new Rect(position.x - 0.5f * size.x, position.y - 0.5f * size.y, size.x, size.y),
            brushMaterial.mainTexture, brushMaterial
        );
        RenderTexture.active = restoreRenderTexture;
        GL.PopMatrix();
    }
}