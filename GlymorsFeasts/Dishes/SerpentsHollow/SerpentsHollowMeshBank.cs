using System.IO;
using System.Reflection;
using UnityEngine;

namespace GlymorsFeasts
{
    internal static class SerpentsHollowMeshBank
    {
        private static Mesh _potBody;
        private static Mesh _potFloor;
        private static Mesh _ladleScoop;
        private static Mesh _ladleHandle;
        private static Mesh _stewFull;
        private static Mesh _stewHalf;
        private static Mesh _stewLow;
        private static Mesh _stewChunksFull;
        private static Mesh _stewChunksHalf;
        private static Mesh _stewChunksLow;
        private static Mesh _stewBowlFull;
        private static Mesh _stewBowlHalf;
        private static Mesh _stewBowlChunksFull;
        private static Mesh _stewBowlChunksHalf;
        private static Mesh _breadRoll;
        private static Mesh _breadBowl;
        private static Mesh _breadLid;
        private static Mesh _breadHalf;
        private static Mesh _jamJar;
        private static Mesh _jamSmear;
        private static Mesh _jamDish;
        private static Mesh _plainsBoardClean;

        internal static Mesh PotBody => Load(ref _potBody, "pot_body.bin", "PotBody");
        internal static Mesh PotFloor => Load(ref _potFloor, "pot_floor.bin", "PotFloor");
        internal static Mesh LadleScoop => Load(ref _ladleScoop, "ladle_scoop.bin", "LadleScoop");
        internal static Mesh LadleHandle => Load(ref _ladleHandle, "ladle_handle.bin", "LadleHandle");
        internal static Mesh StewFull => Load(ref _stewFull, "stew_full.bin", "StewFull");
        internal static Mesh StewHalf => Load(ref _stewHalf, "stew_half.bin", "StewHalf");
        internal static Mesh StewLow => Load(ref _stewLow, "stew_low.bin", "StewLow");
        internal static Mesh StewChunksFull => Load(ref _stewChunksFull, "stew_chunks_full.bin", "StewChunksFull");
        internal static Mesh StewChunksHalf => Load(ref _stewChunksHalf, "stew_chunks_half.bin", "StewChunksHalf");
        internal static Mesh StewChunksLow => Load(ref _stewChunksLow, "stew_chunks_low.bin", "StewChunksLow");
        internal static Mesh StewBowlFull => Load(ref _stewBowlFull, "stew_bowl_full.bin", "StewBowlFull");
        internal static Mesh StewBowlHalf => Load(ref _stewBowlHalf, "stew_bowl_half.bin", "StewBowlHalf");
        internal static Mesh StewBowlChunksFull => Load(ref _stewBowlChunksFull, "stew_bowl_chunks_full.bin", "StewBowlChunksFull");
        internal static Mesh StewBowlChunksHalf => Load(ref _stewBowlChunksHalf, "stew_bowl_chunks_half.bin", "StewBowlChunksHalf");
        internal static Mesh BreadRoll => Load(ref _breadRoll, "bread_roll.bin", "BreadRoll");
        internal static Mesh BreadBowl => Load(ref _breadBowl, "bread_bowl.bin", "BreadBowl");
        internal static Mesh BreadLid => Load(ref _breadLid, "bread_lid.bin", "BreadLid");
        internal static Mesh BreadHalf => Load(ref _breadHalf, "bread_half.bin", "BreadHalf");
        internal static Mesh JamJar => Load(ref _jamJar, "jam_jar.bin", "JamJar");
        internal static Mesh JamSmear => Load(ref _jamSmear, "jam_smear.bin", "JamSmear");
        internal static Mesh JamDish => Load(ref _jamDish, "jam_dish.bin", "JamDish");
        internal static Mesh PlainsBoardClean => Load(ref _plainsBoardClean, "plains_board_clean.bin", "PlainsBoardClean");

        private static Mesh Load(ref Mesh cached, string fileName, string meshName)
        {
            if (cached != null)
            {
                return cached;
            }
            cached = LoadMesh(fileName, meshName);
            return cached;
        }

        private static Mesh LoadMesh(string fileName, string meshName)
        {
            byte[] data = ReadResource(fileName);
            if (data == null || data.Length < 12)
            {
                Plugin.Log.LogError("Missing baked mesh " + fileName);
                return null;
            }
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader br = new BinaryReader(ms))
            {
                char[] magic = br.ReadChars(4);
                if (new string(magic) != "PBFM")
                {
                    Plugin.Log.LogError("Bad mesh magic in " + fileName);
                    return null;
                }
                int version = br.ReadInt32();
                if (version != 1)
                {
                    Plugin.Log.LogError("Unsupported mesh version " + version + " in " + fileName);
                    return null;
                }
                int vCount = br.ReadInt32();
                int iCount = br.ReadInt32();
                Vector3[] verts = new Vector3[vCount];
                for (int i = 0; i < vCount; i++)
                {
                    verts[i] = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                }
                Vector2[] uvs = new Vector2[vCount];
                for (int i = 0; i < vCount; i++)
                {
                    uvs[i] = new Vector2(br.ReadSingle(), br.ReadSingle());
                }
                int[] tris = new int[iCount];
                for (int i = 0; i < iCount; i++)
                {
                    tris[i] = br.ReadInt32();
                }
                Mesh mesh = new Mesh();
                mesh.name = meshName;
                mesh.vertices = verts;
                mesh.uv = uvs;
                mesh.triangles = tris;
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
                Plugin.Log.LogInfo("Loaded baked mesh " + meshName + " (" + vCount + " verts, " + (iCount / 3) + " tris)");
                return mesh;
            }
        }

        internal static byte[] ReadResource(string fileName)
        {
            Assembly asm = typeof(SerpentsHollowMeshBank).Assembly;
            string[] names = asm.GetManifestResourceNames();
            string match = null;
            foreach (string n in names)
            {
                if (n.EndsWith(fileName, System.StringComparison.OrdinalIgnoreCase)
                    || n.EndsWith("meshes." + fileName, System.StringComparison.OrdinalIgnoreCase)
                    || n.EndsWith("meshes.tex." + fileName, System.StringComparison.OrdinalIgnoreCase))
                {
                    match = n;
                    break;
                }
            }
            if (match == null)
            {
                Plugin.Log.LogError("Embedded resource not found: " + fileName + ". Have: " + string.Join(", ", names));
                return null;
            }
            using (Stream s = asm.GetManifestResourceStream(match))
            {
                if (s == null)
                {
                    return null;
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    s.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}
