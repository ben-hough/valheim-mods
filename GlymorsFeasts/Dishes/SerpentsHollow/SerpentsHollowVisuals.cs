using System;
using System.Collections.Generic;
using UnityEngine;

namespace GlymorsFeasts
{
    internal static class SerpentsHollowVisuals
    {
        private static readonly string[] StripNameHints =
        {
            "fx_itemsparkles", "fx_foodsteam_small", "fx_foodsteam", "equipoffset",
            "blast", "fern", "leaf", "flower", "sparkle"
        };

        private static readonly Dictionary<string, Material> MaterialCache = new Dictionary<string, Material>();
        private static readonly Dictionary<string, GameObject> VisualPrototypes = new Dictionary<string, GameObject>();

        internal static void Compose(GameObject piece, Feast feast)
        {
            if (piece == null)
            {
                return;
            }
            if (!IsOurPrefab(piece))
            {
                Plugin.Log.LogError("Compose refused to touch " + piece.name + " (not a Serpent's Hollow prefab).");
                return;
            }

            StripVanillaLeftovers(piece);
            DisableLodGroups(piece);

            GameObject steam = FindNamed(piece.transform, "fx_FoodSteam");
            GameObject board = FindNamed(piece.transform, "board");
            GameObject full = FindNamed(piece.transform, "full");
            GameObject newRoot = FindNamed(piece.transform, "New");

            KeepBoardVisible(board);
            SwapBoardMesh(board);

            List<Renderer> keepRenderers = new List<Renderer>();
            if (board != null)
            {
                keepRenderers.AddRange(board.GetComponentsInChildren<Renderer>(true));
            }
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

            HideOriginalFood(piece, keepRenderers);

            Transform stageParent = newRoot != null ? newRoot.transform : piece.transform;
            Transform root = new GameObject("SerpentBreadBowl").transform;
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

            // Pot + floor sit on every stage, including the bare board.
            // The ladle is placed with the stew (full and half only) so it
            // does not hang in mid-air over the leftover puddle or the empty pot.
            Transform potShared = new GameObject("PotShared").transform;
            potShared.SetParent(root, false);
            Matrix4x4 potMatrix = PlacePotGroup(potShared, includeStew: false, stew: null);

            // The three built-in ramekins are part of the board mesh and stay
            // filled on every stage, including the bare board.
            Transform dishJams = new GameObject("DishJams").transform;
            dishJams.SetParent(root, false);
            PlaceDishJams(dishJams);

            BuildServedDish(foodHigh.transform, Stage.Full, potMatrix);
            BuildServedDish(foodMid.transform, Stage.Half, potMatrix);
            BuildServedDish(foodLow.transform, Stage.Leftover, potMatrix);

            Vector3 potTrans = potMatrix.GetColumn(3);
            Vector3 potSteamPos = root.localPosition + potTrans;
            potSteamPos.y += 0.50f;
            Vector3 bowlSteamPos = root.localPosition + SerpentsHollowLayout.FilledBowl.Pos;
            bowlSteamPos.y += SerpentsHollowLayout.DeckY + 0.22f;

            GameObject potSteam = steam;
            if (potSteam != null)
            {
                potSteam.transform.SetParent(stageParent, true);
                potSteam.transform.localPosition = potSteamPos;
                potSteam.transform.localRotation = Quaternion.identity;
                potSteam.name = "fx_FoodSteam";
            }
            else if (SerpentsHollowConfig.EnableSteam.Value)
            {
                potSteam = CloneVanillaFoodSteam(stageParent, potSteamPos, "fx_FoodSteam");
            }

            // Clone bowl steam from the already-working pot steam (the original
            // FeastPlains fx_FoodSteam). Instantiating from a temporary inactive
            // copy of a whole vanilla feast produced stray blue/white glass-like
            // rings around the filled bowl in 1.1.4.
            GameObject bowlSteam = null;
            if (SerpentsHollowConfig.EnableSteam.Value)
            {
                if (potSteam != null)
                {
                    bowlSteam = UnityEngine.Object.Instantiate(potSteam);
                    bowlSteam.name = "fx_FoodSteam_Bowl";
                    bowlSteam.transform.SetParent(stageParent, false);
                    bowlSteam.transform.localPosition = bowlSteamPos;
                    bowlSteam.transform.localRotation = Quaternion.identity;
                    bowlSteam.transform.localScale = Vector3.one;
                    Plugin.Log.LogInfo("Cloned fx_FoodSteam_Bowl from the working pot steam");
                }
                else
                {
                    bowlSteam = CloneVanillaFoodSteam(stageParent, bowlSteamPos, "fx_FoodSteam_Bowl");
                }
            }

            if (potSteam != null)
            {
                potSteam.SetActive(SerpentsHollowConfig.EnableSteam.Value);
            }
            if (bowlSteam != null)
            {
                bowlSteam.SetActive(SerpentsHollowConfig.EnableSteam.Value);
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
            if (potSteam != null && SerpentsHollowConfig.EnableSteam.Value)
            {
                // On while any serving remains, same as vanilla feasts.
                levels.Add(new Feast.FeastLevel
                {
                    m_onAboveEquals = potSteam,
                    m_threshold = 0.1f,
                    m_thresholdBelowMax = 0f
                });
            }
            if (bowlSteam != null && SerpentsHollowConfig.EnableSteam.Value)
            {
                // Only while the filled bread bowl is still on the tray (full and half).
                levels.Add(new Feast.FeastLevel
                {
                    m_onAboveEquals = bowlSteam,
                    m_threshold = 0.4f,
                    m_thresholdBelowMax = 0f
                });
            }
            feast.m_feastParts = levels;
            feast.UpdateVisual();
            KeepBoardVisible(board);
            SwapBoardMesh(board);
            Plugin.Log.LogInfo(string.Format(
                "Bread-bowl platter align: New local={0} board local={1} foodRoot local={2}",
                newRoot != null ? newRoot.transform.localPosition.ToString("F4") : "none",
                board != null ? board.transform.localPosition.ToString("F4") : "none",
                root.localPosition.ToString("F4")));
        }

        private enum Stage { Full, Half, Leftover }

        private static void BuildServedDish(Transform parent, Stage stage, Matrix4x4 potMatrix)
        {
            Mesh stewMesh = stage == Stage.Full ? SerpentsHollowMeshBank.StewFull : (stage == Stage.Half ? SerpentsHollowMeshBank.StewHalf : SerpentsHollowMeshBank.StewLow);
            Mesh chunkMesh = stage == Stage.Full ? SerpentsHollowMeshBank.StewChunksFull : (stage == Stage.Half ? SerpentsHollowMeshBank.StewChunksHalf : SerpentsHollowMeshBank.StewChunksLow);
            PlaceAtMatrix(stewMesh, "SerpentStew", parent, potMatrix, "pot_stew");
            PlaceAtMatrix(chunkMesh, SerpentsHollowTexBank.MeatMaterial, parent, potMatrix, "pot_chunks");

            bool filled = stage != Stage.Leftover;
            SerpentsHollowLayout.Pose[] wholes;
            SerpentsHollowLayout.Pose[] slices;
            bool ladle = stage != Stage.Leftover;
            if (stage == Stage.Full)
            {
                wholes = new[] { SerpentsHollowLayout.WholeA, SerpentsHollowLayout.WholeB, SerpentsHollowLayout.WholeC, SerpentsHollowLayout.WholeD, SerpentsHollowLayout.WholeE, SerpentsHollowLayout.WholeF };
                slices = new[] { SerpentsHollowLayout.Slice1, SerpentsHollowLayout.Slice2, SerpentsHollowLayout.Slice3, SerpentsHollowLayout.Slice4, SerpentsHollowLayout.Slice5, SerpentsHollowLayout.Slice6 };
            }
            else if (stage == Stage.Half)
            {
                wholes = new[] { SerpentsHollowLayout.WholeA, SerpentsHollowLayout.WholeC, SerpentsHollowLayout.WholeD };
                slices = new[] { SerpentsHollowLayout.Slice1, SerpentsHollowLayout.Slice2, SerpentsHollowLayout.Slice3 };
            }
            else
            {
                wholes = new[] { SerpentsHollowLayout.WholeA };
                slices = new[] { SerpentsHollowLayout.Slice1 };
            }

            SerpentsHollowSettle session = new SerpentsHollowSettle().Init();
            Material breadMat = SerpentsHollowTexBank.BreadMaterial;
            session.Place(SerpentsHollowMeshBank.BreadBowl, breadMat, parent, SerpentsHollowLayout.EmptyBowl, "empty_bowl", "empty_bowl", allowTilt: false);
            if (filled)
            {
                GameObject bowl = session.Place(SerpentsHollowMeshBank.BreadBowl, breadMat, parent, SerpentsHollowLayout.FilledBowl, "filled_bowl", "filled_bowl", allowTilt: false);
                Mesh bowlStew = stage == Stage.Full ? SerpentsHollowMeshBank.StewBowlFull : SerpentsHollowMeshBank.StewBowlHalf;
                Mesh bowlChunks = stage == Stage.Full ? SerpentsHollowMeshBank.StewBowlChunksFull : SerpentsHollowMeshBank.StewBowlChunksHalf;
                if (bowl != null)
                {
                    // Stew disc is authored in bowl-local space; use the settled bowl transform.
                    PlaceAtTransform(bowlStew, "SerpentStew", bowl.transform, Vector3.zero, Vector3.zero, Vector3.one, "bowl_stew", snap: false);
                    PlaceAtTransform(bowlChunks, SerpentsHollowTexBank.MeatMaterial, bowl.transform, Vector3.zero, Vector3.zero, Vector3.one, "bowl_chunks", snap: false);
                }
            }
            session.Place(SerpentsHollowMeshBank.BreadLid, breadMat, parent, SerpentsHollowLayout.Lid, "lid", "lid", allowTilt: true);

            // Board-sitters first (A, C, D), then stacks / leans (E on D, F on A, B on A).
            SerpentsHollowLayout.Pose[] wholePoses =
            {
                SerpentsHollowLayout.WholeA, SerpentsHollowLayout.WholeC, SerpentsHollowLayout.WholeD,
                SerpentsHollowLayout.WholeE, SerpentsHollowLayout.WholeF, SerpentsHollowLayout.WholeB
            };
            char[] wholeKeys = { 'A', 'C', 'D', 'E', 'F', 'B' };
            for (int i = 0; i < wholeKeys.Length; i++)
            {
                if (!ContainsPose(wholes, wholePoses[i]))
                {
                    continue;
                }
                session.Place(SerpentsHollowMeshBank.BreadRoll, breadMat, parent, wholePoses[i], "roll", SerpentsHollowSettle.WholeTag(wholeKeys[i]), allowTilt: true);
            }

            SerpentsHollowLayout.Pose[] slicePoses =
            {
                SerpentsHollowLayout.Slice1, SerpentsHollowLayout.Slice3, SerpentsHollowLayout.Slice4,
                SerpentsHollowLayout.Slice2, SerpentsHollowLayout.Slice6, SerpentsHollowLayout.Slice5
            };
            char[] sliceKeys = { '1', '3', '4', '2', '6', '5' };
            for (int i = 0; i < sliceKeys.Length; i++)
            {
                if (!ContainsPose(slices, slicePoses[i]))
                {
                    continue;
                }
                GameObject slice = session.Place(SerpentsHollowMeshBank.BreadHalf, breadMat, parent, slicePoses[i], "slice", SerpentsHollowSettle.SliceTag(sliceKeys[i]), allowTilt: true);
                if (slice != null)
                {
                    PlaceAtTransform(SerpentsHollowMeshBank.JamSmear, SerpentsHollowTexBank.JamSpreadMaterial, slice.transform, new Vector3(0f, SerpentsHollowLayout.SmearLift, 0f), Vector3.zero, Vector3.one, "smear", snap: false);
                }
            }
            if (ladle)
            {
                PlaceAtMatrix(SerpentsHollowMeshBank.LadleScoop, "FeastMountains", parent, potMatrix, "ladle_scoop");
                PlaceAtMatrix(SerpentsHollowMeshBank.LadleHandle, SerpentsHollowTexBank.LadleHandleMaterial, parent, potMatrix, "ladle_handle");
            }
        }


        private static bool ContainsPose(SerpentsHollowLayout.Pose[] list, SerpentsHollowLayout.Pose pose)
        {
            if (list == null)
            {
                return false;
            }
            for (int i = 0; i < list.Length; i++)
            {
                SerpentsHollowLayout.Pose p = list[i];
                if (p.Pos == pose.Pos && p.Euler == pose.Euler && Mathf.Abs(p.Scale - pose.Scale) < 1e-6f)
                {
                    return true;
                }
            }
            return false;
        }

        // Place the pot, ladle and inner floor. Returns the snapped local matrix so
        // stew surfaces can share the exact same pose (do not snap them on their own).
        private static Matrix4x4 PlacePotGroup(Transform parent, bool includeStew, Mesh stew)
        {
            GameObject pot = PlaceRaw(SerpentsHollowMeshBank.PotBody, "FeastMountains", parent, SerpentsHollowLayout.PotPos, SerpentsHollowLayout.PotEuler, SerpentsHollowLayout.PotScale, "pot_body", snap: true);
            Matrix4x4 m;
            if (pot != null)
            {
                m = Matrix4x4.TRS(pot.transform.localPosition, pot.transform.localRotation, pot.transform.localScale);
            }
            else
            {
                m = Matrix4x4.TRS(SerpentsHollowLayout.PotPos, Quaternion.Euler(SerpentsHollowLayout.PotEuler), SerpentsHollowLayout.PotScale);
            }
            PlaceAtMatrix(SerpentsHollowMeshBank.PotFloor, "FeastMountains", parent, m, "pot_floor");
            if (includeStew && stew != null)
            {
                PlaceAtMatrix(stew, "SerpentStew", parent, m, "pot_stew");
            }
            return m;
        }

        private static void PlaceDishJams(Transform parent)
        {
            SerpentsHollowLayout.Pose[] dishes = { SerpentsHollowLayout.DishA, SerpentsHollowLayout.DishB, SerpentsHollowLayout.DishC };
            int i = 0;
            foreach (SerpentsHollowLayout.Pose p in dishes)
            {
                // Absolute Y is the fill height; do not snap to the deck.
                PlaceRaw(SerpentsHollowMeshBank.JamDish, SerpentsHollowTexBank.JamSpreadMaterial, parent, p, "dish_jam_" + i, snap: false);
                i++;
            }
        }

        private static GameObject PlaceRaw(Mesh mesh, string materialPrefab, Transform parent, SerpentsHollowLayout.Pose pose, string name, bool snap)
        {
            return PlaceRaw(mesh, MaterialFrom(materialPrefab), parent, pose, name, snap);
        }

        private static GameObject PlaceRaw(Mesh mesh, Material mat, Transform parent, SerpentsHollowLayout.Pose pose, string name, bool snap)
        {
            // Bread is placed through SerpentsHollowSettle (vertex drop onto the
            // board or the pieces under it). Do not add pose.Lift on top of a snap:
            // that was the source of the floating rolls in 1.1.0.
            return PlaceRaw(mesh, mat, parent, pose.Pos, pose.Euler, Vector3.one * pose.Scale, name, snap);
        }

        private static GameObject PlaceRaw(Mesh mesh, string materialPrefab, Transform parent, Vector3 localPos, Vector3 euler, Vector3 scale, string name, bool snap)
        {
            return PlaceRaw(mesh, MaterialFrom(materialPrefab), parent, localPos, euler, scale, name, snap);
        }

        private static GameObject PlaceRaw(Mesh mesh, Material mat, Transform parent, Vector3 localPos, Vector3 euler, Vector3 scale, string name, bool snap)
        {
            if (mesh == null)
            {
                return null;
            }
            if (mat == null)
            {
                Plugin.Log.LogWarning("No material for " + name);
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
            if (snap)
            {
                SnapToDeck(go, SerpentsHollowLayout.DeckY);
            }
            return go;
        }

        private static GameObject Place(string prefabName, Transform parent, SerpentsHollowLayout.Pose pose, string name, bool snap)
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
            visual.name = name;
            visual.transform.SetParent(parent, false);
            visual.transform.localPosition = pose.Pos;
            visual.transform.localRotation = Quaternion.Euler(pose.Euler);
            visual.transform.localScale = Vector3.one * pose.Scale;
            if (snap)
            {
                SnapToDeck(visual, SerpentsHollowLayout.DeckY);
            }
            return visual;
        }

        private static GameObject PlaceAtMatrix(Mesh mesh, string materialPrefab, Transform parent, Matrix4x4 local, string name)
        {
            return PlaceAtMatrix(mesh, MaterialFrom(materialPrefab), parent, local, name);
        }

        private static GameObject PlaceAtMatrix(Mesh mesh, Material mat, Transform parent, Matrix4x4 local, string name)
        {
            Vector3 pos = local.GetColumn(3);
            Vector3 scale = new Vector3(local.GetColumn(0).magnitude, local.GetColumn(1).magnitude, local.GetColumn(2).magnitude);
            Quaternion rot = Quaternion.LookRotation(local.GetColumn(2), local.GetColumn(1));
            return PlaceRaw(mesh, mat, parent, pos, rot.eulerAngles, scale, name, snap: false);
        }

        private static GameObject PlaceAtTransform(Mesh mesh, string materialPrefab, Transform parent, Vector3 localPos, Vector3 euler, Vector3 scale, string name, bool snap)
        {
            return PlaceRaw(mesh, materialPrefab, parent, localPos, euler, scale, name, snap);
        }

        private static GameObject PlaceAtTransform(Mesh mesh, Material mat, Transform parent, Vector3 localPos, Vector3 euler, Vector3 scale, string name, bool snap)
        {
            return PlaceRaw(mesh, mat, parent, localPos, euler, scale, name, snap);
        }

        private static void TintRenderer(Transform t, Color color)
        {
            if (t == null)
            {
                return;
            }
            MeshRenderer mr = t.GetComponent<MeshRenderer>();
            if (mr == null || mr.sharedMaterial == null)
            {
                return;
            }
            Material mat = new Material(mr.sharedMaterial);
            if (mat.HasProperty("_Color"))
            {
                mat.color = color;
            }
            if (mat.HasProperty("_TintColor"))
            {
                mat.SetColor("_TintColor", color);
            }
            mr.material = mat;
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
                        any = true;
                    }
                }
            }
            if (any)
            {
                Vector3 p = go.transform.localPosition;
                p.y += deckLocalY - minY;
                go.transform.localPosition = p;
            }
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

        private static void SwapBoardMesh(GameObject board)
        {
            Mesh clean = SerpentsHollowMeshBank.PlainsBoardClean;
            if (board == null || clean == null)
            {
                return;
            }
            if (board.transform.root != null && !IsOurPrefab(board.transform.root.gameObject) && !IsOurPrefab(board))
            {
                Plugin.Log.LogError("SwapBoardMesh refused to touch " + board.transform.root.name);
                return;
            }
            int swapped = 0;
            foreach (MeshFilter f in board.GetComponentsInChildren<MeshFilter>(true))
            {
                if (f == null || f.sharedMesh == null)
                {
                    continue;
                }
                if (f.sharedMesh == clean)
                {
                    continue;
                }
                f.sharedMesh = clean;
                swapped++;
            }
            if (swapped > 0)
            {
                Plugin.Log.LogInfo("Swapped " + swapped + " board mesh(es) to the decoration-free FeastPlains picnic board.");
            }
        }

        private static void HideOriginalFood(GameObject piece, List<Renderer> keep)
        {
            if (!IsOurPrefab(piece))
            {
                return;
            }
            HashSet<Renderer> keepSet = new HashSet<Renderer>(keep);
            foreach (Renderer r in piece.GetComponentsInChildren<Renderer>(true))
            {
                if (r == null || keepSet.Contains(r))
                {
                    continue;
                }
                if (IsSteamObject(r.transform) || r is ParticleSystemRenderer)
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

        internal static Material MaterialFromPublic(string prefabName)
        {
            return MaterialFrom(prefabName);
        }

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
            Material src = mr != null ? mr.sharedMaterial : null;
            UnityEngine.Object.DestroyImmediate(tmp);
            if (src == null)
            {
                return null;
            }
            // Always clone. Caching and handing out a vanilla shared material
            // would leak any later texture or color change into other items.
            Material mat = new Material(src);
            mat.name = "SbbfMat_" + prefabName;
            MaterialCache[prefabName] = mat;
            return mat;
        }

        private static GameObject ExtractVisual(string prefabName)
        {
            GameObject proto;
            if (!VisualPrototypes.TryGetValue(prefabName, out proto) || proto == null)
            {
                GameObject src = FeastHelpers.FindPrefab(prefabName);
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

        private static GameObject CloneVanillaFoodSteam(Transform parent, Vector3 localPos, string name)
        {
            string[] sources =
            {
                "FeastSwamps", "FeastMountains", "FeastOceans", "FeastBlackforest",
                "FeastMeadows", "FeastPlains", "FeastMistlands", "FeastAshlands", "FeastDeepNorth"
            };
            foreach (string srcName in sources)
            {
                GameObject prefab = FeastHelpers.FindPrefab(srcName);
                if (prefab == null || IsOurPrefab(prefab))
                {
                    continue;
                }
                GameObject temp = UnityEngine.Object.Instantiate(prefab);
                temp.name = srcName + "_steamSrc";
                temp.SetActive(false);
                GameObject src = FindNamed(temp.transform, "fx_FoodSteam");
                GameObject clone = null;
                if (src != null)
                {
                    clone = UnityEngine.Object.Instantiate(src);
                    clone.name = name;
                    clone.transform.SetParent(parent, false);
                    clone.transform.localPosition = localPos;
                    clone.transform.localRotation = Quaternion.identity;
                    clone.transform.localScale = Vector3.one;
                }
                UnityEngine.Object.DestroyImmediate(temp);
                if (clone != null)
                {
                    Plugin.Log.LogInfo("Cloned " + name + " from a temporary copy of " + srcName);
                    return clone;
                }
            }
            return null;
        }

        internal static void ComposeItemDrop(GameObject material, GameObject piece)
        {
            if (material == null || piece == null)
            {
                return;
            }
            if (!IsOurPrefab(material) || !IsOurPrefab(piece))
            {
                Plugin.Log.LogError("ComposeItemDrop refused to touch " + material.name + " / " + piece.name + " (not our prefabs).");
                return;
            }
            if (FindNamed(material.transform, "SerpentBreadBowlDrop") != null)
            {
                return;
            }

            StripVanillaLeftovers(material);
            DisableLodGroups(material);

            bool wasActive = material.activeSelf;
            LogDropPrefab("before", material);

            Transform attach = material.transform.Find("attach");
            bool hadAttach = attach != null;
            bool attachWasActive = hadAttach && attach.gameObject.activeSelf;

            // Our dish uses the FeastPlains picnic board, so fit to that item visual.
            Bounds? fit = MeasureItemVisual("FeastPlains_Material");

            DisableDropSteam(material);
            HideDropMeshes(material);

            if (!hadAttach)
            {
                GameObject created = new GameObject("attach");
                created.transform.SetParent(material.transform, false);
                created.transform.localPosition = Vector3.zero;
                created.transform.localRotation = Quaternion.identity;
                created.transform.localScale = Vector3.one;
                created.SetActive(false);
                attach = created.transform;
                Plugin.Log.LogInfo("Created inactive attach child on " + material.name);
            }

            if (hadAttach && attachWasActive)
            {
                GameObject attachVis = BuildDropVisual(attach, piece, "SerpentBreadBowlDrop");
                FitDropVisual(attachVis.transform, attach, fit);
            }
            else
            {
                GameObject worldVis = BuildDropVisual(material.transform, piece, "SerpentBreadBowlDrop");
                FitDropVisual(worldVis.transform, material.transform, fit);
                if (attach != null)
                {
                    GameObject attachVis = BuildDropVisual(attach, piece, "SerpentBreadBowlDrop");
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

        private static GameObject BuildDropVisual(Transform parent, GameObject piece, string name)
        {
            GameObject root = new GameObject(name);
            root.transform.SetParent(parent, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            GameObject board = FindNamed(piece.transform, "board");
            GameObject potShared = FindNamed(piece.transform, "PotShared");
            GameObject foodHigh = FindNamed(piece.transform, "Food_High");
            if (board != null)
            {
                CopyRendererTree(board.transform, root.transform, piece.transform, "board");
            }
            if (potShared != null)
            {
                CopyRendererTree(potShared.transform, root.transform, piece.transform, "PotShared");
            }
            if (foodHigh != null)
            {
                CopyRendererTree(foodHigh.transform, root.transform, piece.transform, "Food_High");
            }
            return root;
        }

        private static void CopyRendererTree(Transform src, Transform destParent, Transform space, string name, bool worldRoot = true)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(destParent, false);
            if (worldRoot)
            {
                // First-level copy (board / pot / food root) is re-rooted under the
                // drop visual, so convert from piece space into that new parent.
                go.transform.localPosition = space.InverseTransformPoint(src.position);
                go.transform.localRotation = Quaternion.Inverse(space.rotation) * src.rotation;
                Vector3 srcScale = src.lossyScale;
                Vector3 spaceScale = space.lossyScale;
                go.transform.localScale = new Vector3(
                    SafeDiv(srcScale.x, spaceScale.x),
                    SafeDiv(srcScale.y, spaceScale.y),
                    SafeDiv(srcScale.z, spaceScale.z));
            }
            else
            {
                // Nested children (jam smear, bowl stew, chunks) keep the local
                // pose they already have under their parent. Using piece-space
                // positions here was double-transforming them and scattering
                // toppings around the dropped item.
                go.transform.localPosition = src.localPosition;
                go.transform.localRotation = src.localRotation;
                go.transform.localScale = src.localScale;
            }

            MeshFilter mf = src.GetComponent<MeshFilter>();
            MeshRenderer mr = src.GetComponent<MeshRenderer>();
            if (mf != null && mf.sharedMesh != null && mr != null && mr.enabled)
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
                CopyRendererTree(child, go.transform, space, child.name, worldRoot: false);
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
            // Our dish already uses the FeastPlains picnic board; don't shrink or grow it much.
            if (s > 1.15f)
            {
                s = 1.0f;
            }
            if (s < 0.85f)
            {
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

        private static Bounds? MeasureItemVisual(string prefabName)
        {
            GameObject go = FeastHelpers.FindPrefab(prefabName);
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
            if (!IsOurPrefab(material))
            {
                return;
            }
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
            if (!IsOurPrefab(material))
            {
                return;
            }
            foreach (ParticleSystem ps in material.GetComponentsInChildren<ParticleSystem>(true))
            {
                if (ps == null)
                {
                    continue;
                }
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                var emission = ps.emission;
                emission.enabled = false;
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

        private static bool IsOurPrefab(GameObject go)
        {
            if (go == null)
            {
                return false;
            }
            string n = go.name;
            return n.IndexOf("FeastSerpentBreadBowl", StringComparison.Ordinal) >= 0;
        }

        private static void DisableLodGroups(GameObject go)
        {
            if (go == null || !IsOurPrefab(go))
            {
                return;
            }
            foreach (LODGroup lod in go.GetComponentsInChildren<LODGroup>(true))
            {
                if (lod != null)
                {
                    lod.enabled = false;
                }
            }
        }

        private static void StripVanillaLeftovers(GameObject go)
        {
            if (go == null || !IsOurPrefab(go))
            {
                return;
            }
            string[] strip = { "full", "half", "empty", "pie", "korv", "meat", "Sphere", "Sphere (5)",
                "fork", "knife", "spoon", "gaffel", "cutlery", "utensil", "forke", "gabel" };
            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            {
                if (t == null || t == go.transform)
                {
                    continue;
                }
                // Never strip our own composed food, the picnic board, or steam.
                string path = t.name;
                if (path.IndexOf("SerpentBreadBowl", StringComparison.Ordinal) >= 0
                    || path == "board" || path == "PotShared" || path == "Food_High"
                    || path == "Food_Mid" || path == "Food_Low" || IsSteamObject(t))
                {
                    continue;
                }
                bool match = false;
                foreach (string s in strip)
                {
                    if (string.Equals(t.name, s, StringComparison.OrdinalIgnoreCase)
                        || t.name.StartsWith(s + " ", StringComparison.OrdinalIgnoreCase)
                        || t.name.StartsWith(s + "(", StringComparison.OrdinalIgnoreCase))
                    {
                        match = true;
                        break;
                    }
                }
                if (!match)
                {
                    continue;
                }
                foreach (Renderer r in t.GetComponentsInChildren<Renderer>(true))
                {
                    if (r != null)
                    {
                        r.enabled = false;
                    }
                }
                t.gameObject.SetActive(false);
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
    }
}
