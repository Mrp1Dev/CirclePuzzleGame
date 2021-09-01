#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ArtPPUSetter : AssetPostprocessor
{
    [SerializeField] [Tooltip("All lowercase.")]
    private string packsFolderName = "packs";

    private void OnPostprocessTexture(Texture2D texture)
    {
        if (assetPath.ToLower().IndexOf($"/{packsFolderName}/") == -1) return;
        var textureImporter = (TextureImporter) assetImporter;
        textureImporter.spritePixelsPerUnit = texture.width;
    }
}
#endif