using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GlymorsFeasts
{
    internal static class V120Composer
    {
        internal static void Compose(GameObject piece, Feast feast, string layoutId, bool enableSteam)
        {
            if (piece == null || feast == null)
            {
                return;
            }
            V120DishLayout v120DishLayout = V120LayoutBank.Get(layoutId);
            if (v120DishLayout == null)
            {
                Plugin.Log.LogError(("No V120 layout for " + layoutId));
                return;
            }
            StripVanilla(piece);
            DisableLod(piece);
            GameObject val = FindNamed(piece.transform, "fx_FoodSteam");
            GameObject val2 = FindNamed(piece.transform, "board");
            GameObject val3 = FindNamed(piece.transform, "full");
            GameObject val4 = FindNamed(piece.transform, "New");
            HideOriginalFood(piece);
            Transform val5 = ((val4 != null) ? val4.transform : piece.transform);
            Transform transform = new GameObject("GlymorV120_" + layoutId).transform;
            transform.SetParent(val5, false);
            Transform val6 = ((val2 != null) ? val2.transform : ((val3 != null) ? val3.transform : null));
            if (val6 != null && val6.parent == val5)
            {
                transform.localPosition = val6.localPosition;
                transform.localRotation = val6.localRotation;
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localPosition = new Vector3(0f, -0.069f, 0f);
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
            }
            if (val2 != null)
            {
                Renderer[] componentsInChildren = val2.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer val7 in componentsInChildren)
                {
                    if (val7 != null && !(val7 is ParticleSystemRenderer))
                    {
                        val7.enabled = false;
                    }
                }
            }
            Transform transform2 = new GameObject("Vessel").transform;
            transform2.SetParent(transform, false);
            PlaceList(transform2, v120DishLayout.Stage0, vesselsOnly: false);
            PlaceMissingVessels(transform2, v120DishLayout.Stage10, v120DishLayout.Stage0);
            GameObject val8 = new GameObject("Food_High");
            GameObject val9 = new GameObject("Food_Mid");
            GameObject val10 = new GameObject("Food_Low");
            GameObject[] array = (GameObject[])(object)new GameObject[3] { val8, val9, val10 };
            for (int i = 0; i < array.Length; i++)
            {
                array[i].transform.SetParent(transform, false);
            }
            PlaceList(val8.transform, v120DishLayout.Stage10, vesselsOnly: false, skipVessels: true);
            PlaceList(val9.transform, v120DishLayout.Stage6, vesselsOnly: false, skipVessels: true);
            PlaceList(val10.transform, v120DishLayout.Stage3, vesselsOnly: false, skipVessels: true);
            List<Feast.FeastLevel> list = new List<Feast.FeastLevel>
            {
                new Feast.FeastLevel
                {
                    m_onAboveEquals = val8,
                    m_onBelow = val9,
                    m_threshold = 0.7f,
                    m_thresholdBelowMax = 0.4f
                },
                new Feast.FeastLevel
                {
                    m_onBelow = val10,
                    m_threshold = 0.4f,
                    m_thresholdBelowMax = 0.1f
                }
            };
            if (enableSteam)
            {
                Vector3 val11 = transform.localPosition + new Vector3(0f, 0.22f, 0f);
                if (val != null)
                {
                    val.transform.SetParent(val5, true);
                    val.transform.localPosition = val11;
                    val.transform.localRotation = Quaternion.identity;
                    ((Object)val).name = "fx_FoodSteam";
                    val.SetActive(true);
                }
                else
                {
                    val = CloneFoodSteam(val5, val11, "fx_FoodSteam");
                }
                if (val != null)
                {
                    list.Add(new Feast.FeastLevel
                    {
                        m_onAboveEquals = val,
                        m_threshold = 0.1f,
                        m_thresholdBelowMax = 0f
                    });
                }
            }
            else if (val != null)
            {
                val.SetActive(false);
            }
            feast.m_feastParts = list;
            feast.UpdateVisual();
            EnsureCollider(piece, v120DishLayout);
            Plugin.Log.LogInfo(("Composed V120 visuals for " + layoutId));
        }

        internal static void ComposeItemDrop(GameObject material, GameObject piece, string layoutId)
        {
            if (material == null || piece == null || FindNamed(material.transform, "GlymorV120Drop") != null)
            {
                return;
            }
            StripVanilla(material);
            DisableLod(material);
            bool activeSelf = material.activeSelf;
            Transform val = material.transform.Find("attach");
            bool num = val != null;
            bool flag = num && ((Component)val).gameObject.activeSelf;
            Bounds? target = GrizzledPlatterVisuals.MeasureItemVisual("FeastPlains_Material");
            HideDropMeshes(material);
            DisableDropSteam(material);
            if (!num)
            {
                GameObject val2 = new GameObject("attach");
                val2.transform.SetParent(material.transform, false);
                val2.transform.localPosition = Vector3.zero;
                val2.transform.localRotation = Quaternion.identity;
                val2.transform.localScale = Vector3.one;
                val2.SetActive(false);
                val = val2.transform;
            }
            if (num && flag)
            {
                GrizzledPlatterVisuals.FitDropVisual(BuildDropVisual(val, piece, "GlymorV120Drop").transform, val, target);
            }
            else
            {
                GrizzledPlatterVisuals.FitDropVisual(BuildDropVisual(material.transform, piece, "GlymorV120Drop").transform, material.transform, target);
                if (val != null)
                {
                    GrizzledPlatterVisuals.FitDropVisual(BuildDropVisual(val, piece, "GlymorV120Drop").transform, val, target);
                    if (!flag)
                    {
                        ((Component)val).gameObject.SetActive(false);
                    }
                }
            }
            if (material.activeSelf != activeSelf)
            {
                material.SetActive(activeSelf);
            }
        }

        private static void PlaceMissingVessels(Transform parent, V120Inst[] stage10, V120Inst[] stage0)
        {
            HashSet<string> hashSet = new HashSet<string>();
            V120Inst[] array;
            if (stage0 != null)
            {
                array = stage0;
                foreach (V120Inst v120Inst in array)
                {
                    hashSet.Add(v120Inst.Name ?? "");
                }
            }
            if (stage10 == null)
            {
                return;
            }
            array = stage10;
            foreach (V120Inst v120Inst2 in array)
            {
                if (v120Inst2.Vessel && !hashSet.Contains(v120Inst2.Name ?? ""))
                {
                    PlaceOne(parent, v120Inst2);
                }
            }
        }

        private static void PlaceList(Transform parent, V120Inst[] list, bool vesselsOnly, bool skipVessels = false)
        {
            if (list == null)
            {
                return;
            }
            foreach (V120Inst v120Inst in list)
            {
                if ((!vesselsOnly || v120Inst.Vessel) && (!skipVessels || !v120Inst.Vessel))
                {
                    PlaceOne(parent, v120Inst);
                }
            }
        }

        private static void PlaceOne(Transform parent, V120Inst inst)
        {
            Mesh val = V120LayoutBank.MeshAt(inst.Mesh);
            Material val2 = V120LayoutBank.MaterialAt(inst.Tex, inst.Gloss);
            if (!string.IsNullOrEmpty(inst.Name) && inst.Name.IndexOf("chicken", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Material val3 = LiftChickenMaterial(inst.Tex, inst.Gloss);
                if (val3 != null)
                {
                    val2 = val3;
                }
            }
            else if (!string.IsNullOrEmpty(inst.Name) && inst.Name.IndexOf("onion", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                WarmTint(val2, new Color(1.28f, 0.8f, 0.36f, 1f), 0.08f);
            }
            if (!(val == null) && !(val2 == null))
            {
                Matrix4x4 val4 = FromColumnMajor(inst.Matrix);
                Vector3 localPosition = (Vector4)(val4.GetColumn(3));
                Vector4 column = val4.GetColumn(0);
                float magnitude = column.magnitude;
                column = val4.GetColumn(1);
                float magnitude2 = column.magnitude;
                column = val4.GetColumn(2);
                Vector3 localScale = default(Vector3);
                localScale = new Vector3(magnitude, magnitude2, column.magnitude);
                Vector3 val5 = (Vector4)(val4.GetColumn(2));
                Vector3 val6 = (Vector4)(val4.GetColumn(1));
                if (val5.sqrMagnitude < 1E-08f || val6.sqrMagnitude < 1E-08f)
                {
                    val5 = Vector3.forward;
                    val6 = Vector3.up;
                }
                Quaternion localRotation = Quaternion.LookRotation(val5, val6);
                GameObject val7 = new GameObject(string.IsNullOrEmpty(inst.Name) ? "part" : inst.Name);
                val7.transform.SetParent(parent, false);
                val7.transform.localPosition = localPosition;
                val7.transform.localRotation = localRotation;
                val7.transform.localScale = localScale;
                val7.AddComponent<MeshFilter>().sharedMesh = val;
                ((Renderer)val7.AddComponent<MeshRenderer>()).sharedMaterial = val2;
            }
        }

        private static Material LiftChickenMaterial(int texIndex, float gloss)
        {
            Material val = null;
            try
            {
                GameObject val2 = FeastHelpers.FindPrefab("HoneyGlazedChicken");
                if (val2 != null)
                {
                    Renderer[] componentsInChildren = val2.GetComponentsInChildren<Renderer>(true);
                    for (int i = 0; i < componentsInChildren.Length; i++)
                    {
                        if (componentsInChildren[i] != null && componentsInChildren[i].sharedMaterial != null)
                        {
                            val = componentsInChildren[i].sharedMaterial;
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
            if (val == null)
            {
                return V120LayoutBank.MaterialAt(texIndex, gloss);
            }
            Material val3 = new Material(val);
            ((Object)val3).name = "V120_chicken_lifted";
            try
            {
                Texture val4 = null;
                if (val3.HasProperty("_MainTex"))
                {
                    val4 = val3.GetTexture("_MainTex");
                }
                if (val4 == null && val3.HasProperty("_BaseMap"))
                {
                    val4 = val3.GetTexture("_BaseMap");
                }
                if (val4 != null)
                {
                    Texture2D val5 = V120LayoutBank.WithMips(val4, "V120_chicken_vanilla");
                    if (val5 != null)
                    {
                        if (val3.HasProperty("_MainTex"))
                        {
                            val3.SetTexture("_MainTex", (Texture)(object)val5);
                        }
                        if (val3.HasProperty("_BaseMap"))
                        {
                            val3.SetTexture("_BaseMap", (Texture)(object)val5);
                        }
                    }
                }
            }
            catch
            {
            }
            if (val3.HasProperty("_Color"))
            {
                val3.SetColor("_Color", new Color(1.25f, 1.12f, 0.96f, 1f));
            }
            if (val3.HasProperty("_Metallic"))
            {
                val3.SetFloat("_Metallic", 0f);
            }
            if (val3.HasProperty("_Glossiness"))
            {
                val3.SetFloat("_Glossiness", Mathf.Clamp01(gloss * 0.35f));
            }
            if (val3.HasProperty("_Smoothness"))
            {
                val3.SetFloat("_Smoothness", Mathf.Clamp01(gloss * 0.35f));
            }
            return val3;
        }

        private static void WarmTint(Material mat, Color tint, float metallic)
        {
            if (!(mat == null))
            {
                if (mat.HasProperty("_Color"))
                {
                    mat.SetColor("_Color", tint);
                }
                if (mat.HasProperty("_Metallic"))
                {
                    mat.SetFloat("_Metallic", metallic);
                }
            }
        }

        private static Matrix4x4 FromColumnMajor(float[] m)
        {
            Matrix4x4 result = default(Matrix4x4);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[j, i] = m[i * 4 + j];
                }
            }
            return result;
        }

        private static void EnsureCollider(GameObject piece, V120DishLayout layout)
        {
            Transform val = piece.transform.Find("collider");
            BoxCollider val2 = ((val != null) ? ((Component)val).GetComponent<BoxCollider>() : null);
            if (val2 == null)
            {
                GameObject val3 = (GameObject)((val != null) ? ((object)((Component)val).gameObject) : ((object)new GameObject("collider")));
                if (val == null)
                {
                    val3.transform.SetParent(piece.transform, false);
                }
                val2 = val3.GetComponent<BoxCollider>() ?? val3.AddComponent<BoxCollider>();
                val = val3.transform;
            }
            ((Component)val).gameObject.SetActive(true);
            val.localPosition = Vector3.zero;
            val.localRotation = Quaternion.identity;
            val.localScale = Vector3.one;
            ((Collider)val2).enabled = true;
            val2.center = layout.ColliderCenter;
            val2.size = layout.ColliderSize;
        }

        private static void StripVanilla(GameObject go)
        {
            if (go == null)
            {
                return;
            }
            List<GameObject> list = new List<GameObject>();
            Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
            foreach (Transform val in componentsInChildren)
            {
                if (!(val == null) && !(val == go.transform))
                {
                    string text = ((Object)val).name.ToLowerInvariant();
                    if (text.Contains("fx_itemsparkles") || text.Contains("fx_foodsteam_small") || text.Contains("equipoffset") || text.Contains("blast") || text == "sparkle")
                    {
                        list.Add(((Component)val).gameObject);
                    }
                }
            }
            foreach (GameObject item in list)
            {
                if (item != null)
                {
                    Object.DestroyImmediate(item);
                }
            }
        }

        private static void HideOriginalFood(GameObject piece)
        {
            Renderer[] componentsInChildren = piece.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer val in componentsInChildren)
            {
                if (!(val == null) && !(val is ParticleSystemRenderer) && !IsSteamObject(((Component)val).transform))
                {
                    val.enabled = false;
                }
            }
        }

        private static void HideDropMeshes(GameObject material)
        {
            Renderer[] componentsInChildren = material.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer val in componentsInChildren)
            {
                if (!(val == null) && !(val is ParticleSystemRenderer) && !IsSteamObject(((Component)val).transform))
                {
                    val.enabled = false;
                }
            }
        }

        private static void DisableDropSteam(GameObject go)
        {
            Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
            foreach (Transform val in componentsInChildren)
            {
                if (val != null && IsSteamObject(val))
                {
                    ((Component)val).gameObject.SetActive(false);
                }
            }
        }

        private static void DisableLod(GameObject go)
        {
            LODGroup[] componentsInChildren = go.GetComponentsInChildren<LODGroup>(true);
            foreach (LODGroup val in componentsInChildren)
            {
                if (val != null)
                {
                    val.enabled = false;
                }
            }
        }

        private static bool IsSteamObject(Transform t)
        {
            while (t != null)
            {
                if (((Object)t).name.ToLowerInvariant().Contains("fx_foodsteam"))
                {
                    return true;
                }
                t = t.parent;
            }
            return false;
        }

        internal static GameObject FindNamed(Transform root, string name)
        {
            if (root == null)
            {
                return null;
            }
            if (((Object)root).name == name)
            {
                return ((Component)root).gameObject;
            }
            Transform val = root.Find(name);
            if (val != null)
            {
                return ((Component)val).gameObject;
            }
            Transform[] componentsInChildren = ((Component)root).GetComponentsInChildren<Transform>(true);
            foreach (Transform val2 in componentsInChildren)
            {
                if (val2 != null && ((Object)val2).name == name)
                {
                    return ((Component)val2).gameObject;
                }
            }
            return null;
        }

        private static GameObject CloneFoodSteam(Transform parent, Vector3 localPos, string name)
        {
            GameObject val = FeastHelpers.FindPrefab("FeastPlains");
            if (val == null)
            {
                return null;
            }
            Transform val2 = null;
            Transform[] componentsInChildren = val.GetComponentsInChildren<Transform>(true);
            foreach (Transform val3 in componentsInChildren)
            {
                if (val3 != null && ((Object)val3).name.ToLowerInvariant().Contains("fx_foodsteam"))
                {
                    val2 = val3;
                    break;
                }
            }
            if (val2 == null)
            {
                return null;
            }
            GameObject obj = Object.Instantiate<GameObject>(((Component)val2).gameObject);
            ((Object)obj).name = name;
            obj.transform.SetParent(parent, false);
            obj.transform.localPosition = localPos;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
            return obj;
        }

        private static GameObject BuildDropVisual(Transform parent, GameObject piece, string name)
        {
            GameObject val = new GameObject(name);
            val.transform.SetParent(parent, false);
            val.transform.localPosition = Vector3.zero;
            val.transform.localRotation = Quaternion.identity;
            val.transform.localScale = Vector3.one;
            GameObject val2 = null;
            Transform[] componentsInChildren = piece.GetComponentsInChildren<Transform>(true);
            foreach (Transform val3 in componentsInChildren)
            {
                if (val3 != null && ((Object)val3).name.StartsWith("GlymorV120_"))
                {
                    val2 = ((Component)val3).gameObject;
                    break;
                }
            }
            if (val2 != null)
            {
                CopyRendererTree(val2.transform, val.transform, piece.transform, ((Object)val2).name, worldRoot: true);
            }
            GameObject val4 = FindNamed(val.transform, "Food_Mid");
            if (val4 != null)
            {
                val4.SetActive(false);
            }
            val4 = FindNamed(val.transform, "Food_Low");
            if (val4 != null)
            {
                val4.SetActive(false);
            }
            return val;
        }

        private static void CopyRendererTree(Transform src, Transform destParent, Transform space, string name, bool worldRoot)
        {
            GameObject val = new GameObject(name);
            val.transform.SetParent(destParent, false);
            if (worldRoot)
            {
                val.transform.localPosition = space.InverseTransformPoint(src.position);
                val.transform.localRotation = Quaternion.Inverse(space.rotation) * src.rotation;
                Vector3 lossyScale = src.lossyScale;
                Vector3 lossyScale2 = space.lossyScale;
                val.transform.localScale = new Vector3(SafeDiv(lossyScale.x, lossyScale2.x), SafeDiv(lossyScale.y, lossyScale2.y), SafeDiv(lossyScale.z, lossyScale2.z));
            }
            else
            {
                val.transform.localPosition = src.localPosition;
                val.transform.localRotation = src.localRotation;
                val.transform.localScale = src.localScale;
            }
            MeshFilter component = ((Component)src).GetComponent<MeshFilter>();
            MeshRenderer component2 = ((Component)src).GetComponent<MeshRenderer>();
            if (component != null && component.sharedMesh != null && component2 != null)
            {
                val.AddComponent<MeshFilter>().sharedMesh = component.sharedMesh;
                MeshRenderer obj = val.AddComponent<MeshRenderer>();
                ((Renderer)obj).sharedMaterials = ((Renderer)component2).sharedMaterials;
                ((Renderer)obj).enabled = true;
            }
            foreach (Transform item in src)
            {
                Transform val2 = item;
                if (!(val2 == null) && !IsSteamObject(val2) && !(((Component)val2).GetComponent<ParticleSystem>() != null) && ((Component)val2).GetComponentsInChildren<MeshRenderer>(true).Length != 0)
                {
                    CopyRendererTree(val2, val.transform, space, ((Object)val2).name, worldRoot: false);
                }
            }
        }

        private static float SafeDiv(float a, float b)
        {
            if (!(Mathf.Abs(b) < 1E-05f))
            {
                return a / b;
            }
            return a;
        }

    }
}
