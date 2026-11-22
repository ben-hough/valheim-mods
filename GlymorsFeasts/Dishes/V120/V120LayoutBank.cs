using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GlymorsFeasts
{
    internal static class V120LayoutBank
    {
        private static bool _loaded;

        private static Mesh[] _meshes;

        private static Texture2D[] _textures;

        private static Material[] _materials;

        private static readonly Dictionary<string, V120DishLayout> Dishes = new Dictionary<string, V120DishLayout>();

        internal static V120DishLayout Get(string id)
        {
            EnsureLoaded();
            if (!Dishes.TryGetValue(id, out var value))
            {
                return null;
            }
            return value;
        }

        internal static Mesh MeshAt(int index)
        {
            EnsureLoaded();
            if (_meshes == null || index < 0 || index >= _meshes.Length)
            {
                return null;
            }
            return _meshes[index];
        }

        internal static Texture2D TextureAt(int index)
        {
            EnsureLoaded();
            if (_textures == null || index < 0 || index >= _textures.Length)
            {
                return null;
            }
            return _textures[index];
        }

        internal static Material MaterialAt(int texIndex, float gloss)
        {
            EnsureLoaded();
            if (_materials == null || texIndex < 0 || texIndex >= _materials.Length)
            {
                return null;
            }
            Material val = _materials[texIndex];
            if (val == null)
            {
                return null;
            }
            Material val2 = new Material(val);
            ((Object)val2).name = ((Object)val).name + "_g";
            if (val2.HasProperty("_Glossiness"))
            {
                val2.SetFloat("_Glossiness", Mathf.Clamp01(gloss));
            }
            if (val2.HasProperty("_Smoothness"))
            {
                val2.SetFloat("_Smoothness", Mathf.Clamp01(gloss));
            }
            if (val2.HasProperty("_Metallic"))
            {
                val2.SetFloat("_Metallic", Mathf.Clamp01(gloss * 0.25f));
            }
            return val2;
        }

        internal static void EnsureLoaded()
        {
            if (_loaded)
            {
                return;
            }
            _loaded = true;
            byte[] array = ReadResource("layouts.bin");
            if (array == null || array.Length < 8)
            {
                Plugin.Log.LogError("V120 layouts.bin missing.");
                return;
            }
            using (MemoryStream input = new MemoryStream(array))
            {
                using BinaryReader binaryReader = new BinaryReader(input);
                if (new string(binaryReader.ReadChars(4)) != "GF12")
                {
                    Plugin.Log.LogError("Bad V120 layout magic.");
                    return;
                }
                int num = binaryReader.ReadInt32();
                if (num != 1)
                {
                    Plugin.Log.LogError(("Unsupported V120 layout version " + num));
                    return;
                }
                int num2 = binaryReader.ReadInt32();
                _meshes = (Mesh[])(object)new Mesh[num2];
                for (int i = 0; i < num2; i++)
                {
                    string text = ReadW(binaryReader);
                    _meshes[i] = LoadMesh(text, Path.GetFileNameWithoutExtension(text));
                }
                int num3 = binaryReader.ReadInt32();
                _textures = (Texture2D[])(object)new Texture2D[num3];
                _materials = (Material[])(object)new Material[num3];
                Material val = SerpentsHollowVisuals.MaterialFromPublic("Bread");
                if (val == null)
                {
                    val = SerpentsHollowVisuals.MaterialFromPublic("FeastMountains");
                }
                for (int j = 0; j < num3; j++)
                {
                    string text2 = ReadW(binaryReader);
                    Texture2D val2 = LoadTex(text2, "V120_" + Path.GetFileNameWithoutExtension(text2));
                    _textures[j] = val2;
                    if (!(val != null))
                    {
                        continue;
                    }
                    Material val3 = new Material(val);
                    ((Object)val3).name = "V120Mat_" + j;
                    if (val2 != null)
                    {
                        if (val3.HasProperty("_MainTex"))
                        {
                            val3.SetTexture("_MainTex", (Texture)(object)val2);
                        }
                        if (val3.HasProperty("_BaseMap"))
                        {
                            val3.SetTexture("_BaseMap", (Texture)(object)val2);
                        }
                    }
                    _materials[j] = val3;
                }
                int num4 = binaryReader.ReadInt32();
                for (int k = 0; k < num4; k++)
                {
                    V120DishLayout v120DishLayout = new V120DishLayout();
                    v120DishLayout.Id = ReadW(binaryReader);
                    v120DishLayout.ColliderCenter = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                    v120DishLayout.ColliderSize = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                    v120DishLayout.Stage10 = ReadStage(binaryReader);
                    v120DishLayout.Stage6 = ReadStage(binaryReader);
                    v120DishLayout.Stage3 = ReadStage(binaryReader);
                    v120DishLayout.Stage0 = ReadStage(binaryReader);
                    Dishes[v120DishLayout.Id] = v120DishLayout;
                }
            }
            Plugin.Log.LogInfo(("V120 layouts loaded: " + Dishes.Count + " dishes, " + ((_meshes != null) ? _meshes.Length : 0) + " meshes, " + ((_textures != null) ? _textures.Length : 0) + " textures."));
        }

        private static V120Inst[] ReadStage(BinaryReader br)
        {
            int num = br.ReadInt32();
            V120Inst[] array = new V120Inst[num];
            for (int i = 0; i < num; i++)
            {
                V120Inst v120Inst = new V120Inst();
                v120Inst.Mesh = br.ReadInt16();
                v120Inst.Tex = br.ReadInt16();
                v120Inst.Matrix = new float[16];
                for (int j = 0; j < 16; j++)
                {
                    v120Inst.Matrix[j] = br.ReadSingle();
                }
                v120Inst.Gloss = br.ReadSingle();
                v120Inst.Vessel = br.ReadByte() != 0;
                v120Inst.Name = ReadW(br);
                array[i] = v120Inst;
            }
            return array;
        }

        private static string ReadW(BinaryReader br)
        {
            int count = br.ReadUInt16();
            byte[] bytes = br.ReadBytes(count);
            return Encoding.UTF8.GetString(bytes);
        }

        private static Mesh LoadMesh(string fileName, string meshName)
        {
            byte[] array = ReadResource(fileName);
            if (array == null || array.Length < 12)
            {
                Plugin.Log.LogError(("Missing baked mesh " + fileName));
                return null;
            }
            using MemoryStream input = new MemoryStream(array);
            using BinaryReader binaryReader = new BinaryReader(input);
            if (new string(binaryReader.ReadChars(4)) != "PBFM")
            {
                Plugin.Log.LogError(("Bad mesh magic in " + fileName));
                return null;
            }
            if (binaryReader.ReadInt32() != 1)
            {
                Plugin.Log.LogError(("Unsupported mesh version in " + fileName));
                return null;
            }
            int num = binaryReader.ReadInt32();
            int num2 = binaryReader.ReadInt32();
            Vector3[] array2 = (Vector3[])(object)new Vector3[num];
            for (int i = 0; i < num; i++)
            {
                array2[i] = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
            }
            Vector2[] array3 = (Vector2[])(object)new Vector2[num];
            for (int j = 0; j < num; j++)
            {
                array3[j] = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
            }
            int[] array4 = new int[num2];
            for (int k = 0; k < num2; k++)
            {
                array4[k] = binaryReader.ReadInt32();
            }
            Mesh val = new Mesh
            {
                name = meshName,
                vertices = array2,
                uv = array3,
                triangles = array4
            };
            val.RecalculateNormals();
            val.RecalculateBounds();
            return val;
        }

        private static Texture2D LoadTex(string fileName, string name)
        {
            byte[] array = ReadResource(fileName);
            if (array == null || array.Length == 0)
            {
                Plugin.Log.LogError(("Missing texture " + fileName));
                return null;
            }
            Texture2D val = new Texture2D(2, 2, (TextureFormat)4, false);
            if (!ImageConversion.LoadImage(val, array, false))
            {
                Plugin.Log.LogError(("Failed to decode " + fileName));
                Object.Destroy(val);
                return null;
            }
            Texture2D result = WithMips((Texture)(object)val, name);
            Object.Destroy(val);
            return result;
        }

        internal static Texture2D WithMips(Texture src, string name)
        {
            if (src == null)
            {
                return null;
            }
            int width = src.width;
            int height = src.height;
            Color[] array = null;
            Texture2D val = (Texture2D)(object)((src is Texture2D) ? src : null);
            if (val != null && ((Texture)val).isReadable)
            {
                try
                {
                    array = val.GetPixels();
                }
                catch
                {
                    array = null;
                }
            }
            if (array == null)
            {
                RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, (RenderTextureFormat)0);
                Graphics.Blit(src, temporary);
                RenderTexture active = RenderTexture.active;
                RenderTexture.active = temporary;
                Texture2D val2 = new Texture2D(width, height, (TextureFormat)4, false);
                val2.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
                val2.Apply(false, false);
                RenderTexture.active = active;
                RenderTexture.ReleaseTemporary(temporary);
                array = val2.GetPixels();
                Object.Destroy((Object)val2);
            }
            Texture2D val3 = new Texture2D(width, height, (TextureFormat)4, true)
            {
                name = (string.IsNullOrEmpty(name) ? "V120_mips" : name),
                wrapMode = (TextureWrapMode)1,
                filterMode = (FilterMode)2,
                anisoLevel = 8
            };
            val3.SetPixels(array);
            val3.Apply(true, false);
            return val3;
        }

        internal static byte[] ReadResource(string fileName)
        {
            Assembly assembly = typeof(V120LayoutBank).Assembly;
            string[] manifestResourceNames = assembly.GetManifestResourceNames();
            string text = null;
            string[] array = manifestResourceNames;
            foreach (string text2 in array)
            {
                if ((text2.EndsWith("." + fileName, StringComparison.OrdinalIgnoreCase) || text2.Equals(fileName, StringComparison.OrdinalIgnoreCase) || text2.EndsWith(fileName, StringComparison.OrdinalIgnoreCase)) && (text2.IndexOf("v120", StringComparison.OrdinalIgnoreCase) >= 0 || text == null))
                {
                    text = text2;
                    if (text2.IndexOf("v120", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        break;
                    }
                }
            }
            if (text == null)
            {
                Plugin.Log.LogError(("Embedded resource not found: " + fileName));
                return null;
            }
            using Stream stream = assembly.GetManifestResourceStream(text);
            if (stream == null)
            {
                return null;
            }
            using MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        internal static Sprite LoadIcon(string fileName, string spriteName)
        {
            try
            {
                byte[] array = ReadResource(fileName);
                if (array == null || array.Length == 0)
                {
                    return null;
                }
                Texture2D val = new Texture2D(2, 2, (TextureFormat)4, false);
                if (!ImageConversion.LoadImage(val, array))
                {
                    return null;
                }
                ((Object)val).name = spriteName;
                return Sprite.Create(val, new Rect(0f, 0f, (float)((Texture)val).width, (float)((Texture)val).height), new Vector2(0.5f, 0.5f), 100f);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning(("Could not load icon " + fileName + ": " + ex.Message));
                return null;
            }
        }
    }
}
