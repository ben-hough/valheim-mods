using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GlymorsFeasts
{
    internal static class SeaGodsFavor
    {
        private static StatusEffect _se;

        internal static void Apply(ItemDrop item)
        {
            if (!(item == null) && item.m_itemData != null && item.m_itemData.m_shared != null)
            {
                StatusEffect val = Ensure();
                if (val != null)
                {
                    item.m_itemData.m_shared.m_consumeStatusEffect = val;
                }
            }
        }

        private static StatusEffect Ensure()
        {
            if (_se != null)
            {
                return _se;
            }
            StatusEffect val = null;
            GameObject obj = FeastHelpers.FindPrefab("MeadSwimmer");
            ItemDrop val2 = ((obj != null) ? obj.GetComponent<ItemDrop>() : null);
            if (val2 != null && val2.m_itemData != null && val2.m_itemData.m_shared != null)
            {
                val = val2.m_itemData.m_shared.m_consumeStatusEffect;
            }
            if (val == null && ObjectDB.instance != null)
            {
                val = ObjectDB.instance.GetStatusEffect(StringExtensionMethods.GetStableHashCode("Potion_swimmer"));
                if (val == null)
                {
                    val = ObjectDB.instance.GetStatusEffect(StringExtensionMethods.GetStableHashCode("SE_Potion_swimmer"));
                }
            }
            if (val == null)
            {
                Plugin.Log.LogWarning("Sea-God's Favor: could not find Potion_swimmer / MeadSwimmer status effect to clone.");
                return null;
            }
            _se = Object.Instantiate<StatusEffect>(val);
            ((Object)_se).name = "SE_SeaGodsFavor";
            _se.m_name = "$se_seagodsfavor";
            if (!string.IsNullOrEmpty(val.m_tooltip) || _se.m_tooltip != null)
            {
                try
                {
                    _se.m_tooltip = "$se_seagodsfavor_desc";
                }
                catch
                {
                }
            }
            try
            {
                Plugin.RegisterEnglishToken("se_seagodsfavor", "Sea-God's Favor");
                Plugin.RegisterEnglishToken("se_seagodsfavor_desc", "The sea asks less of you. Swim stamina use is reduced.");
            }
            catch
            {
            }
            _se.m_ttl = 300f;
            CopySwimCap(_se, val);
            if (ObjectDB.instance != null && ObjectDB.instance.m_StatusEffects != null && !_seExists(ObjectDB.instance.m_StatusEffects, _se))
            {
                ObjectDB.instance.m_StatusEffects.Add(_se);
            }
            Plugin.Log.LogInfo(("Cloned Sea-God's Favor from " + ((Object)val).name + " (ttl 300s)."));
            return _se;
        }

        private static bool _seExists(List<StatusEffect> list, StatusEffect se)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == se)
                {
                    return true;
                }
            }
            return false;
        }

        private static void CopySwimCap(StatusEffect dest, StatusEffect src)
        {
            FieldInfo[] fields = ((object)dest).GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fields)
            {
                if (fieldInfo.Name.ToLowerInvariant().Contains("speed") && (fieldInfo.FieldType == typeof(float) || fieldInfo.FieldType == typeof(int)))
                {
                    try
                    {
                        fieldInfo.SetValue(dest, Convert.ChangeType(0, fieldInfo.FieldType));
                    }
                    catch
                    {
                    }
                }
            }
            fields = ((object)dest).GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo2 in fields)
            {
                string text = fieldInfo2.Name.ToLowerInvariant();
                if (fieldInfo2.FieldType != typeof(float) || !text.Contains("swim") || (!text.Contains("stamina") && !text.Contains("use")))
                {
                    continue;
                }
                try
                {
                    float num = (float)fieldInfo2.GetValue(src);
                    float num2 = -0.5f;
                    if (num < 0f)
                    {
                        num2 = Mathf.Max(num, -0.5f);
                    }
                    fieldInfo2.SetValue(dest, num2);
                }
                catch
                {
                }
            }
        }
    }
}
