using UnityEngine;

namespace GlymorsFeasts
{
    internal static class SerpentsHollowTexBank
    {
        private static Texture2D _breadAtlas;
        private static Texture2D _jamAtlas;
        private static Texture2D _meatChunk;
        private static Material _breadMat;
        private static Material _jamMat;
        private static Material _jamSpreadMat;
        private static Material _meatMat;
        private static Material _handleMat;

        internal static Texture2D BreadAtlas => _breadAtlas != null ? _breadAtlas : (_breadAtlas = LoadTex("bread_atlas.png", "BreadAtlas"));
        internal static Texture2D JamAtlas => _jamAtlas != null ? _jamAtlas : (_jamAtlas = LoadTex("jam_atlas.png", "JamAtlas"));
        internal static Texture2D MeatChunk => _meatChunk != null ? _meatChunk : (_meatChunk = LoadTex("meat_chunk.png", "MeatChunk"));

        internal static Material BreadMaterial => _breadMat != null ? _breadMat : (_breadMat = WithTexture("Bread", BreadAtlas, "SbbfBread"));
        internal static Material JamMaterial => _jamMat != null ? _jamMat : (_jamMat = WithTexture("QueensJam", JamAtlas, "SbbfJam"));
        // Opaque food material + jam texture. The Queen's Jam jar shader is
        // glass-like and turned the smear into an open ring in game.
        internal static Material JamSpreadMaterial => _jamSpreadMat != null ? _jamSpreadMat : (_jamSpreadMat = WithTexture("Bread", JamAtlas, "SbbfJamSpread"));
        internal static Material MeatMaterial => _meatMat != null ? _meatMat : (_meatMat = WithTexture("SerpentStew", MeatChunk, "SbbfMeat"));
        internal static Material LadleHandleMaterial => _handleMat != null ? _handleMat : (_handleMat = Tinted("FeastMountains", new Color(1.15f, 0.85f, 0.55f, 1f), "SbbfLadleHandle"));

        private static Texture2D LoadTex(string fileName, string name)
        {
            byte[] data = SerpentsHollowMeshBank.ReadResource(fileName);
            if (data == null || data.Length == 0)
            {
                Plugin.Log.LogError("Missing embedded texture " + fileName);
                return null;
            }
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.name = name;
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;
            if (!tex.LoadImage(data))
            {
                Plugin.Log.LogError("Failed to decode " + fileName);
                return null;
            }
            tex.Apply(false, false);
            Plugin.Log.LogInfo("Loaded texture " + name + " " + tex.width + "x" + tex.height);
            return tex;
        }

        private static Material WithTexture(string sourcePrefab, Texture2D tex, string name)
        {
            Material src = SerpentsHollowVisuals.MaterialFromPublic(sourcePrefab);
            if (src == null)
            {
                Plugin.Log.LogWarning("No source material " + sourcePrefab + " for " + name);
                return null;
            }
            Material mat = new Material(src);
            mat.name = name;
            if (tex != null)
            {
                if (mat.HasProperty("_MainTex"))
                {
                    mat.SetTexture("_MainTex", tex);
                }
                if (mat.HasProperty("_BaseMap"))
                {
                    mat.SetTexture("_BaseMap", tex);
                }
            }
            return mat;
        }

        private static Material Tinted(string sourcePrefab, Color tint, string name)
        {
            Material src = SerpentsHollowVisuals.MaterialFromPublic(sourcePrefab);
            if (src == null)
            {
                return null;
            }
            Material mat = new Material(src);
            mat.name = name;
            if (mat.HasProperty("_Color"))
            {
                mat.color = tint;
            }
            return mat;
        }
    }
}
