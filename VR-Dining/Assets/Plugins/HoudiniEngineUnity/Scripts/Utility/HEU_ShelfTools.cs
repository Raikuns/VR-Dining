/*
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace HoudiniEngineUnity
{
	/// <summary>
	/// Container of a tool's metadata
	/// </summary>
	[System.Serializable]
	public class HEU_ShelfToolData
	{
		public string name = "";

		public enum ToolType
		{
			GENARATOR,
			OPERATOR_SINGLE,
			OPERATOR_MULTI,
			BATCH
		}

		public ToolType toolType;

		public string toolTip = "";

		public string iconPath = "";

		public string assetPath = "";

		public string helpURL = "";

		public string[] target;
	}

	public class HEU_ShelfTools
	{
		public static List<HEU_ShelfToolData> _tools = new List<HEU_ShelfToolData>();

		public static bool _toolsLoaded = false;

		// Target values to check for compatibility with this plugin
		public const string TARGET_ALL = "all";
		public const string TARGET_UNITY = "unity";


#pragma warning disable 0649
		// Wrapper to enable us to write out arrays using JsonUtility.
		[System.Serializable]
		private class ShelfToolsWrapper
		{
			public HEU_ShelfToolData[] _tools;
		}
#pragma warning restore 0649

		/// <summary>
		/// Retrieve the array from given JSON string.
		/// </summary>
		/// <typeparam name="T">Type of array</typeparam>
		/// <param name="jsonArray">String containing JSON array.</param>
		/// <returns>Array of objects of type T.</returns>
		private static HEU_ShelfToolData[] GetJSONArray(string jsonArray)
		{
			// Parse out array string into array class, then just grab the array.
			ShelfToolsWrapper dataArray = JsonUtility.FromJson<ShelfToolsWrapper> (jsonArray);
			return dataArray._tools;
		}

		public static void ClearTools()
		{
			_tools.Clear();
		}

		public static bool LoadToolsFromDirectory(string folderPath)
		{
			string[] filePaths = HEU_Platform.GetFilesInFolder(folderPath, "*.json", true);
			bool bResult = false;
			try
			{
				if (filePaths != null)
				{
					foreach (string fileName in filePaths)
					{
						LoadToolFromJsonFile(fileName);
					}

					bResult = true;
				}
			}
			catch(System.Exception ex)
			{
				Debug.LogErrorFormat("Parsing JSON files in directory caused exception: {0}", ex);
				return false;
			}

			return bResult;
		}

		public static bool LoadToolFromJsonFile(string jsonPath)
		{
			string json = null;
			try
			{
				StreamReader fileReader = new StreamReader(jsonPath);
				json = fileReader.ReadToEnd();
				fileReader.Close();
			}
			catch(System.Exception ex)
			{
				Debug.LogErrorFormat("Exception while reading {0}: {1}", jsonPath, ex);
				return false;
			}

			return LoadToolFromJsonString(json);
		}

		public static bool LoadToolFromJsonString(string json)
		{
			// Get environment variable for tool path
			string envValue = HEU_Platform.GetEnvironmentValue(HEU_Defines.HEU_PATH_KEY_TOOL);
			string envKey = string.Format("<{0}>", HEU_Defines.HEU_PATH_KEY_TOOL);

			if (!string.IsNullOrEmpty(json))
			{
				HEU_ShelfToolData toolData = JsonUtility.FromJson<HEU_ShelfToolData>(json);

				if(toolData != null 
					&& !string.IsNullOrEmpty(toolData.name) 
					&& !string.IsNullOrEmpty(toolData.assetPath)
					&& !string.IsNullOrEmpty(toolData.iconPath))
				{
					// Make sure this tool targets Unity (must have "all" or "unity" set in target field)
					bool bCompatiple = false;
					if(toolData.target != null)
					{
						int numTargets = toolData.target.Length;
						for(int i = 0; i < numTargets; ++i)
						{
							if (toolData.target[i].Equals(TARGET_ALL))
							{
								bCompatiple = true;
								break;
							}
							else if(toolData.target[i].Equals(TARGET_UNITY))
							{
								bCompatiple = true;
								break;
							}
						}
					}

					if (bCompatiple)
					{
						toolData.assetPath = toolData.assetPath.Replace(HEU_Defines.HEU_PATH_KEY_PROJECT + "/", "");
						toolData.iconPath = toolData.iconPath.Replace(HEU_Defines.HEU_PATH_KEY_PROJECT + "/", "");

						if(toolData.assetPath.Contains(envKey))
						{
							if(string.IsNullOrEmpty(envValue))
							{
								Debug.LogErrorFormat("Environment value {0} used but not set in environment.", HEU_Defines.HEU_PATH_KEY_TOOL);
							}
							else
							{
								toolData.assetPath = toolData.assetPath.Replace(envKey, envValue);
							}
						}

						if (toolData.iconPath.Contains(envKey))
						{
							if (string.IsNullOrEmpty(envValue))
							{
								Debug.LogErrorFormat("Environment value {0} used but not set in environment.", HEU_Defines.HEU_PATH_KEY_TOOL);
							}
							else
							{
								toolData.iconPath = toolData.iconPath.Replace(envKey, envValue);
							}
						}

						_tools.Add(toolData);
						Debug.LogFormat("Added tool: {0}", toolData.name);
					}
				}

				return true;
			}

			return false;
		}

		public static void ExecuteTool(int toolSlot)
		{
			if(toolSlot < 0 || toolSlot >= _tools.Count)
			{
				Debug.LogWarning("Invalid tool selection!");
				return;
			}

			HEU_ShelfToolData toolData = _tools[toolSlot];
			if(toolData.toolType == HEU_ShelfToolData.ToolType.GENARATOR)
			{
				Matrix4x4 targetMatrix = HEU_EditorUtility.GetSelectedObjectsMeanTransform();
				Vector3 position = HEU_HAPIUtility.GetPosition(ref targetMatrix);
				Quaternion rotation = HEU_HAPIUtility.GetQuaternion(ref targetMatrix);
				Vector3 scale = HEU_HAPIUtility.GetScale(ref targetMatrix);
				scale = Vector3.one;

				ExecuteToolGenerator(toolData.name, toolData.assetPath, position, rotation, scale);
			}
			else if(toolData.toolType == HEU_ShelfToolData.ToolType.OPERATOR_SINGLE)
			{
				GameObject[] selectedObjects = HEU_EditorUtility.GetSelectedObjects();

				ExecuteToolOperatorSingle(toolData.name, toolData.assetPath, selectedObjects);
			}
			else if (toolData.toolType == HEU_ShelfToolData.ToolType.OPERATOR_MULTI)
			{
				GameObject[] selectedObjects = HEU_EditorUtility.GetSelectedObjects();

				ExecuteToolOperatorMultiple(toolData.name, toolData.assetPath, selectedObjects);
			}
			else if (toolData.toolType == HEU_ShelfToolData.ToolType.BATCH)
			{
				GameObject[] selectedObjects = HEU_EditorUtility.GetSelectedObjects();

				ExecuteToolBatch(toolData.name, toolData.assetPath, selectedObjects);
			}
		}

		public static void ExecuteToolGenerator(string toolName, string toolPath, Vector3 targetPosition, Quaternion targetRotation, Vector3 targetScale)
		{
			GameObject go = HEU_HAPIUtility.InstantiateHDA(toolPath, targetPosition, HEU_SessionManager.GetOrCreateDefaultSession(), true);
			if (go != null)
			{
				go.transform.rotation = targetRotation;
				go.transform.localScale = targetScale;

				HEU_EditorUtility.SelectObject(go);
			}
			else
			{
				Debug.LogWarningFormat("Failed to instantiate tool: {0}", toolName);
			}
		}

		public static void ExecuteToolOperatorSingle(string toolName, string toolPath, GameObject[] inputObjects)
		{
			GameObject go = HEU_HAPIUtility.InstantiateHDA(toolPath, Vector3.zero, HEU_SessionManager.GetOrCreateDefaultSession(), true);
			if (go != null)
			{
				HEU_HoudiniAssetRoot assetRoot = go.GetComponent<HEU_HoudiniAssetRoot>();
				if (assetRoot != null)
				{
					//assetRoot._houdiniAsset
					// TODO
				}

				HEU_EditorUtility.SelectObject(go);
			}
			else
			{
				Debug.LogWarningFormat("Failed to instantiate tool: {0}", toolName);
			}
		}

		public static void ExecuteToolOperatorMultiple(string toolName, string toolPath, GameObject[] inputObjects)
		{
			// TODO
		}

		public static void ExecuteToolBatch(string toolName, string toolPat, GameObject[] batchObjects)
		{
			// TODO
		}
	}

}   // HoudiniEngineUnity