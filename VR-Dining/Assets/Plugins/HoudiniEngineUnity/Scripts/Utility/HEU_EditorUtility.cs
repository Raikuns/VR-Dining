﻿/*
* Copyright (c) <2018> Side Effects Software Inc.
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*
* 1. Redistributions of source code must retain the above copyright notice,
*    this list of conditions and the following disclaimer.
*
* 2. The name of Side Effects Software may not be used to endorse or
*    promote products derived from this software without specific prior
*    written permission.
*
* THIS SOFTWARE IS PROVIDED BY SIDE EFFECTS SOFTWARE "AS IS" AND ANY EXPRESS
* OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
* OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.  IN
* NO EVENT SHALL SIDE EFFECTS SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT,
* INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
* LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
* OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
* LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
* NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
* EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HoudiniEngineUnity
{
	/// <summary>
	/// Wrapper around Unity Editor functions.
	/// </summary>
	public static class HEU_EditorUtility
	{
		/// <summary>
		/// Helper to mark current scene dirty so that Unity's save system will save out any procedural changes.
		/// </summary>
		public static void MarkSceneDirty()
		{
#if UNITY_EDITOR
			if (Application.isEditor && !Application.isPlaying)
			{
				UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
			}
#endif
		}

		public static void SelectObject(GameObject gameObject)
		{
#if UNITY_EDITOR
			Selection.objects = new GameObject[] { gameObject };
#endif
		}

		public static void SelectObjects(GameObject[] gameObjects)
		{
#if UNITY_EDITOR
			Selection.objects = gameObjects;
#endif
		}

		public static Vector3 GetSelectedObjectsMeanPosition()
		{
			Vector3 meanPosition = Vector3.zero;
#if UNITY_EDITOR

			Transform[] selectedTransforms = Selection.GetTransforms(SelectionMode.Unfiltered);
			int numTransforms = selectedTransforms.Length;
			if (numTransforms > 0)
			{
				meanPosition = selectedTransforms[0].position;

				for (int i = 1; i < numTransforms; ++i)
				{
					meanPosition += selectedTransforms[i].position;
				}

				meanPosition /= (float)numTransforms;
			}
#endif
			return meanPosition;
		}

		public static Matrix4x4 GetSelectedObjectsMeanTransform()
		{
			Matrix4x4 meanTransformMatrix = Matrix4x4.identity;
#if UNITY_EDITOR

			Transform[] selectedTransforms = Selection.GetTransforms(SelectionMode.Unfiltered);
			int numTransforms = selectedTransforms.Length;
			if (numTransforms > 0)
			{
				meanTransformMatrix = selectedTransforms[0].localToWorldMatrix;

				for (int i = 1; i < numTransforms; ++i)
				{
					meanTransformMatrix *= selectedTransforms[i].localToWorldMatrix;
				}

				Vector3 position = HEU_HAPIUtility.GetPosition(ref meanTransformMatrix);
				position /= (float)numTransforms;
				HEU_HAPIUtility.SetMatrixPosition(ref meanTransformMatrix, ref position);
			}
#endif
			return meanTransformMatrix;
		}

		public static GameObject CreatePrefab(string path, GameObject go)
		{
#if UNITY_EDITOR
			return PrefabUtility.CreatePrefab(path, go);
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
			return null;
#endif
		}

		public static bool IsEditorPlaying()
		{
#if UNITY_EDITOR
			return EditorApplication.isPlaying;
#else
			return false;
#endif
		}

		public enum HEU_ReplacePrefabOptions
		{
			// Replaces prefabs by matching pre-existing connections to the prefab.
			Default = 0,

			// Connects the passed objects to the prefab after uploading the prefab.
			ConnectToPrefab = 1,

			// Replaces the prefab using name based lookup in the transform hierarchy.
			ReplaceNameBased = 2
		}

		public static GameObject ReplacePrefab(GameObject go, Object targetPrefab, HEU_ReplacePrefabOptions heuOptions)
		{
#if UNITY_EDITOR
			ReplacePrefabOptions unityOptions = ReplacePrefabOptions.Default;
			switch(heuOptions)
			{
				case HEU_ReplacePrefabOptions.Default:			unityOptions = ReplacePrefabOptions.Default; break;
				case HEU_ReplacePrefabOptions.ConnectToPrefab:	unityOptions = ReplacePrefabOptions.ConnectToPrefab; break;
				case HEU_ReplacePrefabOptions.ReplaceNameBased: unityOptions = ReplacePrefabOptions.ReplaceNameBased; break;
				default: Debug.LogFormat("Unsupported replace prefab option: {0}", heuOptions); break;
			}

			return PrefabUtility.ReplacePrefab(go, targetPrefab, unityOptions);
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
			return null;
#endif
		}

		/// <summary>
		/// Returns true if given GameObject is an instance of a prefab.
		/// </summary>
		/// <param name="go">GameObject to check</param>
		/// <returns>True if given GameObject is an instance of a prefab</returns>
		public static bool IsPrefabInstance(GameObject go)
		{
#if UNITY_EDITOR
#if UNITY_2018_2_OR_NEWER
			return PrefabUtility.GetCorrespondingObjectFromSource(go) != null;
#else
			return PrefabUtility.GetPrefabParent(go) != null && PrefabUtility.GetPrefabObject(go) != null;
#endif
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
			return false;
#endif
		}

		/// <summary>
		/// Returns true if given GameObject is a prefab (and not an instance of a prefab).
		/// </summary>
		/// <param name="go">GameObject to check</param>
		/// <returns>True if given GameObject is a prefab</returns>
		public static bool IsPrefabOriginal(GameObject go)
		{
#if UNITY_EDITOR
#if UNITY_2018_2_OR_NEWER
			return PrefabUtility.GetPrefabType(go) == PrefabType.Prefab;
#else
			return PrefabUtility.GetPrefabParent(go) == null && PrefabUtility.GetPrefabObject(go) != null;
#endif
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
			return false;
#endif
		}

		/// <summary>
		/// Returns true if given GameObject is a disconnected instance of a prefab.
		/// </summary>
		/// <param name="go">GameObject to check</param>
		/// <returns>True if given GameObject is a disconnected instance of a prefab</returns>
		public static bool IsDisconnectedPrefabInstance(GameObject go)
		{
#if UNITY_EDITOR
#if UNITY_2018_2_OR_NEWER
			return PrefabUtility.GetPrefabType(go) == PrefabType.DisconnectedPrefabInstance;
#else
			return PrefabUtility.GetPrefabParent(go) != null && PrefabUtility.GetPrefabObject(go) == null;
#endif
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
			return false;
#endif
		}

		public static Object GetPrefabParent(GameObject go)
		{
#if UNITY_EDITOR
#if UNITY_2018_2_OR_NEWER
			return PrefabUtility.GetCorrespondingObjectFromSource(go);
#else
			return PrefabUtility.GetPrefabParent(go);
#endif
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
			return null;
#endif
		}

		public static GameObject ConnectGameObjectToPrefab(GameObject go, GameObject sourcePrefab)
		{
#if UNITY_EDITOR && UNITY_5_3_OR_NEWER
			return PrefabUtility.ConnectGameObjectToPrefab(go, sourcePrefab);
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
			return null;
#endif
		}

		public static Object InstantiatePrefab(GameObject prefabOriginal)
		{
#if UNITY_EDITOR
			return PrefabUtility.InstantiatePrefab(prefabOriginal);
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
			return null;
#endif
		}

		public static GameObject InstantiateGameObject(GameObject sourceGameObject,  Transform parentTransform, bool instantiateInWorldSpace, bool bRegisterUndo)
		{
			GameObject newGO = null;
#if UNITY_5_4_OR_NEWER
			newGO = GameObject.Instantiate(sourceGameObject, parentTransform, instantiateInWorldSpace);
#else
			newGO = GameObject.Instantiate(sourceGameObject);
			newGO.transform.parent = parentTransform;
#endif

#if UNITY_EDITOR
			if (bRegisterUndo)
			{
				Undo.RegisterCreatedObjectUndo(newGO, "Instantiated " + newGO.name);
			}
#endif
			return newGO;
		}

		public static Component AddComponent<T>(GameObject target, bool bRegisterUndo)
		{
#if UNITY_EDITOR
			if (bRegisterUndo)
			{
				return Undo.AddComponent(target, typeof(T));
			}
			else
#endif
			{
				return target.AddComponent(typeof(T));
			}
		}

		public static void UndoRecordObject(Object objectToUndo, string name)
		{
#if UNITY_EDITOR
			Undo.RecordObject(objectToUndo, name);
#endif
		}

		public static void UndoCollapseCurrentGroup()
		{
#if UNITY_EDITOR
			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
#endif
		}

		/// <summary>
		/// Calculates and returns a list of all assets that obj depends on.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Object[] CollectDependencies(Object obj)
		{
#if UNITY_EDITOR
			return EditorUtility.CollectDependencies(new Object[] { obj });
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
			return null;
#endif
		}

		/// <summary>
		/// Returns true if obj is stored on disk
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool IsPersistant(UnityEngine.Object obj)
		{
#if UNITY_EDITOR
			return EditorUtility.IsPersistent(obj);
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
			return false;
#endif
		}

		/// <summary>
		/// Returns unique name based on siblings. If given name is already found
		/// adds integer to end and increments until found unique.
		/// </summary>
		/// <param name="parentTransform">Target parent for a new GameObject. Null means root level</param>
		/// <param name="name">Requested name for a new GameObject</param>
		/// <returns>Unique name for sibling gameobject</returns>
		public static string GetUniqueNameForSibling(Transform parentTransform, string name)
		{
#if UNITY_EDITOR
			return GameObjectUtility.GetUniqueNameForSibling(parentTransform, name);
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
			return null;
#endif
		}

		/// <summary>
		/// Displays or updates a progress bar.
		/// </summary>
		/// <param name="title">Title of display</param>
		/// <param name="info">Info on display</param>
		/// <param name="progress">Progress ratio from 0 to 1</param>
		/// <returns></returns>
		public static void DisplayProgressBar(string title, string info, float progress)
		{
#if UNITY_EDITOR
			EditorUtility.DisplayProgressBar(title, info, progress);
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
#endif
		}

		/// <summary>
		/// Removes the progress bar on display
		/// </summary>
		public static void ClearProgressBar()
		{
#if UNITY_EDITOR
			EditorUtility.ClearProgressBar();
#else
			Debug.LogWarning(HEU_Defines.HEU_USERMSG_NONEDITOR_NOT_SUPPORTED);
#endif
		}

		/// <summary>
		/// Returns true if we are in Editor, and we are not in play mode nor going into play mode.
		/// </summary>
		public static bool IsEditorNotInPlayModeAndNotGoingToPlayMode()
		{
#if UNITY_EDITOR
			return Application.isEditor && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && (Time.timeSinceLevelLoad > 0 || !Application.isPlaying);
#else
			return Application.isEditor && !Application.isPlaying;
#endif
		}

		/// <summary>
		/// Display message boxes in the editor.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="message"></param>
		/// <param name="ok"></param>
		/// <param name="cancel"></param>
		/// <returns>True if OK button is pressed.</returns>
		public static bool DisplayDialog(string title, string message, string ok, string cancel = "")
		{
#if UNITY_EDITOR
			return EditorUtility.DisplayDialog(title, message, ok, cancel);
#else
			Debug.Log(string.Format("{0}: {1}", title, message));
			return true;
#endif
		}

		/// <summary>
		/// Display error message boxes in the editor.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="message"></param>
		/// <param name="ok"></param>
		/// <param name="cancel"></param>
		/// <returns>True if OK button is pressed.</returns>
		public static bool DisplayErrorDialog(string title, string message, string ok, string cancel = "")
		{
#if UNITY_EDITOR
			return EditorUtility.DisplayDialog(string.Format("{0}: {1}", HEU_Defines.HEU_ERROR_TITLE, title), message, ok, cancel);
#else
			Debug.Log(string.Format("{0}: {1} - {2}", HEU_Defines.HEU_ERROR_TITLE, title, message));
			return true;
#endif
		}

#if UNITY_EDITOR
		public static SerializedProperty GetSerializedProperty(SerializedObject serializedObject, string propertyName)
		{
			SerializedProperty foundProperty = serializedObject.FindProperty(propertyName);
			if(foundProperty == null)
			{
				string errorMsg = string.Format("Property {0} not found on Object {1}. Make sure class {1} has member variable {0}.", propertyName, serializedObject.targetObject.GetType().ToString());
				Debug.LogAssertion(errorMsg);
			}
			return foundProperty;
		}
#endif

#if UNITY_EDITOR
		public static bool EditorDrawSerializedProperty(SerializedObject serializedObject, string propertyName, string label = null, string tooltip = null, bool bIncludeChildren = true)
		{
			SerializedProperty serializedProperty = GetSerializedProperty(serializedObject, propertyName);
			if(serializedProperty != null)
			{
				if(label == null)
				{
					EditorGUILayout.PropertyField(serializedProperty, bIncludeChildren);
				}
				else if(label.Length == 0)
				{
					EditorGUILayout.PropertyField(serializedProperty, GUIContent.none, bIncludeChildren);
				}
				else
				{
					EditorGUILayout.PropertyField(serializedProperty, new GUIContent(label, tooltip), bIncludeChildren);
				}
				return true;
			}
			return false;
		}

		public static bool EditorDrawFloatProperty(SerializedObject serializedObject, string propertyName, string label = null, string tooltip = null)
		{
			SerializedProperty serializedProperty = GetSerializedProperty(serializedObject, propertyName);
			if (serializedProperty != null)
			{
				if (label == null)
				{
					EditorGUILayout.DelayedFloatField(serializedProperty);
				}
				else if (label.Length == 0)
				{
					EditorGUILayout.DelayedFloatField(serializedProperty, GUIContent.none);
				}
				else
				{
					EditorGUILayout.DelayedFloatField(serializedProperty, new GUIContent(label, tooltip));
				}
				return true;
			}
			return false;
		}

		public static bool EditorDrawIntProperty(SerializedObject serializedObject, string propertyName, string label = null, string tooltip = null)
		{
			SerializedProperty serializedProperty = GetSerializedProperty(serializedObject, propertyName);
			if (serializedProperty != null)
			{
				if (label == null)
				{
					EditorGUILayout.DelayedIntField(serializedProperty);
				}
				else if (label.Length == 0)
				{
					EditorGUILayout.DelayedIntField(serializedProperty, GUIContent.none);
				}
				else
				{
					EditorGUILayout.DelayedIntField(serializedProperty, new GUIContent(label, tooltip));
				}
				return true;
			}
			return false;
		}

		public static bool EditorDrawTextProperty(SerializedObject serializedObject, string propertyName, string label = null)
		{
			SerializedProperty serializedProperty = GetSerializedProperty(serializedObject, propertyName);
			if (serializedProperty != null)
			{
				if (label == null)
				{
					EditorGUILayout.DelayedTextField(serializedProperty);
				}
				else if (label.Length == 0)
				{
					EditorGUILayout.DelayedTextField(serializedProperty, GUIContent.none);
				}
				else
				{
					EditorGUILayout.DelayedTextField(serializedProperty, new GUIContent(label));
				}
				return true;
			}
			return false;
		}

		public static bool EditorDrawBoolProperty(SerializedObject serializedObject, string propertyName, string label = null, string tooltip = null)
		{
			SerializedProperty serializedProperty = GetSerializedProperty(serializedObject, propertyName);
			if (serializedProperty != null)
			{
				if (label == null)
				{
					EditorGUILayout.PropertyField(serializedProperty);
				}
				else if (label.Length == 0)
				{
					EditorGUILayout.PropertyField(serializedProperty, GUIContent.none);
				}
				else
				{
					EditorGUILayout.PropertyField(serializedProperty, new GUIContent(label, tooltip));
				}
				return true;
			}
			return false;
		}

		public static void EditorDrawIntArray(ref int[] intValues, string label = null)
		{
			// Arrays are drawn with a label, and rows of values.

			GUILayout.BeginHorizontal();
			{
				if(!string.IsNullOrEmpty(label))
				{
					EditorGUILayout.PrefixLabel(label);
				}

				GUILayout.BeginVertical(EditorStyles.helpBox);
				{
					int numElements = intValues.Length;
					int maxElementsPerRow = 4;

					GUILayout.BeginHorizontal();
					{
						for (int i = 0; i < numElements; ++i)
						{
							if (i > 0 && i % maxElementsPerRow == 0)
							{
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
							}

							intValues[i] = EditorGUILayout.DelayedIntField(intValues[i]);
						}
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}

		public static void EditorDrawFloatArray(ref float[] floatFields, string label = null)
		{
			// Arrays are drawn with a label, and rows of values.

			GUILayout.BeginHorizontal();
			{
				if (!string.IsNullOrEmpty(label))
				{
					EditorGUILayout.PrefixLabel(label);
				}

				GUILayout.BeginVertical(EditorStyles.helpBox);
				{
					int numElements = floatFields.Length;
					int maxElementsPerRow = 4;

					GUILayout.BeginHorizontal();
					{
						for (int i = 0; i < numElements; ++i)
						{
							if (i > 0 && i % maxElementsPerRow == 0)
							{
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
							}

							floatFields[i] = EditorGUILayout.DelayedFloatField(floatFields[i]);
						}
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}

		public static void EditorDrawTextArray(ref string[] stringFields, string label = null)
		{
			// Arrays are drawn with a label, and rows of values.

			GUILayout.BeginHorizontal();
			{
				if (!string.IsNullOrEmpty(label))
				{
					EditorGUILayout.PrefixLabel(label);
				}

				GUILayout.BeginVertical(EditorStyles.helpBox);
				{
					int numElements = stringFields.Length;
					int maxElementsPerRow = 4;

					GUILayout.BeginHorizontal();
					{
						for (int i = 0; i < numElements; ++i)
						{
							if (i > 0 && i % maxElementsPerRow == 0)
							{
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
							}

							stringFields[i] = EditorGUILayout.DelayedTextField(stringFields[i]);
						}
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}

		public static void EditorDrawArrayProperty(SerializedProperty arrayProperty, EditorDrawPropertyDelegate drawDelegate, string label = null)
		{
			// Arrays are drawn with a label, and rows of values.

			GUILayout.BeginHorizontal();
			{
				if (!string.IsNullOrEmpty(label))
				{
					EditorGUILayout.PrefixLabel(label);
				}

				GUILayout.BeginVertical(EditorStyles.helpBox);
				{
					int numElements = arrayProperty.arraySize;
					int maxElementsPerRow = 4;

					GUILayout.BeginHorizontal();
					{
						for (int i = 0; i < numElements; ++i)
						{
							if (i > 0 && i % maxElementsPerRow == 0)
							{
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal();
							}

							SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(i);
							drawDelegate(elementProperty);
						}
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}

		public delegate void EditorDrawPropertyDelegate(SerializedProperty property);

		public static void EditorDrawIntProperty(SerializedProperty property)
		{
			property.intValue = EditorGUILayout.DelayedIntField(property.intValue);
		}

		public static void EditorDrawFloatProperty(SerializedProperty property)
		{
			property.floatValue = EditorGUILayout.DelayedFloatField(property.floatValue);
		}

		public static void EditorDrawTextProperty(SerializedProperty property)
		{
			property.stringValue = EditorGUILayout.DelayedTextField(property.stringValue);
		}

		public static int[] GetSerializedPropertyArrayValuesInt(SerializedProperty property)
		{
			int[] array = null;
			if (property.isArray)
			{
				array = new int[property.arraySize];
				for (int i = 0; i < array.Length; ++i)
				{
					array[i] = property.GetArrayElementAtIndex(i).intValue;
				}
			}
			return array;
		}

		public static float[] GetSerializedPropertyArrayValuesFloat(SerializedProperty property)
		{
			float[] array = null;
			if (property.isArray)
			{
				array = new float[property.arraySize];
				for (int i = 0; i < array.Length; ++i)
				{
					array[i] = property.GetArrayElementAtIndex(i).floatValue;
				}
			}
			return array;
		}

		public static string[] GetSerializedPropertyArrayValuesString(SerializedProperty property)
		{
			string[] array = null;
			if (property.isArray)
			{
				array = new string[property.arraySize];
				for (int i = 0; i < array.Length; ++i)
				{
					array[i] = property.GetArrayElementAtIndex(i).stringValue;
				}
			}
			return array;
		}
#endif

		/// <summary>
		/// Sets given object to require update in Unity.
		/// Used for forcing Update/LateUpdate to be called on objects in Editor.
		/// Only works in Editor currently.
		/// </summary>
		/// <param name="obj">Object to set for update</param>
		public static void SetObjectDirtyForEditorUpdate(Object obj)
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(obj);
#endif
		}

		/// <summary>
		/// Sets the given gameobject's static state.
		/// </summary>
		/// <param name="go">GameObject to set static state on</param>
		/// <param name="bStatic">Static state to set</param>
		public static void SetStatic(GameObject go, bool bStatic)
		{
#if UNITY_EDITOR
			go.isStatic = bStatic;
#endif
		}

		/// <summary>
		/// Returns array of currently selected gameobjects.
		/// </summary>
		/// <returns>Array of currently selected gameobjects</returns>
		public static GameObject[] GetSelectedObjects()
		{
#if UNITY_EDITOR
			return Selection.gameObjects;
#else
			return null;
#endif
		}

		/// <summary>
		/// Get list of user selected Houdini assets root components in the scene.
		/// </summary>
		/// <returns>List of selected Houdini asset root components (HEU_HoudiniAssetRoot)</returns>
		public static HEU_HoudiniAssetRoot[] GetSelectedAssetRoots()
		{
			List<HEU_HoudiniAssetRoot> rootAssets = new List<HEU_HoudiniAssetRoot>();
#if UNITY_EDITOR
			Object[] selectedObjects = Selection.objects;
			foreach (Object obj in selectedObjects)
			{
				GameObject go = obj as GameObject;
				if (go != null)
				{
					HEU_HoudiniAssetRoot root = go.GetComponent<HEU_HoudiniAssetRoot>();
					if (root != null)
					{
						rootAssets.Add(root);
					}
				}
			}
#endif
			return rootAssets.ToArray();
		}

		/// <summary>
		/// Get all Houdini asset root components in the scene.
		/// </summary>
		/// <returns>List of all Houdini asset root components in the scene (HEU_HoudiniAssetRoot)</returns>
		public static HEU_HoudiniAssetRoot[] GetAllAssetRoots()
		{
			return GameObject.FindObjectsOfType<HEU_HoudiniAssetRoot>();
		}

		/// <summary>
		/// Cook the user selected Houdini assets in the scene.
		/// </summary>
		public static void CookSelected()
		{
			CookAssets(GetSelectedAssetRoots());
		}

		/// <summary>
		/// Cook all Houdini assets in the scene.
		/// </summary>
		public static void CookAll()
		{
			CookAssets(GetAllAssetRoots());
		}

		/// <summary>
		/// Cook the given list of Houdini assets in the scene.
		/// </summary>
		/// <param name="rootAssets"></param>
		public static void CookAssets(HEU_HoudiniAssetRoot[] rootAssets)
		{
			if (rootAssets == null || rootAssets.Length == 0)
			{
				return;
			}

			foreach (HEU_HoudiniAssetRoot root in rootAssets)
			{
				if (root._houdiniAsset != null)
				{
					root._houdiniAsset.RequestCook(bCheckParametersChanged: true, bAsync: true, bSkipCookCheck: true, bUploadParameters: true);
				}
			}
		}

		/// <summary>
		/// Rebuild (reset, reload and cook) the user selected Houdini assets in the scene.
		/// </summary>
		public static void RebuildSelected()
		{
			RebuildAssets(GetSelectedAssetRoots());
		}

		/// <summary>
		/// Rebuild (reset, reload and cook) all Houdini assets in the scene.
		/// </summary>
		public static void RebuildAll()
		{
			RebuildAssets(GetAllAssetRoots());
		}

		/// <summary>
		/// Rebuild (reset, reload and cook) given list of Houdini assets in the scene.
		/// </summary>
		/// <param name="rootAssets"></param>
		public static void RebuildAssets(HEU_HoudiniAssetRoot[] rootAssets)
		{
			if (rootAssets == null || rootAssets.Length == 0)
			{
				return;
			}

			foreach (HEU_HoudiniAssetRoot root in rootAssets)
			{
				if (root._houdiniAsset != null)
				{
					root._houdiniAsset.RequestReload(true);
				}
			}
		}

		/// <summary>
		/// Bake out and replace with baked objects the user selected Houdini assets in the scene.
		/// </summary>
		public static void BakeAndReplaceSelectedInScene()
		{
			BakeAndReplaceAssets(GetSelectedAssetRoots());
		}

		/// <summary>
		/// Bake out and replace with baked objects all Houdini assets in the scene.
		/// </summary>
		public static void BakeAndReplaceAllInScene()
		{
			BakeAndReplaceAssets(GetAllAssetRoots());
		}

		/// <summary>
		/// Bake out and replace with baked object the given list of Houdini assets in the scene.
		/// </summary>
		/// <param name="rootAssets"></param>
		public static void BakeAndReplaceAssets(HEU_HoudiniAssetRoot[] rootAssets)
		{
			if (rootAssets == null || rootAssets.Length == 0)
			{
				return;
			}

			foreach (HEU_HoudiniAssetRoot root in rootAssets)
			{
				if(root._houdiniAsset != null)
				{
					root._houdiniAsset.RequestBakeInPlace();
				}
			}
		}
	}

}   // HoudiniEngineUnity
