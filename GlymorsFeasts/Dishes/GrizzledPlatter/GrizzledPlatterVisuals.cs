using System;
using System.Collections.Generic;
using UnityEngine;

namespace GlymorsFeasts
{
    internal static class GrizzledPlatterVisuals
    {
        private static readonly string[] PlatterNameHints =
        {
            "platter", "plate", "tray", "plateau", "dish", "board", "bowl",
            "wood", "wooden", "table", "pewter", "tin", "iron", "metal",
            "stand", "base"
        };

        private static readonly string[] StripNameHints =
        {
            "fx_itemsparkles", "fx_foodsteam_small", "fx_foodsteam", "equipoffset",
            "blast", "fern", "leaf", "flower", "sparkle"
        };

        // Inner deck of feast_blackforest_4 in the board/full local space
        // (same space as the vanilla full/half/empty/board children under New).
        private const float BoardDeckLocalY = 0.082f;
        private static readonly Color MeatWarmth = new Color(1.34f, 1.08f, 0.82f, 1f);
        private static readonly Color MeatJuicy = new Color(1.46f, 1.20f, 0.94f, 1f);
        private static readonly Color ShredTint = new Color(1.16f, 0.88f, 0.62f, 1f);
        private static readonly Color TurnipRoast = new Color(1.05f, 0.82f, 0.55f, 1f);

        internal static void Compose(GameObject piece, Feast feast)
        {
            if (piece == null)
            {
                return;
            }

            GameObject steam = FindNamed(piece.transform, "fx_FoodSteam");
            GameObject board = FindNamed(piece.transform, "board");
            GameObject full = FindNamed(piece.transform, "full");
            GameObject newRoot = FindNamed(piece.transform, "New");

            // Vanilla full/half/empty meshes already include the board, so the
            // feast component only enables the bare "board" child when the platter
            // is empty. We hide those combined meshes, so the board must stay on
            // at every stage or the food appears to sit on the table.
            KeepBoardVisible(board);

            Bounds platterBounds = MeasurePlatter(piece, board, out List<Renderer> keepRenderers);

            if (steam != null)
            {
                foreach (Renderer r in steam.GetComponentsInChildren<Renderer>(true))
                {
                    if (!keepRenderers.Contains(r))
                    {
                        keepRenderers.Add(r);
                    }
                }
            }
            if (board != null)
            {
                foreach (Renderer r in board.GetComponentsInChildren<Renderer>(true))
                {
                    if (!keepRenderers.Contains(r))
                    {
                        keepRenderers.Add(r);
                    }
                }
            }

            HideOriginalFood(piece, keepRenderers);

            // Vanilla food stages are children of New at local (0, -0.069, 0),
            // the same pose as the board. Parent our layout there so it shares
            // the board's space and the piece pivot (board bottom at y≈0).
            Transform stageParent = newRoot != null ? newRoot.transform : piece.transform;
            Transform root = new GameObject("PulledBearPlatter").transform;
            root.SetParent(stageParent, false);
            Transform poseSrc = board != null ? board.transform : (full != null ? full.transform : null);
            if (poseSrc != null && poseSrc.parent == stageParent)
            {
                root.localPosition = poseSrc.localPosition;
                root.localRotation = poseSrc.localRotation;
                root.localScale = Vector3.one;
            }
            else
            {
                root.localPosition = new Vector3(0f, -0.069f, 0f);
                root.localRotation = Quaternion.identity;
                root.localScale = Vector3.one;
            }

            // Board mesh is almost centered on its origin; don't use world bounds
            // from compose-time (the board starts inactive, bounds can be stale).
            Vector3 localCenter = Vector3.zero;

            GameObject foodHigh = new GameObject("Food_High");
            GameObject foodMid = new GameObject("Food_Mid");
            GameObject foodLow = new GameObject("Food_Low");
            foreach (GameObject go in new[] { foodHigh, foodMid, foodLow })
            {
                go.transform.SetParent(root, false);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
            }

            BuildServedDish(foodHigh.transform, piece, full, localCenter, BoardDeckLocalY, Stage.Full);
            BuildServedDish(foodMid.transform, piece, full, localCenter, BoardDeckLocalY, Stage.Half);
            BuildServedDish(foodLow.transform, piece, full, localCenter, BoardDeckLocalY, Stage.Leftover);

            if (steam != null)
            {
                steam.transform.SetParent(stageParent, true);
                steam.transform.localPosition = root.localPosition + new Vector3(0f, BoardDeckLocalY + 0.22f, 0f);
                steam.transform.localRotation = Quaternion.identity;
            }
            else if (GrizzledPlatterConfig.EnableSteam.Value)
            {
                Vector3 steamPos = root.localPosition + new Vector3(0f, BoardDeckLocalY + 0.22f, 0f);
                steam = CloneVanillaFoodSteam(stageParent, steamPos);
            }

            Plugin.Log.LogInfo(string.Format(
                "Platter align: New local={0} board local={1} (parent={2}) foodRoot local={3} deckY={4}",
                newRoot != null ? newRoot.transform.localPosition.ToString("F4") : "none",
                board != null ? board.transform.localPosition.ToString("F4") : "none",
                board != null && board.transform.parent != null ? board.transform.parent.name : "none",
                root.localPosition.ToString("F4"),
                BoardDeckLocalY));

            if (steam != null)
            {
                steam.SetActive(GrizzledPlatterConfig.EnableSteam.Value);
            }

            List<Feast.FeastLevel> levels = new List<Feast.FeastLevel>
            {
                new Feast.FeastLevel
                {
                    m_onAboveEquals = foodHigh,
                    m_onBelow = foodMid,
                    m_threshold = 0.7f,
                    m_thresholdBelowMax = 0.4f
                },
                new Feast.FeastLevel
                {
                    m_onBelow = foodLow,
                    m_threshold = 0.4f,
                    m_thresholdBelowMax = 0.1f
                }
            };
            if (steam != null && GrizzledPlatterConfig.EnableSteam.Value)
            {
                levels.Add(new Feast.FeastLevel
                {
                    m_onAboveEquals = steam,
                    m_threshold = 0.1f,
                    m_thresholdBelowMax = 0f
                });
                Plugin.Log.LogInfo("Using vanilla feast steam at " + PathOf(steam.transform));
            }
            else if (!GrizzledPlatterConfig.EnableSteam.Value)
            {
                Plugin.Log.LogInfo("Steam disabled by config.");
            }
            else
            {
                Plugin.Log.LogWarning("No fx_FoodSteam found; the platter will have no steam.");
            }

            feast.m_feastParts = levels;
            feast.UpdateVisual();
            KeepBoardVisible(board);
        }

        private enum Stage
        {
            Full,
            Half,
            Leftover
        }

        private static void BuildServedDish(Transform parent, GameObject piece, GameObject full, Vector3 center, float deckWorldY, Stage stage)
        {
            MatBit[] mats;
            MeatBit[] body;
            StrandBit[] strands;
            ShredBit[] shreds;
            VegBit[] vegs;
            bool herbs;
            if (stage == Stage.Full)
            {
                mats = Full.Mats();
                body = Full.Body();
                strands = Full.Strands();
                shreds = Full.Shreds();
                vegs = LayoutVeg.Full();
                herbs = true;
            }
            else if (stage == Stage.Half)
            {
                mats = Half.Mats();
                body = Half.Body();
                strands = Half.Strands();
                shreds = Half.Shreds();
                vegs = LayoutVeg.Half();
                herbs = false;
            }
            else
            {
                mats = Low.Mats();
                body = Low.Body();
                strands = Low.Strands();
                shreds = Low.Shreds();
                vegs = LayoutVeg.Low();
                herbs = false;
            }

            Mesh[] matMeshes = { MeshBank.StrandMatA, MeshBank.StrandMatB, MeshBank.StrandMatC };
            foreach (MeatBit bit in body)
            {
                Vector3 pos = center + new Vector3(bit.Xz.x, 0f, bit.Xz.y);
                GameObject go = PlaceRaw(MeshBank.PulledMeat, "PulledBear", parent, pos, bit.Euler, bit.Scale, deckWorldY + bit.Lift, "MeatBody");
                if (go != null)
                {
                    TintRenderers(go, bit.Juicy ? MeatJuicy : MeatWarmth);
                    GlossRenderers(go, 0.58f);
                }
            }

            foreach (MatBit bit in mats)
            {
                Vector3 pos = center + new Vector3(bit.Xz.x, 0f, bit.Xz.y);
                Mesh mesh = matMeshes[Mathf.Clamp(bit.Mesh, 0, 2)];
                GameObject go = PlaceRaw(mesh, "PulledBear", parent, pos, bit.Euler, bit.Scale, deckWorldY + bit.Lift, "StrandMat");
                if (go != null)
                {
                    TintRenderers(go, bit.Juicy ? MeatJuicy : MeatWarmth);
                    GlossRenderers(go, bit.Juicy ? 0.66f : 0.60f);
                }
            }

            Mesh[] clumpMeshes = { MeshBank.StrandClumpA, MeshBank.StrandClumpB, MeshBank.StrandClumpC };
            foreach (StrandBit bit in strands)
            {
                Vector3 pos = center + new Vector3(bit.Xz.x, 0f, bit.Xz.y);
                Mesh mesh = clumpMeshes[Mathf.Clamp(bit.Mesh, 0, 2)];
                GameObject go = PlaceRaw(mesh, "PulledBear", parent, pos, bit.Euler, Vector3.one * bit.Scale, deckWorldY + bit.Lift, "StrandClump");
                if (go != null)
                {
                    TintRenderers(go, bit.Juicy ? MeatJuicy : MeatWarmth);
                    GlossRenderers(go, bit.Juicy ? 0.78f : 0.68f);
                }
            }

            Mesh[] shredMeshes = { MeshBank.FibreShredA, MeshBank.FibreShredB };
            foreach (ShredBit bit in shreds)
            {
                Vector3 pos = center + new Vector3(bit.Xz.x, 0f, bit.Xz.y);
                Mesh mesh = shredMeshes[Mathf.Clamp(bit.Mesh, 0, 1)];
                GameObject go = PlaceRaw(mesh, "PulledBear", parent, pos, bit.Euler, Vector3.one * bit.Scale, deckWorldY + bit.Lift, "FibreShred");
                if (go != null)
                {
                    TintRenderers(go, ShredTint);
                    GlossRenderers(go, 0.48f);
                }
            }

            foreach (VegBit veg in vegs)
            {
                float h = BedHeight(veg.Xz.x, veg.Xz.y);
                float sink = deckWorldY + Mathf.Max(0.012f, h * 0.35f);
                Vector3 pos = center + new Vector3(veg.Xz.x, 0f, veg.Xz.y);
                if (veg.Kind == 2)
                {
                    GameObject turnip = Place("Turnip", parent, pos, veg.Euler, Vector3.one * veg.Scale, sink);
                    if (turnip != null)
                    {
                        ReplaceMesh(turnip, MeshBank.TurnipHalf);
                        TintRenderers(turnip, TurnipRoast);
                        GlossRenderers(turnip, 0.32f);
                        SnapToDeck(turnip, sink);
                    }
                }
                else
                {
                    Mesh mesh = veg.Kind == 0 ? MeshBank.CarrotRoundA : MeshBank.CarrotRoundB;
                    PlaceRaw(mesh, "Carrot", parent, pos, veg.Euler, Vector3.one * veg.Scale, sink, "CarrotRound");
                }
            }

            if (herbs)
            {
                AddBuffetHerbs(parent, piece, full);
            }
        }

        private static float BedHeight(float x, float z)
        {
            float r = Mathf.Sqrt(x * x + z * z);
            float t = r / 0.30f;
            return 0.050f + 0.075f * Mathf.Exp(-(t * t));
        }

        // Pull only the shredded meat (and the carrots cooked into it) out of the
        // Pulled Bear item mesh, dropping the grey serving tray and its floor cap.
        private static void StripToShreddedMeat(GameObject go)
        {
            foreach (MeshFilter mf in go.GetComponentsInChildren<MeshFilter>(true))
            {
                if (mf == null || mf.sharedMesh == null)
                {
                    continue;
                }
                MeshRenderer mr = mf.GetComponent<MeshRenderer>();
                Texture albedo = null;
                if (mr != null && mr.sharedMaterial != null)
                {
                    albedo = mr.sharedMaterial.mainTexture;
                }
                Texture2D readable = MakeReadable(albedo);
                FilterTriangles(mf, (Vector3 a, Vector3 b, Vector3 c, Vector2 ua, Vector2 ub, Vector2 uc) =>
                {
                    Vector3 mid = (a + b + c) / 3f;
                    if (mid.y < 0.058f)
                    {
                        return false;
                    }
                    Vector3 n = Vector3.Cross(b - a, c - a);
                    float ny = n.y / (n.magnitude + 1e-8f);
                    float yExtent = Mathf.Max(a.y, Mathf.Max(b.y, c.y)) - Mathf.Min(a.y, Mathf.Min(b.y, c.y));
                    if (Mathf.Abs(ny) > 0.85f && yExtent < 0.008f && mid.y < 0.10f)
                    {
                        return false;
                    }
                    return IsMeatOrCarrotColor(SampleAlbedo(readable, (ua + ub + uc) / 3f));
                });
            }
        }

        private static bool IsMeatOrCarrotColor(Color col)
        {
            if (col.a < 0.2f)
            {
                return false;
            }
            float max = Mathf.Max(col.r, Mathf.Max(col.g, col.b));
            float min = Mathf.Min(col.r, Mathf.Min(col.g, col.b));
            float sat = max - min;
            float lum = (col.r + col.g + col.b) / 3f;
            if (sat < 0.22f && lum > 0.28f)
            {
                return false;
            }
            if (lum > 0.68f)
            {
                return false;
            }
            bool carrot = col.r > 0.55f && col.g > 0.25f && col.g < 0.65f && col.b < 0.40f && col.r > col.g + 0.15f;
            bool meat = col.r > 0.25f && col.r >= col.g && col.g >= col.b - 0.05f && lum < 0.65f && sat > 0.12f;
            return carrot || meat;
        }

        private static void CutMeshInHalf(GameObject go)
        {
            foreach (MeshFilter mf in go.GetComponentsInChildren<MeshFilter>(true))
            {
                if (mf == null || mf.sharedMesh == null)
                {
                    continue;
                }
                FilterTriangles(mf, (Vector3 a, Vector3 b, Vector3 c, Vector2 ua, Vector2 ub, Vector2 uc) =>
                {
                    return ((a + b + c) / 3f).x >= -0.05f;
                });
            }
        }

        private static void AddBuffetHerbs(Transform parent, GameObject piece, GameObject full)
        {
            if (full == null)
            {
                full = FindNamed(piece.transform, "full");
            }
            if (full == null)
            {
                return;
            }
            MeshRenderer srcRenderer = full.GetComponent<MeshRenderer>();
            if (srcRenderer == null || srcRenderer.sharedMaterial == null)
            {
                return;
            }
            Mesh herbsMesh = MeshBank.BfHerbs;
            if (herbsMesh == null)
            {
                return;
            }
            GameObject herbs = new GameObject("BuffetHerbs");
            herbs.transform.SetParent(parent, false);
            herbs.transform.position = full.transform.position;
            herbs.transform.rotation = full.transform.rotation;
            herbs.transform.localScale = Vector3.one;
            MeshFilter mf = herbs.AddComponent<MeshFilter>();
            mf.sharedMesh = herbsMesh;
            MeshRenderer mr = herbs.AddComponent<MeshRenderer>();
            mr.sharedMaterial = srcRenderer.sharedMaterial;
        }


        private static void FilterTriangles(MeshFilter mf, Func<Vector3, Vector3, Vector3, Vector2, Vector2, Vector2, bool> keep)
        {
            Mesh src = mf.sharedMesh;
            if (src == null)
            {
                return;
            }
            Vector3[] verts = src.vertices;
            Vector2[] uvs = src.uv;
            if (uvs == null || uvs.Length != verts.Length)
            {
                uvs = new Vector2[verts.Length];
            }
            int[] tris = src.triangles;
            List<int> kept = new List<int>(tris.Length);
            for (int i = 0; i + 2 < tris.Length; i += 3)
            {
                int ia = tris[i];
                int ib = tris[i + 1];
                int ic = tris[i + 2];
                if (keep(verts[ia], verts[ib], verts[ic], uvs[ia], uvs[ib], uvs[ic]))
                {
                    kept.Add(ia);
                    kept.Add(ib);
                    kept.Add(ic);
                }
            }
            Mesh copy = UnityEngine.Object.Instantiate(src);
            copy.name = src.name + "_filtered";
            copy.triangles = kept.ToArray();
            copy.RecalculateBounds();
            mf.sharedMesh = copy;
        }

        private static Texture2D MakeReadable(Texture tex)
        {
            if (tex == null)
            {
                return null;
            }
            Texture2D direct = tex as Texture2D;
            if (direct != null && direct.isReadable)
            {
                return direct;
            }
            int w = tex.width;
            int h = tex.height;
            RenderTexture rt = RenderTexture.GetTemporary(w, h, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(tex, rt);
            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D copy = new Texture2D(w, h, TextureFormat.RGBA32, false);
            copy.ReadPixels(new Rect(0f, 0f, w, h), 0, 0);
            copy.Apply();
            RenderTexture.active = prev;
            RenderTexture.ReleaseTemporary(rt);
            return copy;
        }

        private static Color SampleAlbedo(Texture2D tex, Vector2 uv)
        {
            if (tex == null)
            {
                return Color.magenta;
            }
            return tex.GetPixelBilinear(uv.x, uv.y);
        }

        private static void TintRenderers(GameObject go, Color tint)
        {
            foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true))
            {
                if (r == null || r is ParticleSystemRenderer)
                {
                    continue;
                }
                Material mat = r.material;
                if (mat != null)
                {
                    mat.color = tint;
                    if (mat.HasProperty("_Color"))
                    {
                        mat.SetColor("_Color", tint);
                    }
                }
            }
        }

        private static void GlossRenderers(GameObject go, float gloss)
        {
            if (go == null)
            {
                return;
            }
            foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true))
            {
                if (r == null || r is ParticleSystemRenderer)
                {
                    continue;
                }
                Material mat = r.material;
                if (mat == null)
                {
                    continue;
                }
                if (mat.HasProperty("_Glossiness"))
                {
                    mat.SetFloat("_Glossiness", gloss);
                }
                if (mat.HasProperty("_Smoothness"))
                {
                    mat.SetFloat("_Smoothness", gloss);
                }
                if (mat.HasProperty("_GlossMapScale"))
                {
                    mat.SetFloat("_GlossMapScale", gloss);
                }
            }
        }

        private static GameObject CloneVanillaFoodSteam(Transform parent, Vector3 localPos)
        {
            string[] sources =
            {
                "FeastBlackforest", "FeastSwamps", "FeastMeadows", "FeastPlains",
                "FeastMistlands", "FeastMountains", "FeastOceans", "FeastAshlands", "FeastDeepNorth"
            };
            foreach (string name in sources)
            {
                GameObject prefab = GrizzledPlatterDish.FindPrefab(name);
                if (prefab == null)
                {
                    continue;
                }
                GameObject src = FindNamed(prefab.transform, "fx_FoodSteam");
                if (src == null)
                {
                    continue;
                }
                GameObject clone = UnityEngine.Object.Instantiate(src, parent);
                clone.name = "fx_FoodSteam";
                clone.transform.localPosition = localPos;
                clone.transform.localRotation = Quaternion.identity;
                Plugin.Log.LogInfo("Cloned fx_FoodSteam from " + name);
                return clone;
            }
            return null;
        }

        private static Bounds MeasurePlatter(GameObject piece, GameObject board, out List<Renderer> platterRenderers)
        {
            platterRenderers = new List<Renderer>();
            if (board != null)
            {
                platterRenderers.AddRange(board.GetComponentsInChildren<Renderer>(true));
                if (platterRenderers.Count > 0)
                {
                    Bounds b = platterRenderers[0].bounds;
                    for (int i = 1; i < platterRenderers.Count; i++)
                    {
                        b.Encapsulate(platterRenderers[i].bounds);
                    }
                    return b;
                }
            }

            Renderer[] renderers = piece.GetComponentsInChildren<Renderer>(true);
            Bounds all = new Bounds(piece.transform.position, Vector3.one * 0.4f);
            bool any = false;
            float lowest = float.MaxValue;
            Renderer lowestR = null;
            foreach (Renderer r in renderers)
            {
                if (r == null || r is ParticleSystemRenderer)
                {
                    continue;
                }
                string n = r.gameObject.name.ToLowerInvariant();
                foreach (string h in PlatterNameHints)
                {
                    if (n.Contains(h))
                    {
                        platterRenderers.Add(r);
                        break;
                    }
                }
                if (!any)
                {
                    all = r.bounds;
                    any = true;
                }
                else
                {
                    all.Encapsulate(r.bounds);
                }
                if (r.bounds.center.y < lowest)
                {
                    lowest = r.bounds.center.y;
                    lowestR = r;
                }
            }
            if (platterRenderers.Count == 0 && lowestR != null)
            {
                platterRenderers.Add(lowestR);
            }
            if (platterRenderers.Count > 0)
            {
                Bounds b = platterRenderers[0].bounds;
                for (int i = 1; i < platterRenderers.Count; i++)
                {
                    b.Encapsulate(platterRenderers[i].bounds);
                }
                return b;
            }
            return all;
        }

        private static float EstimateDeckWorldY(GameObject board, Bounds platterBounds)
        {
            // Never read Mesh.vertices: vanilla feast meshes are not CPU-readable
            // in the player and will log/throw. Renderer bounds are safe.
            if (board != null)
            {
                Renderer r = board.GetComponentInChildren<Renderer>();
                if (r != null)
                {
                    return r.bounds.min.y + 0.055f;
                }
            }
            return platterBounds.min.y + 0.055f;
        }

        private static void KeepBoardVisible(GameObject board)
        {
            if (board == null)
            {
                return;
            }
            board.SetActive(true);
            foreach (Renderer r in board.GetComponentsInChildren<Renderer>(true))
            {
                if (r != null && !(r is ParticleSystemRenderer))
                {
                    r.enabled = true;
                }
            }
        }

        private static void HideOriginalFood(GameObject piece, List<Renderer> keep)
        {
            HashSet<Renderer> keepSet = new HashSet<Renderer>(keep);
            foreach (Renderer r in piece.GetComponentsInChildren<Renderer>(true))
            {
                if (r == null || keepSet.Contains(r))
                {
                    continue;
                }
                if (IsSteamObject(r.transform))
                {
                    continue;
                }
                if (r is ParticleSystemRenderer)
                {
                    continue;
                }
                r.enabled = false;
            }
        }

        private static bool IsSteamObject(Transform t)
        {
            while (t != null)
            {
                string n = t.name.ToLowerInvariant();
                if (n.Contains("fx_foodsteam") || n == "fx_foodsteam")
                {
                    return true;
                }
                t = t.parent;
            }
            return false;
        }

        private static readonly Dictionary<string, Material> MaterialCache = new Dictionary<string, Material>();

        private static Material MaterialFrom(string prefabName)
        {
            Material cached;
            if (MaterialCache.TryGetValue(prefabName, out cached) && cached != null)
            {
                return cached;
            }
            GameObject tmp = ExtractVisual(prefabName);
            if (tmp == null)
            {
                return null;
            }
            MeshRenderer mr = tmp.GetComponentInChildren<MeshRenderer>(true);
            Material mat = mr != null ? mr.sharedMaterial : null;
            UnityEngine.Object.DestroyImmediate(tmp);
            if (mat != null)
            {
                MaterialCache[prefabName] = mat;
            }
            return mat;
        }

        private static GameObject PlaceRaw(Mesh mesh, string materialPrefab, Transform parent, Vector3 localPos, Vector3 euler, Vector3 scale, float deckWorldY, string name)
        {
            if (mesh == null)
            {
                return null;
            }
            Material mat = MaterialFrom(materialPrefab);
            if (mat == null)
            {
                Plugin.Log.LogWarning("No material from " + materialPrefab + " for " + name);
                return null;
            }
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPos;
            go.transform.localRotation = Quaternion.Euler(euler);
            go.transform.localScale = scale;
            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = mat;
            SnapToDeck(go, deckWorldY);
            return go;
        }

        private static GameObject Place(string prefabName, Transform parent, Vector3 localPos, Vector3 euler, float scale, float deckWorldY)
        {
            return Place(prefabName, parent, localPos, euler, Vector3.one * scale, deckWorldY);
        }

        private static GameObject Place(string prefabName, Transform parent, Vector3 localPos, Vector3 euler, Vector3 scale, float deckWorldY)
        {
            GameObject visual;
            try
            {
                visual = ExtractVisual(prefabName);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError("ExtractVisual(" + prefabName + ") failed: " + ex);
                return null;
            }
            if (visual == null)
            {
                return null;
            }
            visual.name = prefabName + "_visual";
            visual.transform.SetParent(parent, false);
            visual.transform.localPosition = localPos;
            visual.transform.localRotation = Quaternion.Euler(euler);
            visual.transform.localScale = scale;
            SnapToDeck(visual, deckWorldY);
            return visual;
        }

        private static void SnapToDeck(GameObject go, float deckLocalY)
        {
            if (go == null || go.transform.parent == null)
            {
                return;
            }
            Transform space = go.transform.parent;
            float minY = float.MaxValue;
            bool any = false;
            foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true))
            {
                if (r == null || !r.enabled || r is ParticleSystemRenderer)
                {
                    continue;
                }
                Bounds b = r.bounds;
                Vector3 c = b.center;
                Vector3 e = b.extents;
                for (int i = 0; i < 8; i++)
                {
                    Vector3 corner = c + new Vector3(
                        (i & 1) == 0 ? -e.x : e.x,
                        (i & 2) == 0 ? -e.y : e.y,
                        (i & 4) == 0 ? -e.z : e.z);
                    float y = space.InverseTransformPoint(corner).y;
                    if (y < minY)
                    {
                        minY = y;
                    }
                    any = true;
                }
            }
            if (!any)
            {
                return;
            }
            float dy = deckLocalY - minY;
            if (Mathf.Abs(dy) > 0.0001f)
            {
                // Move along the parent's up so XZ in board space stays put.
                go.transform.localPosition += new Vector3(0f, dy, 0f);
            }
        }

        private static Bounds? RenderBounds(GameObject go)
        {
            Bounds? b = null;
            foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true))
            {
                if (r == null || !r.enabled || r is ParticleSystemRenderer)
                {
                    continue;
                }
                if (!b.HasValue)
                {
                    b = r.bounds;
                }
                else
                {
                    Bounds bb = b.Value;
                    bb.Encapsulate(r.bounds);
                    b = bb;
                }
            }
            return b;
        }

        private static readonly Dictionary<string, GameObject> VisualPrototypes = new Dictionary<string, GameObject>();

        private static GameObject ExtractVisual(string prefabName)
        {
            GameObject proto;
            if (!VisualPrototypes.TryGetValue(prefabName, out proto) || proto == null)
            {
                GameObject src = GrizzledPlatterDish.FindPrefab(prefabName);
                if (src == null)
                {
                    Plugin.Log.LogWarning("Missing visual prefab: " + prefabName);
                    return null;
                }
                proto = new GameObject(prefabName + "_proto");
                proto.SetActive(false);
                UnityEngine.Object.DontDestroyOnLoad(proto);
                CopyVisualTree(src.transform, proto.transform, isRoot: true);
                VisualPrototypes[prefabName] = proto;
            }
            GameObject clone = UnityEngine.Object.Instantiate(proto);
            clone.name = prefabName;
            clone.SetActive(true);
            return clone;
        }

        private static bool ShouldSkipVisual(string name)
        {
            string n = name.ToLowerInvariant();
            foreach (string hint in StripNameHints)
            {
                if (n == hint || n.Contains(hint))
                {
                    return true;
                }
            }
            return n.StartsWith("fx_");
        }

        private static void CopyVisualTree(Transform src, Transform dst, bool isRoot)
        {
            if (!isRoot)
            {
                dst.localPosition = src.localPosition;
                dst.localRotation = src.localRotation;
                dst.localScale = src.localScale;
            }
            MeshFilter srcFilter = src.GetComponent<MeshFilter>();
            MeshRenderer srcRenderer = src.GetComponent<MeshRenderer>();
            if (srcFilter != null && srcFilter.sharedMesh != null && srcRenderer != null)
            {
                MeshFilter dstFilter = dst.gameObject.AddComponent<MeshFilter>();
                dstFilter.sharedMesh = srcFilter.sharedMesh;
                MeshRenderer dstRenderer = dst.gameObject.AddComponent<MeshRenderer>();
                dstRenderer.sharedMaterials = srcRenderer.sharedMaterials;
            }
            for (int i = 0; i < src.childCount; i++)
            {
                Transform child = src.GetChild(i);
                if (child == null || ShouldSkipVisual(child.name))
                {
                    continue;
                }
                GameObject childGo = new GameObject(child.name);
                childGo.transform.SetParent(dst, false);
                CopyVisualTree(child, childGo.transform, isRoot: false);
            }
        }

        private static void StripGameplay(GameObject go)
        {
            foreach (ZNetView nv in go.GetComponentsInChildren<ZNetView>(true))
            {
                UnityEngine.Object.DestroyImmediate(nv);
            }
            foreach (ItemDrop drop in go.GetComponentsInChildren<ItemDrop>(true))
            {
                UnityEngine.Object.DestroyImmediate(drop);
            }
            foreach (Piece piece in go.GetComponentsInChildren<Piece>(true))
            {
                UnityEngine.Object.DestroyImmediate(piece);
            }
            foreach (Feast feast in go.GetComponentsInChildren<Feast>(true))
            {
                UnityEngine.Object.DestroyImmediate(feast);
            }
            foreach (WearNTear wear in go.GetComponentsInChildren<WearNTear>(true))
            {
                UnityEngine.Object.DestroyImmediate(wear);
            }
            foreach (Rigidbody rb in go.GetComponentsInChildren<Rigidbody>(true))
            {
                UnityEngine.Object.DestroyImmediate(rb);
            }
            foreach (Collider col in go.GetComponentsInChildren<Collider>(true))
            {
                UnityEngine.Object.DestroyImmediate(col);
            }
            foreach (FloatingTerrain floating in go.GetComponentsInChildren<FloatingTerrain>(true))
            {
                UnityEngine.Object.DestroyImmediate(floating);
            }
            foreach (Floating floating in go.GetComponentsInChildren<Floating>(true))
            {
                UnityEngine.Object.DestroyImmediate(floating);
            }
            foreach (Light light in go.GetComponentsInChildren<Light>(true))
            {
                UnityEngine.Object.DestroyImmediate(light);
            }
            foreach (Component c in go.GetComponentsInChildren<Component>(true))
            {
                if (c == null)
                {
                    continue;
                }
                string tn = c.GetType().Name;
                if (tn == "ZSyncTransform" || tn == "ZSyncAnimation" || tn == "HoverText" || tn == "Pickable")
                {
                    UnityEngine.Object.DestroyImmediate(c);
                }
            }
        }

        private static void StripItemDropPresentation(GameObject go)
        {
            List<GameObject> doomed = new List<GameObject>();
            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            {
                if (t == null || t.gameObject == go)
                {
                    continue;
                }
                string n = t.name.ToLowerInvariant();
                foreach (string hint in StripNameHints)
                {
                    if (n == hint || n.Contains(hint))
                    {
                        doomed.Add(t.gameObject);
                        break;
                    }
                }
            }
            foreach (ParticleSystem ps in go.GetComponentsInChildren<ParticleSystem>(true))
            {
                if (ps != null)
                {
                    doomed.Add(ps.gameObject);
                }
            }
            foreach (ParticleSystemRenderer pr in go.GetComponentsInChildren<ParticleSystemRenderer>(true))
            {
                if (pr != null)
                {
                    doomed.Add(pr.gameObject);
                }
            }
            foreach (GameObject child in doomed)
            {
                if (child != null)
                {
                    UnityEngine.Object.DestroyImmediate(child);
                }
            }
        }

        private static void ReplaceMesh(GameObject go, Mesh mesh)
        {
            if (go == null || mesh == null)
            {
                return;
            }
            bool assigned = false;
            foreach (MeshFilter mf in go.GetComponentsInChildren<MeshFilter>(true))
            {
                if (mf == null)
                {
                    continue;
                }
                mf.sharedMesh = mesh;
                assigned = true;
                break;
            }
            if (!assigned)
            {
                Plugin.Log.LogWarning("No MeshFilter found to replace on " + go.name);
            }
        }

        internal static void ComposeItemDrop(GameObject material, GameObject piece)
        {
            if (material == null || piece == null)
            {
                return;
            }
            if (FindNamed(material.transform, "GrizzledPlatterDrop") != null)
            {
                return;
            }

            bool wasActive = material.activeSelf;
            LogDropPrefab("before", material);

            Transform attach = material.transform.Find("attach");
            bool hadAttach = attach != null;
            bool attachWasActive = hadAttach && attach.gameObject.activeSelf;

            // Same wooden board as the Black Forest buffet drop, not the long Swamp dish.
            Bounds? fit = MeasureItemVisual("FeastBlackforest_Material") ?? MeasureItemVisual("FeastSwamps_Material");

            DisableDropSteam(material);
            HideDropMeshes(material);

            if (!hadAttach)
            {
                GameObject created = new GameObject("attach");
                created.transform.SetParent(material.transform, false);
                created.transform.localPosition = Vector3.zero;
                created.transform.localRotation = Quaternion.identity;
                created.transform.localScale = Vector3.one;
                // ItemStand finds attach even when it is inactive. Keep it off so the
                // world drop does not draw two platters.
                created.SetActive(false);
                attach = created.transform;
                Plugin.Log.LogInfo("Created inactive attach child on " + material.name);
            }

            if (hadAttach && attachWasActive)
            {
                GameObject attachVis = BuildDropVisual(attach, piece, "GrizzledPlatterDrop");
                FitDropVisual(attachVis.transform, attach, fit);
            }
            else
            {
                GameObject worldVis = BuildDropVisual(material.transform, piece, "GrizzledPlatterDrop");
                FitDropVisual(worldVis.transform, material.transform, fit);
                if (attach != null)
                {
                    GameObject attachVis = BuildDropVisual(attach, piece, "GrizzledPlatterDrop");
                    FitDropVisual(attachVis.transform, attach, fit);
                    if (!attachWasActive)
                    {
                        attach.gameObject.SetActive(false);
                    }
                }
            }

            if (material.activeSelf != wasActive)
            {
                material.SetActive(wasActive);
                Plugin.Log.LogWarning("Restored item prefab activeSelf=" + wasActive + " on " + material.name);
            }
            LogDropPrefab("after", material);
        }

        private static Bounds? MeasureItemVisual(string prefabName)
        {
            GameObject go = GrizzledPlatterDish.FindPrefab(prefabName);
            if (go == null)
            {
                return null;
            }
            Transform attach = go.transform.Find("attach");
            if (attach != null && attach.gameObject.activeSelf)
            {
                return LocalMeshBounds(attach, attach);
            }
            return LocalMeshBounds(go.transform, go.transform, attach);
        }

        private static void LogDropPrefab(string label, GameObject go)
        {
            ZNetView nv = go.GetComponent<ZNetView>();
            ItemDrop drop = go.GetComponent<ItemDrop>();
            Rigidbody rb = go.GetComponent<Rigidbody>();
            int ps = go.GetComponentsInChildren<ParticleSystem>(true).Length;
            Plugin.Log.LogInfo(string.Format(
                "{0} drop prefab {1}: active={2} nview={3} itemDrop={4} rigidbody={5} particles={6} children={7}",
                label, go.name, go.activeSelf, nv != null, drop != null, rb != null, ps, go.transform.childCount));
        }

        private static GameObject BuildDropVisual(Transform parent, GameObject piece, string name)
        {
            GameObject root = new GameObject(name);
            root.transform.SetParent(parent, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            GameObject board = FindNamed(piece.transform, "board");
            GameObject foodHigh = FindNamed(piece.transform, "Food_High");
            if (board != null)
            {
                CopyRendererTree(board.transform, root.transform, piece.transform, "board");
            }
            if (foodHigh != null)
            {
                CopyRendererTree(foodHigh.transform, root.transform, piece.transform, "Food_High");
            }
            return root;
        }

        private static void CopyRendererTree(Transform src, Transform destParent, Transform space, string name)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(destParent, false);
            go.transform.localPosition = space.InverseTransformPoint(src.position);
            go.transform.localRotation = Quaternion.Inverse(space.rotation) * src.rotation;
            Vector3 srcScale = src.lossyScale;
            Vector3 spaceScale = space.lossyScale;
            go.transform.localScale = new Vector3(
                SafeDiv(srcScale.x, spaceScale.x),
                SafeDiv(srcScale.y, spaceScale.y),
                SafeDiv(srcScale.z, spaceScale.z));

            MeshFilter mf = src.GetComponent<MeshFilter>();
            MeshRenderer mr = src.GetComponent<MeshRenderer>();
            if (mf != null && mf.sharedMesh != null && mr != null && mr.enabled && !(mr is ParticleSystemRenderer))
            {
                MeshFilter copyF = go.AddComponent<MeshFilter>();
                copyF.sharedMesh = mf.sharedMesh;
                MeshRenderer copyR = go.AddComponent<MeshRenderer>();
                copyR.sharedMaterials = mr.sharedMaterials;
                copyR.enabled = true;
            }

            foreach (Transform child in src)
            {
                if (child == null || IsSteamObject(child))
                {
                    continue;
                }
                if (child.GetComponent<ParticleSystem>() != null)
                {
                    continue;
                }
                if (child.GetComponentsInChildren<MeshRenderer>(true).Length == 0)
                {
                    continue;
                }
                CopyRendererTree(child, go.transform, space, child.name);
            }
        }

        private static float SafeDiv(float a, float b)
        {
            return Mathf.Abs(b) < 1e-6f ? a : a / b;
        }

        private static void FitDropVisual(Transform visual, Transform space, Bounds? target)
        {
            if (visual == null)
            {
                return;
            }
            Bounds? built = LocalMeshBounds(visual, space);
            if (!built.HasValue)
            {
                Plugin.Log.LogWarning("Drop visual on " + space.name + " has no meshes.");
                return;
            }
            if (!target.HasValue || target.Value.size.sqrMagnitude < 1e-6f)
            {
                Plugin.Log.LogInfo("Drop visual on " + space.name + " left at piece scale; no original item bounds to match.");
                return;
            }
            Bounds oldB = target.Value;
            Bounds newB = built.Value;
            float oldXz = Mathf.Max(oldB.size.x, oldB.size.z);
            float newXz = Mathf.Max(newB.size.x, newB.size.z);
            if (newXz < 1e-4f)
            {
                return;
            }
            float s = oldXz / newXz;
            // Never make our square board larger than the placed platter just to
            // match the longer Swamp Dweller dish.
            if (s > 1.05f)
            {
                Plugin.Log.LogInfo(string.Format("Capping drop scale {0:0.000} -> 1.00 (target xz {1:0.000}, built {2:0.000})", s, oldXz, newXz));
                s = 1.0f;
            }
            visual.localScale = visual.localScale * s;
            Bounds? scaled = LocalMeshBounds(visual, space);
            if (!scaled.HasValue)
            {
                return;
            }
            Vector3 delta = oldB.center - scaled.Value.center;
            delta.y = oldB.min.y - scaled.Value.min.y;
            visual.localPosition += delta;
            Plugin.Log.LogInfo(string.Format(
                "Fitted drop visual under {0}: scale={1:0.000} size {2:0.000}->{3:0.000}",
                space.name, s, newXz, oldXz));
        }

        private static Bounds? LocalMeshBounds(Transform root, Transform space, Transform exclude = null)
        {
            bool any = false;
            Bounds acc = new Bounds(Vector3.zero, Vector3.zero);
            foreach (MeshFilter mf in root.GetComponentsInChildren<MeshFilter>(true))
            {
                if (mf == null || mf.sharedMesh == null)
                {
                    continue;
                }
                if (exclude != null && (mf.transform == exclude || mf.transform.IsChildOf(exclude)))
                {
                    continue;
                }
                if (!IsLocallyActiveUnder(mf.transform, root))
                {
                    continue;
                }
                MeshRenderer mr = mf.GetComponent<MeshRenderer>();
                if (mr != null && !mr.enabled)
                {
                    continue;
                }
                if (IsSteamObject(mf.transform) || mf.GetComponent<ParticleSystemRenderer>() != null)
                {
                    continue;
                }
                Bounds mb = mf.sharedMesh.bounds;
                Vector3 c = mb.center;
                Vector3 e = mb.extents;
                for (int i = 0; i < 8; i++)
                {
                    Vector3 corner = c + new Vector3(
                        (i & 1) == 0 ? -e.x : e.x,
                        (i & 2) == 0 ? -e.y : e.y,
                        (i & 4) == 0 ? -e.z : e.z);
                    Vector3 world = mf.transform.TransformPoint(corner);
                    Vector3 local = space.InverseTransformPoint(world);
                    if (!any)
                    {
                        acc = new Bounds(local, Vector3.zero);
                        any = true;
                    }
                    else
                    {
                        acc.Encapsulate(local);
                    }
                }
            }
            return any ? acc : (Bounds?)null;
        }

        private static bool IsLocallyActiveUnder(Transform t, Transform root)
        {
            while (t != null && t != root)
            {
                if (!t.gameObject.activeSelf)
                {
                    return false;
                }
                t = t.parent;
            }
            return true;
        }

        private static void HideDropMeshes(GameObject material)
        {
            foreach (Renderer r in material.GetComponentsInChildren<Renderer>(true))
            {
                if (r == null || r is ParticleSystemRenderer)
                {
                    continue;
                }
                r.enabled = false;
            }
        }

        private static void DisableDropSteam(GameObject material)
        {
            foreach (ParticleSystem ps in material.GetComponentsInChildren<ParticleSystem>(true))
            {
                if (ps == null)
                {
                    continue;
                }
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                var emission = ps.emission;
                emission.enabled = false;
                // Never deactivate the item prefab root (that leaves Instantiate
                // copies inactive, so ItemDrop.Awake never runs and SetStack NREs).
                if (ps.gameObject == material || ps.GetComponent<ItemDrop>() != null || ps.GetComponent<ZNetView>() != null)
                {
                    continue;
                }
                ps.gameObject.SetActive(false);
            }
            GameObject steam = FindNamed(material.transform, "fx_FoodSteam");
            if (steam != null && steam != material)
            {
                steam.SetActive(false);
            }
        }

        private static GameObject FindNamed(Transform root, string name)

        {
            if (root == null)
            {
                return null;
            }
            if (root.name == name)
            {
                return root.gameObject;
            }
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child != null && child.name == name)
                {
                    return child.gameObject;
                }
            }
            return null;
        }

        private static string PathOf(Transform t)
        {
            List<string> parts = new List<string>();
            Transform cur = t;
            while (cur != null)
            {
                parts.Add(cur.name);
                cur = cur.parent;
            }
            parts.Reverse();
            return string.Join("/", parts.ToArray());
        }
    }
}
