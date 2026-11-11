using System.IO;
using System.Reflection;
using UnityEngine;

namespace GlymorsFeasts
{
    internal static class MeshBank
    {
        private static Mesh _pulledMeat;
        private static Mesh _turnipHalf;
        private static Mesh _bfHerbs;
        private static Mesh _carrotChunkA;
        private static Mesh _carrotChunkB;
        private static Mesh _strandClumpA;
        private static Mesh _strandClumpB;
        private static Mesh _strandClumpC;
        private static Mesh _carrotRoundA;
        private static Mesh _carrotRoundB;
        private static Mesh _strandMatA;
        private static Mesh _strandMatB;
        private static Mesh _strandMatC;
        private static Mesh _fibreShredA;
        private static Mesh _fibreShredB;

        internal static Mesh PulledMeat => _pulledMeat != null ? _pulledMeat : (_pulledMeat = Load("pulled_meat.bin", "PulledBear_meat"));
        internal static Mesh TurnipHalf => _turnipHalf != null ? _turnipHalf : (_turnipHalf = Load("turnip_half.bin", "Turnip_half"));
        internal static Mesh BfHerbs => _bfHerbs != null ? _bfHerbs : (_bfHerbs = Load("bf_herbs.bin", "BfHerbs"));
        internal static Mesh CarrotChunkA => _carrotChunkA != null ? _carrotChunkA : (_carrotChunkA = Load("carrot_chunk_a.bin", "CarrotChunkA"));
        internal static Mesh CarrotChunkB => _carrotChunkB != null ? _carrotChunkB : (_carrotChunkB = Load("carrot_chunk_b.bin", "CarrotChunkB"));
        internal static Mesh StrandClumpA => _strandClumpA != null ? _strandClumpA : (_strandClumpA = Load("strand_clump_a.bin", "StrandClumpA"));
        internal static Mesh StrandClumpB => _strandClumpB != null ? _strandClumpB : (_strandClumpB = Load("strand_clump_b.bin", "StrandClumpB"));
        internal static Mesh StrandClumpC => _strandClumpC != null ? _strandClumpC : (_strandClumpC = Load("strand_clump_c.bin", "StrandClumpC"));
        internal static Mesh CarrotRoundA => _carrotRoundA != null ? _carrotRoundA : (_carrotRoundA = Load("carrot_round_a.bin", "CarrotRoundA"));
        internal static Mesh CarrotRoundB => _carrotRoundB != null ? _carrotRoundB : (_carrotRoundB = Load("carrot_round_b.bin", "CarrotRoundB"));
        internal static Mesh StrandMatA => _strandMatA != null ? _strandMatA : (_strandMatA = Load("strand_mat_a.bin", "StrandMatA"));
        internal static Mesh StrandMatB => _strandMatB != null ? _strandMatB : (_strandMatB = Load("strand_mat_b.bin", "StrandMatB"));
        internal static Mesh StrandMatC => _strandMatC != null ? _strandMatC : (_strandMatC = Load("strand_mat_c.bin", "StrandMatC"));
        internal static Mesh FibreShredA => _fibreShredA != null ? _fibreShredA : (_fibreShredA = Load("fibre_shred_a.bin", "FibreShredA"));
        internal static Mesh FibreShredB => _fibreShredB != null ? _fibreShredB : (_fibreShredB = Load("fibre_shred_b.bin", "FibreShredB"));

        private static Mesh Load(string fileName, string meshName)
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

        private static byte[] ReadResource(string fileName)
        {
            Assembly asm = typeof(MeshBank).Assembly;
            string[] names = asm.GetManifestResourceNames();
            string match = null;
            foreach (string n in names)
            {
                if (n.EndsWith(fileName, System.StringComparison.OrdinalIgnoreCase) || n.EndsWith("meshes." + fileName, System.StringComparison.OrdinalIgnoreCase))
                {
                    match = n;
                    break;
                }
            }
            if (match == null)
            {
                Plugin.Log.LogError("Embedded mesh not found: " + fileName + ". Have: " + string.Join(", ", names));
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
