///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEditor;

namespace Nephasto
{
  namespace VintageAsset
  {
    /// <summary>
    /// Inspector base.
    /// </summary>
    public abstract class Inspector : Editor 
    {
      /// <summary>
      /// Indent level.
      /// </summary>
      public static int IndentLevel
      {
        get { return EditorGUI.indentLevel; }
        set { EditorGUI.indentLevel = value; }
      }

      /// <summary>
      /// Label width.
      /// </summary>
      public static float LabelWidth
      {
        get { return EditorGUIUtility.labelWidth; }
        set { EditorGUIUtility.labelWidth = value; }
      }

      /// <summary>
      /// Field width.
      /// </summary>
      public static float FieldWidth
      {
        get { return EditorGUIUtility.fieldWidth; }
        set { EditorGUIUtility.fieldWidth = value; }
      }

      /// <summary>
      /// GUI enabled?
      /// </summary>
      public static bool EnableGUI
      {
        get { return GUI.enabled; }
        set { GUI.enabled = value; }
      }

      /// <summary>
      /// GUI changed?
      /// </summary>
      public static bool Changed
      {
        get { return GUI.changed; }
        set { GUI.changed = value; }
      }

      public static GUIStyle HeaderStyle
      {
        get
        {
          if (headerStyle == null)
          {
            headerStyle = "ShurikenModuleTitle";
            headerStyle.font = new GUIStyle("Label").font;
            headerStyle.fontSize = 12;
            headerStyle.border = new RectOffset(15, 7, 4, 4);
            headerStyle.fixedHeight = 22;
            headerStyle.contentOffset = new Vector2(5.0f, -2.0f);
          }

          return headerStyle;
        }
      }

      public static GUIStyle FoldoutStyle
      {
        get
        {
          if (foldoutStyle == null)
          {
            foldoutStyle = "ShurikenModuleTitle";
            foldoutStyle.font = new GUIStyle("Label").font;
            foldoutStyle.fontSize = 12;
            foldoutStyle.border = new RectOffset(15, 7, 4, 4);
            foldoutStyle.fixedHeight = 22;
            foldoutStyle.contentOffset = new Vector2(20.0f, -2.0f);
          }

          return foldoutStyle;
        }
      }

      public static GUIStyle CheckboxStyle => checkboxStyle ?? (checkboxStyle = "ShurikenCheckMark");

      private string productID;
      private readonly Dictionary<string, bool> foldoutDisplay = new Dictionary<string, bool>();

      private PropertyInfo[] properties;

      private static GUIStyle headerStyle;
      private static GUIStyle foldoutStyle;
      private static GUIStyle checkboxStyle;
      
      protected virtual void OnEnable()
      {
        productID = GetType().ToString().Replace("Editor", string.Empty);
      }

      public override void OnInspectorGUI()
      {
        ResetGUI();
        
        serializedObject.Update();

        InspectorGUI();
        
        serializedObject.ApplyModifiedProperties();

        if (Changed == true)
          DirtyGUI();
      }

      protected abstract void InspectorGUI();
      
      /// <summary>
      /// Human-readable strings.
      /// </summary>
      public static string HumanizeName(string text)
      {
        return System.Text.RegularExpressions.Regex.Replace(text, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
      }
      
      /// <summary>
      /// Reset some GUI variables.
      /// </summary>
      public void ResetGUI(int indentLevel = 0, float labelWidth = 0.0f, float fieldWidth = 0.0f, bool guiEnabled = true)
      {
        EditorGUI.indentLevel = 0;
        EditorGUIUtility.labelWidth = 0.0f;
        EditorGUIUtility.fieldWidth = 0.0f;
        GUI.enabled = true;
      }

      /// <summary>
      /// Marks as dirty.
      /// </summary>
      public void DirtyGUI()
      {
        EditorUtility.SetDirty(target);
      }

      /// <summary>
      /// Marks object target as dirty.
      /// </summary>
      public void SetDirty(UnityEngine.Object obj)
      {
        EditorUtility.SetDirty(obj);
      }
      
      /// <summary>
      /// Nice foldout.
      /// </summary>
      public bool Foldout(string title)
      {
        bool display = GetFoldoutDisplay(title);

        Rect rect = GUILayoutUtility.GetRect(16.0f, 22.0f, FoldoutStyle);
        GUI.Box(rect, title, FoldoutStyle);

        Rect toggleRect = new Rect(rect.x + 4.0f, rect.y + 2.0f, 13.0f, 13.0f);
        if (Event.current.type == EventType.Repaint)
          EditorStyles.foldout.Draw(toggleRect, false, false, display, false);

        Event e = Event.current;
        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition) == true)
        {
          display = !display;
          e.Use();
        }

        SetFoldoutDisplay(title, display);

        return display;
      }

      /// <summary>
      /// Toogle foldout.
      /// </summary>
      public bool ToogleFoldout(string label, bool value, ref bool display)
      {
        display = GetFoldoutDisplay(label);
        
        Rect rect = GUILayoutUtility.GetRect(16.0f, 22.0f, FoldoutStyle);
        GUI.Box(rect, label, FoldoutStyle);

        Rect toggleRect = new Rect(rect.x + 4.0f, rect.y + 4.0f, 13.0f, 13.0f);
        if (Event.current.type == EventType.Repaint)
          CheckboxStyle.Draw(toggleRect, false, false, value, false);

        Event e = Event.current;
        if (e.type == EventType.MouseDown)
        {
          if (toggleRect.Contains(e.mousePosition) == true)
          {
            value = !value;
            e.Use();
            GUI.changed = true;
          }
          else if (rect.Contains(e.mousePosition) == true)
          {
            display = !display;
            e.Use();
            GUI.changed = true;
            
            SetFoldoutDisplay(label, display);
          }
        }
        
        return value;
      }

      /// <summary>
      /// Nice label.
      /// </summary>
      public static void Header(string label, string tooltip = default(string))
      {
        Rect rect = GUILayoutUtility.GetRect(16.0f, 22.0f, HeaderStyle);
        GUI.Box(rect, label, HeaderStyle);
      }
      
      /// <summary>
      /// Label.
      /// </summary>
      public static void Label(string label, string tooltip = default(string))
      {
        EditorGUILayout.LabelField(new GUIContent(label, tooltip));
      }
      
      /// <summary>
      /// Button.
      /// </summary>
      public static bool Button(string label, string tooltip = default(string), GUIStyle style = null)
      {
        return GUILayout.Button(new GUIContent(label, tooltip), style ?? GUI.skin.button);
      }
      
      /// <summary>
      /// Button with confirmation.
      /// </summary>
      public static bool ConfirmationButton(string buttonText, Color buttonColor, string dialogTitle, string dialogMessage)
      {
        bool confirmation = false;

        GUI.color = buttonColor;

        if (GUILayout.Button(buttonText) == true)
          confirmation = EditorUtility.DisplayDialog(dialogTitle, dialogMessage, "OK", "Cancel");

        GUI.color = UnityEngine.Color.white;

        return confirmation;
      }

      /// <summary>
      /// Space.
      /// </summary>
      public static void Space(float pixels)
      {
        GUILayout.Space(pixels);
      }
      
      /// <summary>
      /// Line separator.
      /// </summary>
      public static void Line()
      {
        EditorGUILayout.Separator();

        GUILayout.Box(string.Empty, GUILayout.ExpandWidth(true), GUILayout.Height(1.0f));
      }

      /// <summary>
      /// Separator.
      /// </summary>
      public static void Separator()
      {
        EditorGUILayout.Separator();
      }

      /// <summary>
      /// Begin vertical.
      /// </summary>
      public static void BeginVertical(string style = default(string))
      {
        if (string.IsNullOrEmpty(style) == true)
          GUILayout.BeginVertical();
        else
          GUILayout.BeginVertical(style);
      }

      /// <summary>
      /// End vertical.
      /// </summary>
      public static void EndVertical()
      {
        GUILayout.EndVertical();
      }

      /// <summary>
      /// Begin horizontal.
      /// </summary>
      public static void BeginHorizontal(string style = default(string))
      {
        if (string.IsNullOrEmpty(style) == true)
          GUILayout.BeginHorizontal();
        else
          GUILayout.BeginHorizontal(style);
      }

      /// <summary>
      /// End horizontal.
      /// </summary>
      public static void EndHorizontal()
      {
        GUILayout.EndHorizontal();
      }

      /// <summary>
      /// Flexible space.
      /// </summary>
      public static void FlexibleSpace()
      {
        GUILayout.FlexibleSpace();
      }

      /// <summary>
      /// Toggle with reset.
      /// </summary>
      public static bool Toggle(string label, string tooltip, bool value, bool resetValue)
      {
        EditorGUILayout.BeginHorizontal();
        {
          value = EditorGUILayout.Toggle(new GUIContent(label, tooltip), value);

          if (ResetButton(resetValue) == true)
            value = resetValue;
        }
        EditorGUILayout.EndHorizontal();

        return value;
      }

      /// <summary>
      /// Toggle.
      /// </summary>
      public static bool Toggle(string label, string tooltip, bool value)
      {
        return EditorGUILayout.Toggle(new GUIContent(label, tooltip), value);
      }

      /// <summary>
      /// Toggle with reset.
      /// </summary>
      public static bool Toggle(string label, bool value, bool resetValue)
      {
        return Toggle(label, string.Empty, value, resetValue);
      }
      
      /// <summary>
      /// Enum popup with reset.
      /// </summary>
      public static Enum EnumPopupField(string label, string tooltip, Enum selected, Enum resetValue)
      {
        EditorGUILayout.BeginHorizontal();
        {
          selected = EditorGUILayout.EnumPopup(new GUIContent(label, tooltip), selected);

          if (ResetButton(resetValue) == true)
            selected = resetValue;
        }
        EditorGUILayout.EndHorizontal();

        return selected;
      }

      /// <summary>
      /// Enum popup with reset.
      /// </summary>
      public static Enum EnumPopupField(string label, Enum selected, Enum resetValue)
      {
        return EnumPopupField(label, string.Empty, selected, resetValue);
      }
      
      /// <summary>
      /// Slider with reset.
      /// </summary>
      public static float SliderField(string label, string tooltip, float value, float minValue, float maxValue, float resetValue)
      {
        EditorGUILayout.BeginHorizontal();
        {
          value = EditorGUILayout.Slider(new GUIContent(label, tooltip), value, minValue, maxValue);

          if (ResetButton(resetValue) == true)
            value = resetValue;
        }
        EditorGUILayout.EndHorizontal();

        return value;
      }

      /// <summary>
      /// Slider with reset.
      /// </summary>
      public static float SliderField(string label, float value, float minValue, float maxValue, float resetValue)
      {
        return SliderField(label, string.Empty, value, minValue, maxValue, resetValue);
      }
      
      /// <summary>
      /// Float field with reset.
      /// </summary>
      public static float FloatField(string label, string tooltip, float value, float resetValue)
      {
        EditorGUILayout.BeginHorizontal();
        {
          value = EditorGUILayout.FloatField(new GUIContent(label, tooltip), value);

          if (ResetButton(resetValue) == true)
            value = resetValue;
        }
        EditorGUILayout.EndHorizontal();

        return value;
      }

      /// <summary>
      /// Float field with reset.
      /// </summary>
      public static float FloatField(string label, float value, float resetValue)
      {
        return FloatField(label, string.Empty, value, resetValue);
      }

      /// <summary>
      /// Int field with reset.
      /// </summary>
      public static int SliderField(string label, string tooltip, int value, int minValue, int maxValue, int resetValue)
      {
        EditorGUILayout.BeginHorizontal();
        {
          value = EditorGUILayout.IntSlider(new GUIContent(label, tooltip), value, minValue, maxValue);

          if (ResetButton(resetValue) == true)
            value = resetValue;
        }
        EditorGUILayout.EndHorizontal();

        return value;
      }

      /// <summary>
      /// Int field with reset.
      /// </summary>
      public static int SliderField(string label, int value, int minValue, int maxValue, int resetValue)
      {
        return SliderField(label, string.Empty, value, minValue, maxValue, resetValue);
      }

      /// <summary>
      /// Int field with reset.
      /// </summary>
      public static int IntField(string label, string tooltip, int value, int resetValue)
      {
        EditorGUILayout.BeginHorizontal();
        {
          value = EditorGUILayout.IntField(new GUIContent(label, tooltip), value);

          if (ResetButton(resetValue) == true)
            value = resetValue;
        }
        EditorGUILayout.EndHorizontal();

        return value;
      }

      /// <summary>
      /// Int field with reset.
      /// </summary>
      public static int IntField(string label, int value, int resetValue)
      {
        return IntField(label, string.Empty, value, resetValue);
      }

      /// <summary>
      /// Int popup field with reset.
      /// </summary>
      public static int IntPopup(string label, int value, string[] names, int[] values, int resetValue)
      {
        EditorGUILayout.BeginHorizontal();
        {
          value = EditorGUILayout.IntPopup(label, value, names, values);

          if (ResetButton(resetValue) == true)
            value = resetValue;
        }
        EditorGUILayout.EndHorizontal();

        return value;
      }

      /// <summary>
      /// Min-max slider with reset.
      /// </summary>
      public static void MinMaxSliderField(string label, string tooltip, ref float minValue, ref float maxValue, float minLimit, float maxLimit, float defaultMinLimit, float defaultMaxLimit)
      {
        EditorGUILayout.BeginHorizontal();
        {
          EditorGUILayout.MinMaxSlider(new GUIContent(label, tooltip), ref minValue, ref maxValue, minLimit, maxLimit);

          if (GUILayout.Button("R", GUILayout.Width(18.0f), GUILayout.Height(14.0f)) == true)
          {
            minValue = defaultMinLimit;
            maxValue = defaultMaxLimit;
          }
        }
        EditorGUILayout.EndHorizontal();
      }

      /// <summary>
      /// Min-max slider with reset.
      /// </summary>
      public static void MinMaxSliderField(string label, ref float minValue, ref float maxValue, float minLimit, float maxLimit, float defaultMinLimit, float defaultMaxLimit)
      {
        MinMaxSliderField(label, string.Empty, ref minValue, ref maxValue, minLimit, maxLimit, defaultMinLimit, defaultMaxLimit);
      }

      /// <summary>
      /// Color field with reset.
      /// </summary>
      public static Color ColorField(string label, string tooltip, Color value, Color resetValue)
      {
        EditorGUILayout.BeginHorizontal();
        {
          value = EditorGUILayout.ColorField(new GUIContent(label, tooltip), value);

          if (ResetButton(resetValue) == true)
            value = resetValue;
        }
        EditorGUILayout.EndHorizontal();

        return value;
      }

      /// <summary>
      /// Color field with reset.
      /// </summary>
      public static Color ColorField(string label, Color value, Color resetValue)
      {
        return ColorField(label, string.Empty, value, resetValue);
      }

      /// <summary>
      /// Animation curve.
      /// </summary>
      public static AnimationCurve CurveField(string label, string tooltip, AnimationCurve value)
      {
        EditorGUILayout.BeginHorizontal();
        {
          value = EditorGUILayout.CurveField(new GUIContent(label, tooltip), value);

          if (ResetButton() == true)
            value = new AnimationCurve(new Keyframe(1.0f, 0.0f, 0.0f, 0.0f), new Keyframe(0.0f, 1.0f, 0.0f, 0.0f));
        }
        EditorGUILayout.EndHorizontal();

        return value;
      }

      /// <summary>
      /// Animation curve.
      /// </summary>
      public static AnimationCurve CurveField(string label, AnimationCurve curve)
      {
        return CurveField(label, string.Empty, curve);
      }

      /// <summary>
      /// Vector2 field with reset.
      /// </summary>
      public static Vector2 Vector2Field(string label, string tooltip, Vector2 value, Vector2 resetValue)
      {
        EditorGUILayout.BeginHorizontal();
        {
          value = EditorGUILayout.Vector2Field(new GUIContent(label, tooltip), value);

          if (ResetButton(resetValue) == true)
            value = resetValue;
        }
        EditorGUILayout.EndHorizontal();

        return value;
      }

      /// <summary>
      /// Vector2 field with reset.
      /// </summary>
      public static Vector2 Vector2Field(string label, Vector2 value, Vector2 resetValue)
      {
        return Vector2Field(label, string.Empty, value, resetValue);
      }

      /// <summary>
      /// Vector3 field with reset.
      /// </summary>
      public static Vector3 Vector3Field(string label, string tooltip, Vector3 value, Vector3 resetValue)
      {
        EditorGUILayout.BeginHorizontal();
        {
          value = EditorGUILayout.Vector3Field(new GUIContent(label, tooltip), value);

          if (ResetButton(resetValue) == true)
            value = resetValue;
        }
        EditorGUILayout.EndHorizontal();

        return value;
      }

      /// <summary>
      /// Vector3 field with reset.
      /// </summary>
      public static Vector3 Vector3Field(string label, Vector3 value, Vector3 resetValue)
      {
        return Vector3Field(label, string.Empty, value, resetValue);
      }

      /// <summary>
      /// Layermask field with reset.
      /// </summary>
      public static LayerMask LayerMaskField(string label, LayerMask layerMask, int resetValue)
      {
        List<string> layers = new List<string>();
        List<int> layerNumbers = new List<int>();

        for (int i = 0; i < 32; ++i)
        {
          string layerName = UnityEngine.LayerMask.LayerToName(i);
          if (string.IsNullOrEmpty(layerName) == false)
          {
            layers.Add(layerName);
            layerNumbers.Add(i);
          }
        }

        int maskWithoutEmpty = 0;
        for (int i = 0; i < layerNumbers.Count; ++i)
        {
          if (((1 << layerNumbers[i]) & layerMask.value) > 0)
            maskWithoutEmpty |= (1 << i);
        }

        EditorGUILayout.BeginHorizontal();
        {
          maskWithoutEmpty = EditorGUILayout.MaskField(label, maskWithoutEmpty, layers.ToArray());
          int mask = 0;
          for (int i = 0; i < layerNumbers.Count; ++i)
          {
            if ((maskWithoutEmpty & (1 << i)) > 0)
              mask |= (1 << layerNumbers[i]);
          }

          layerMask.value = mask;

          if (ResetButton(resetValue) == true)
            layerMask.value = resetValue;
        }
        EditorGUILayout.EndHorizontal();

        return layerMask;
      }

      /// <summary>
      /// Reset button. 
      /// </summary>
      public static bool ResetButton<T>(T resetValue)
      {
        return GUILayout.Button(new GUIContent("R", $"Reset to '{resetValue}'."), GUILayout.Width(18.0f), GUILayout.Height(14.0f));
      }

      /// <summary>
      /// Reset button.
      /// </summary>
      public static bool ResetButton()
      {
        return GUILayout.Button("R", GUILayout.Width(18.0f), GUILayout.Height(14.0f));
      }

      protected T GetProperty<T>(string propertyName)
      {
        if (properties == null)
          properties = target.GetType().GetProperties();

        for (int i = 0; i < properties.Length; ++i)
        {
          if (properties[i].Name.Equals(propertyName) == true)
            return (T)properties[i].GetValue(target, null);
        }

        return default(T);
      }
      
      private bool GetFoldoutDisplay(string foldoutName)
      {
        string key = $"{productID}.display{foldoutName}";
        bool value = true;

        if (foldoutDisplay.ContainsKey(key) == false)
        {
          value = PlayerPrefs.GetInt(key, 0) == 1;
          
          foldoutDisplay.Add(key, value);
        }
        else
          value = foldoutDisplay[key];

        return value;
      }

      private void SetFoldoutDisplay(string foldoutName, bool value)
      {
        string key = $"{productID}.display{foldoutName}";
        
        if (foldoutDisplay.ContainsKey(key) == false)
          foldoutDisplay.Add(key, value);
        else
          foldoutDisplay[key] = value;
        
        PlayerPrefs.SetInt(key, value == true ? 1 : 0);
      }
    }
  }  
}
