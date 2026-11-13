using System;
using System.Collections.Generic;
using UnityEngine;

namespace GlymorsFeasts
{
    // Drop every bread piece onto the real plains-board mesh top at its XY,
    // or onto the pieces it is supposed to rest on. Replaces the old AABB
    // SnapToDeck + constant Lift path, which left tilted and stacked rolls
    // hovering. Keep in sync with tools/build_visuals_v111.py.
    internal sealed class SerpentsHollowSettle
    {
        internal const float ContactEps = 0.0015f;
        internal const float Sink = 0.0004f;
        private const float CenterOnPiece = 0.02f;

        internal static readonly string[] WholeOrder = { "roll_A", "roll_C", "roll_D", "roll_E", "roll_F", "roll_B" };
        internal static readonly string[] SliceOrder = { "slice_1", "slice_3", "slice_4", "slice_2", "slice_6", "slice_5" };

        private static readonly HashSet<string> StackTags = new HashSet<string>
        {
            "lid", "roll_F", "roll_B", "roll_E", "slice_5", "slice_6", "slice_2"
        };

        private static readonly Dictionary<string, string[]> RestsOn = new Dictionary<string, string[]>
        {
            { "lid", new[] { "empty_bowl" } },
            { "roll_F", new[] { "roll_A" } },
            { "roll_B", new[] { "roll_A", "roll_F" } },
            { "roll_E", new[] { "roll_D" } },
            { "slice_2", new[] { "slice_4" } },
            { "slice_6", new[] { "slice_1", "slice_3" } },
            { "slice_5", new[] { "slice_1", "slice_2", "slice_4" } },
        };

        internal static string WholeTag(char key)
        {
            return "roll_" + key;
        }

        internal static string SliceTag(char key)
        {
            return "slice_" + key;
        }

        internal static bool IsStackedSlice2(ICollection<string> presentTags)
        {
            return presentTags != null && presentTags.Contains("slice_4");
        }

        private readonly Support _board;
        private readonly Support _all;
        private readonly Dictionary<string, Placed> _placed = new Dictionary<string, Placed>();

        internal SerpentsHollowSettle()
        {
            EnsureBoard();
            Mesh board = SerpentsHollowMeshBank.PlainsBoardClean;
            _board = new Support();
            _all = new Support();
            if (board != null)
            {
                Matrix4x4 ident = Matrix4x4.identity;
                _board.AddMesh(board, ident);
                _all.AddMesh(board, ident);
            }
        }

        internal GameObject Place(Mesh mesh, Material mat, Transform parent, SerpentsHollowLayout.Pose pose, string name, string tag, bool allowTilt)
        {
            if (mesh == null)
            {
                return null;
            }
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = new Vector3(pose.Pos.x, 0f, pose.Pos.z);
            go.transform.localRotation = Quaternion.Euler(pose.Euler);
            go.transform.localScale = Vector3.one * pose.Scale;
            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = mat;

            Support use;
            bool tilt;
            bool preferBoard;
            ChooseSupports(tag, pose.Pos.x, pose.Pos.z, allowTilt, out use, out tilt, out preferBoard);
            Apply(go, mesh, pose, use, tilt, preferBoard);

            Matrix4x4 local = Matrix4x4.TRS(go.transform.localPosition, go.transform.localRotation, go.transform.localScale);
            _all.AddMesh(mesh, local);
            _placed[tag] = new Placed { Mesh = mesh, Local = local, Tag = tag };
            return go;
        }

        private void ChooseSupports(string tag, float x, float z, bool allowTilt, out Support use, out bool tilt, out bool preferBoard)
        {
            bool stacked = StackTags.Contains(tag);
            if (tag == "slice_2" && !_placed.ContainsKey("slice_4"))
            {
                stacked = false;
            }
            if (!stacked)
            {
                use = _board;
                tilt = false;
                preferBoard = false;
                return;
            }

            Support rest = new Support();
            rest.AddMesh(SerpentsHollowMeshBank.PlainsBoardClean, Matrix4x4.identity);
            string[] allowed;
            if (RestsOn.TryGetValue(tag, out allowed))
            {
                for (int i = 0; i < allowed.Length; i++)
                {
                    Placed p;
                    if (_placed.TryGetValue(allowed[i], out p))
                    {
                        rest.AddMesh(p.Mesh, p.Local);
                    }
                }
            }
            float bh, sh;
            bool hasBoard = _board.TryHeight(x, z, out bh);
            bool hasRest = rest.TryHeight(x, z, out sh);
            bool centerOnPiece = hasBoard && hasRest && sh > bh + CenterOnPiece;
            use = rest;
            tilt = allowTilt;
            preferBoard = !centerOnPiece;
        }

        private static void Apply(GameObject go, Mesh mesh, SerpentsHollowLayout.Pose pose, Support supports, bool tilt, bool preferBoard)
        {
            Vector3[] localVerts = mesh.vertices;
            if (localVerts == null || localVerts.Length == 0)
            {
                return;
            }
            float x = pose.Pos.x;
            float z = pose.Pos.z;
            float scale = pose.Scale;
            Vector3 euler = pose.Euler;
            Support start = (preferBoard && supports != null) ? null : supports;
            // preferBoard starts from a board-only drop, then searches against `supports`.
            Support board = preferBoard ? BoardOf(supports) : null;
            if (preferBoard)
            {
                board = SharedBoard;
            }

            Vector3 bestE = euler;
            float bestScore, bestDy;
            int bestN;
            if (preferBoard)
            {
                Score(localVerts, bestE, scale, x, z, SharedBoard, out bestScore, out bestDy, out bestN);
                ScorePenalized(localVerts, bestE, scale, x, z, supports, SharedBoard, out bestScore, out bestDy, out bestN);
            }
            else
            {
                Score(localVerts, bestE, scale, x, z, supports, out bestScore, out bestDy, out bestN);
            }

            if (tilt)
            {
                float tiltLim = preferBoard ? 30f : 18f;
                if (preferBoard)
                {
                    float[] seed = { -24f, -16f, -8f, 0f, 8f, 16f, 24f };
                    for (int i = 0; i < seed.Length; i++)
                    {
                        for (int j = 0; j < seed.Length; j++)
                        {
                            Vector3 e = new Vector3(seed[i], euler.y, seed[j]);
                            float sc, dy;
                            int n;
                            ScorePenalized(localVerts, e, scale, x, z, supports, SharedBoard, out sc, out dy, out n);
                            if (sc < bestScore - 1e-6f)
                            {
                                bestScore = sc;
                                bestDy = dy;
                                bestN = n;
                                bestE = e;
                            }
                        }
                    }
                }
                else
                {
                    Vector3 flat = new Vector3(0f, euler.y, 0f);
                    float sc, dy;
                    int n;
                    Score(localVerts, flat, scale, x, z, supports, out sc, out dy, out n);
                    if (sc < bestScore - 1e-6f)
                    {
                        bestScore = sc;
                        bestDy = dy;
                        bestN = n;
                        bestE = flat;
                    }
                }

                float[] steps = preferBoard
                    ? new[] { 8f, 4f, 2f, 1f, 0.5f }
                    : new[] { 4f, 2f, 1f, 0.5f };
                for (int s = 0; s < steps.Length; s++)
                {
                    float step = steps[s];
                    Vector3 cur = bestE;
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dz = -1; dz <= 1; dz++)
                        {
                            if (dx == 0 && dz == 0)
                            {
                                continue;
                            }
                            float nx = cur.x + dx * step;
                            float nz = cur.z + dz * step;
                            if (Mathf.Abs(nx) > tiltLim || Mathf.Abs(nz) > tiltLim)
                            {
                                continue;
                            }
                            Vector3 e = new Vector3(nx, euler.y, nz);
                            float sc, dy;
                            int n;
                            if (preferBoard)
                            {
                                ScorePenalized(localVerts, e, scale, x, z, supports, SharedBoard, out sc, out dy, out n);
                            }
                            else
                            {
                                Score(localVerts, e, scale, x, z, supports, out sc, out dy, out n);
                            }
                            if (sc < bestScore - 1e-6f)
                            {
                                bestScore = sc;
                                bestDy = dy;
                                bestN = n;
                                bestE = e;
                            }
                        }
                    }
                }
            }

            go.transform.localRotation = Quaternion.Euler(bestE);
            go.transform.localPosition = new Vector3(x, bestDy, z);
            go.transform.localScale = Vector3.one * scale;
        }

        // Set once the first session is constructed (the board mesh is static).
        private static Support SharedBoard;

        internal static void EnsureBoard()
        {
            if (SharedBoard != null)
            {
                return;
            }
            SharedBoard = new Support();
            Mesh board = SerpentsHollowMeshBank.PlainsBoardClean;
            if (board != null)
            {
                SharedBoard.AddMesh(board, Matrix4x4.identity);
            }
        }

        internal SerpentsHollowSettle Init()
        {
            EnsureBoard();
            return this;
        }

        private static Support BoardOf(Support unused)
        {
            EnsureBoard();
            return SharedBoard;
        }

        private static void ScorePenalized(Vector3[] local, Vector3 euler, float scale, float x, float z, Support supports, Support board, out float score, out float dy, out int n)
        {
            Score(local, euler, scale, x, z, supports, out score, out dy, out n);
            float dummy, dyB;
            int dummyN;
            Score(local, euler, scale, x, z, board, out dummy, out dyB, out dummyN);
            score += 2.5f * Mathf.Max(0f, dy - dyB);
        }

        private static float DropY(Vector3[] local, Vector3 euler, float scale, float x, float z, Support supports)
        {
            Matrix4x4 m = Matrix4x4.TRS(new Vector3(x, 0f, z), Quaternion.Euler(euler), Vector3.one * scale);
            float dy = float.NegativeInfinity;
            bool any = false;
            float minY = float.PositiveInfinity;
            for (int i = 0; i < local.Length; i++)
            {
                Vector3 p = m.MultiplyPoint3x4(local[i]);
                if (p.y < minY)
                {
                    minY = p.y;
                }
                float s;
                if (supports != null && supports.TryHeight(p.x, p.z, out s))
                {
                    float need = s - p.y;
                    if (!any || need > dy)
                    {
                        dy = need;
                        any = true;
                    }
                }
            }
            if (!any)
            {
                dy = SerpentsHollowLayout.DeckY - minY;
            }
            return dy - Sink;
        }

        private static void Score(Vector3[] local, Vector3 euler, float scale, float x, float z, Support supports, out float score, out float dy, out int nContact)
        {
            dy = DropY(local, euler, scale, x, z, supports);
            Matrix4x4 m = Matrix4x4.TRS(new Vector3(x, dy, z), Quaternion.Euler(euler), Vector3.one * scale);
            List<float> gaps = new List<float>(local.Length);
            nContact = 0;
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < local.Length; i++)
            {
                Vector3 p = m.MultiplyPoint3x4(local[i]);
                sum += p;
                float s;
                if (supports != null && supports.TryHeight(p.x, p.z, out s))
                {
                    float g = p.y - s;
                    gaps.Add(g);
                    if (Mathf.Abs(g + Sink) <= ContactEps || Mathf.Abs(g) <= ContactEps)
                    {
                        nContact++;
                    }
                }
            }
            if (gaps.Count == 0)
            {
                score = 99f;
                return;
            }
            gaps.Sort();
            int k = Math.Max(8, gaps.Count / 3);
            if (k > gaps.Count)
            {
                k = gaps.Count;
            }
            float meanGap = 0f;
            for (int i = 0; i < k; i++)
            {
                meanGap += Mathf.Max(0f, gaps[i]);
            }
            meanGap /= k;
            float comY = sum.y / local.Length;
            float contactPen = nContact < 3 ? 0.06f : (nContact < 5 ? 0.02f : 0f);
            score = comY + 6f * meanGap + contactPen - 0.003f * Mathf.Min(nContact, 40);
        }

        private struct Placed
        {
            internal Mesh Mesh;
            internal Matrix4x4 Local;
            internal string Tag;
        }

        internal sealed class Support
        {
            private const float Cell = 0.025f;
            private const float X0 = -1.50f;
            private const float Z0 = -1.05f;
            private const int Nx = 102; // (1.05 - -1.50)/0.025 ≈ 102
            private const int Nz = 84;  // (1.05 - -1.05)/0.025 ≈ 84

            private readonly List<Vector3> _a = new List<Vector3>();
            private readonly List<Vector3> _b = new List<Vector3>();
            private readonly List<Vector3> _c = new List<Vector3>();
            private readonly List<int>[] _cells = new List<int>[Nx * Nz];

            internal void AddMesh(Mesh mesh, Matrix4x4 local)
            {
                if (mesh == null)
                {
                    return;
                }
                Vector3[] v = mesh.vertices;
                int[] t = mesh.triangles;
                if (v == null || t == null)
                {
                    return;
                }
                for (int i = 0; i + 2 < t.Length; i += 3)
                {
                    Vector3 pa = local.MultiplyPoint3x4(v[t[i]]);
                    Vector3 pb = local.MultiplyPoint3x4(v[t[i + 1]]);
                    Vector3 pc = local.MultiplyPoint3x4(v[t[i + 2]]);
                    float area = (pb.x - pa.x) * (pc.z - pa.z) - (pc.x - pa.x) * (pb.z - pa.z);
                    if (Mathf.Abs(area) < 1e-10f)
                    {
                        continue;
                    }
                    int ti = _a.Count;
                    _a.Add(pa);
                    _b.Add(pb);
                    _c.Add(pc);
                    float xmin = Mathf.Min(pa.x, Mathf.Min(pb.x, pc.x)) - 1e-6f;
                    float xmax = Mathf.Max(pa.x, Mathf.Max(pb.x, pc.x)) + 1e-6f;
                    float zmin = Mathf.Min(pa.z, Mathf.Min(pb.z, pc.z)) - 1e-6f;
                    float zmax = Mathf.Max(pa.z, Mathf.Max(pb.z, pc.z)) + 1e-6f;
                    int ix0 = Mathf.Clamp(Mathf.FloorToInt((xmin - X0) / Cell), 0, Nx - 1);
                    int ix1 = Mathf.Clamp(Mathf.FloorToInt((xmax - X0) / Cell), 0, Nx - 1);
                    int iz0 = Mathf.Clamp(Mathf.FloorToInt((zmin - Z0) / Cell), 0, Nz - 1);
                    int iz1 = Mathf.Clamp(Mathf.FloorToInt((zmax - Z0) / Cell), 0, Nz - 1);
                    for (int iz = iz0; iz <= iz1; iz++)
                    {
                        int row = iz * Nx;
                        for (int ix = ix0; ix <= ix1; ix++)
                        {
                            int ci = row + ix;
                            if (_cells[ci] == null)
                            {
                                _cells[ci] = new List<int>(4);
                            }
                            _cells[ci].Add(ti);
                        }
                    }
                }
            }

            internal bool TryHeight(float x, float z, out float y)
            {
                int ix = Mathf.FloorToInt((x - X0) / Cell);
                int iz = Mathf.FloorToInt((z - Z0) / Cell);
                y = 0f;
                if (ix < 0 || iz < 0 || ix >= Nx || iz >= Nz)
                {
                    return false;
                }
                List<int> cell = _cells[iz * Nx + ix];
                if (cell == null || cell.Count == 0)
                {
                    return false;
                }
                bool any = false;
                float best = 0f;
                for (int i = 0; i < cell.Count; i++)
                {
                    int ti = cell[i];
                    float ty;
                    if (TriY(_a[ti], _b[ti], _c[ti], x, z, out ty))
                    {
                        if (!any || ty > best)
                        {
                            best = ty;
                            any = true;
                        }
                    }
                }
                if (!any)
                {
                    return false;
                }
                y = best;
                return true;
            }

            private static bool TriY(Vector3 pa, Vector3 pb, Vector3 pc, float x, float z, out float y)
            {
                float v0x = pc.x - pa.x, v0z = pc.z - pa.z;
                float v1x = pb.x - pa.x, v1z = pb.z - pa.z;
                float v2x = x - pa.x, v2z = z - pa.z;
                float den = v0x * v1z - v1x * v0z;
                y = 0f;
                if (Mathf.Abs(den) < 1e-12f)
                {
                    return false;
                }
                float u = (v2x * v1z - v1x * v2z) / den;
                float v = (v0x * v2z - v2x * v0z) / den;
                if (u >= -1e-5f && v >= -1e-5f && u + v <= 1f + 1e-5f)
                {
                    y = pa.y + u * (pc.y - pa.y) + v * (pb.y - pa.y);
                    return true;
                }
                return false;
            }
        }
    }
}
