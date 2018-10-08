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
using UnityEditor;


namespace HoudiniEngineUnity
{

	public class HEU_ShelfToolsWindow : EditorWindow
	{
		private const float _windowWidth = 400;
		private const float _windowHeight = 600;

		private const int _toolGridXElements = 3;
		private const float _toolGridFixedCellWidth = 140;

		private const float _buttonWidth = 140;
		private const float _buttonHeight = 25;

		private static Color FolderColorOdd;
		private static Color FolderColorEven;

		private List<string> _toolsDirectories = new List<string>();

		private GUIContent[] _guiContents;

		private GUIContent _reloadButton = new GUIContent("Reload Tools", "Clear current tools and reload from tool folders.");
		private GUIContent _addButton = new GUIContent("Add Folder", "Add a new tools folder (containing json files) to load.");
		private GUIContent _clearButton = new GUIContent("Clear All", "Clear all folders and all tools.");

		private bool _folderFoldout;

		private Vector2 _toolButtonScrollPos = Vector2.zero;

		private GUIStyle _toolGridStyle;
		private GUIStyle _buttonStyle;

		private GUIStyle _folderStyle;

		private bool _initializedUI;

		


		public static void ShowWindow()
		{
			bool bUtility = false;
			bool bFocus = true;
			string title = "HEngine Tools";

			//Rect rect = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, _windowWidth, _windowHeight);
			//EditorWindow window = EditorWindow.GetWindowWithRect<HEU_HoudiniToolsWindow>(rect, bUtility, title, bFocus);

			HEU_ShelfToolsWindow window = EditorWindow.GetWindow<HEU_ShelfToolsWindow>(bUtility, title, bFocus);
			window.autoRepaintOnSceneChange = true;
		}

		private void InitializeUIElements()
		{
			_toolGridStyle = new GUIStyle(GUI.skin.button);
			_toolGridStyle.fixedWidth = _toolGridFixedCellWidth;
			_toolGridStyle.imagePosition = ImagePosition.ImageAbove;

			_buttonStyle = new GUIStyle(GUI.skin.button);

			_folderStyle = new GUIStyle()
			{
				normal = { background = Texture2D.whiteTexture }
			};

			FolderColorOdd = EditorGUIUtility.isProSkin ? new Color(0.18f, 0.18f, 0.18f) : new Color(0.5f, 0.5f, 0.5f);
			FolderColorEven = EditorGUIUtility.isProSkin ? new Color(0.15f, 0.15f, 0.15f) : new Color(0.55f, 0.55f, 0.55f);

			_initializedUI = true;
		}

		public void OnEnable()
		{
			_initializedUI = false;

			// Always reload the tools data when window is reopened
			// since the GUIContents are not kept around when closed.
			LoadJsonTools();
		}

		public void OnGUI()
		{
			if (!_initializedUI)
			{
				// Creating of UI elements must happen in OnGUI
				InitializeUIElements();
			}

			bool bReloadTools = false;
			int selectedIndex = -1;

			Color originalBGColor = GUI.backgroundColor;

			using (new EditorGUILayout.VerticalScope())
			{
				using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
				{
					if (HEU_ShelfTools._toolsLoaded)
					{
						int numTools = HEU_ShelfTools._tools.Count;

						using (EditorGUILayout.ScrollViewScope scroll = new EditorGUILayout.ScrollViewScope(_toolButtonScrollPos))
						{
							if (numTools > 0)
							{
								int numXElements = numTools < _toolGridXElements ? numTools : _toolGridXElements;

								selectedIndex = GUILayout.SelectionGrid(-1, _guiContents, numXElements, _toolGridStyle);
							}
							else
							{
								EditorGUILayout.LabelField("No tools found!");
							}

							_toolButtonScrollPos = scroll.scrollPosition;
						}
					}
				}

				HEU_EditorUI.DrawSeparator();

				using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
				{
					_folderFoldout = EditorGUILayout.Foldout(_folderFoldout, "TOOLS FOLDERS");
					if (_folderFoldout)
					{
						bool bChanged = false;

						HEU_EditorUI.DrawSeparator();

						float folderLineHeight = 24;

						// Draw folder list. If user changes, then re-populate.
						int numFolders = _toolsDirectories.Count;
						for (int i = 0; i < numFolders; ++i)
						{
							GUI.backgroundColor = i % 2 == 0 ? FolderColorEven : FolderColorOdd;
							using (new EditorGUILayout.HorizontalScope(_folderStyle, GUILayout.Height(folderLineHeight)))
							{
								GUIStyle buttonStyle = HEU_EditorUI.GetNewButtonStyle_MarginPadding(0, 0);
								buttonStyle.alignment = TextAnchor.MiddleCenter;

								GUI.backgroundColor = Color.red;
								if (GUILayout.Button("X", buttonStyle, GUILayout.Width(30), GUILayout.Height(folderLineHeight)))
								{
									_toolsDirectories.RemoveAt(i);
									bChanged = true;
									break;
								}

								GUI.backgroundColor = i % 2 == 0 ? FolderColorEven : FolderColorOdd;
								GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
								labelStyle.alignment = TextAnchor.MiddleLeft;
								EditorGUILayout.LabelField(_toolsDirectories[i], labelStyle, GUILayout.Height(folderLineHeight));
							}

							HEU_EditorUI.DrawSeparator();
						}

						GUI.backgroundColor = originalBGColor;

						HEU_EditorUI.DrawSeparator();

						using (new EditorGUILayout.HorizontalScope())
						{
							if (GUILayout.Button(_reloadButton, _buttonStyle, GUILayout.MaxWidth(_buttonWidth), GUILayout.MaxHeight(_buttonHeight)))
							{
								bReloadTools = true;
							}

							if (GUILayout.Button(_addButton, _buttonStyle, GUILayout.MaxWidth(_buttonWidth), GUILayout.MaxHeight(_buttonHeight)))
							{
								string openFilePath = UnityEditor.EditorUtility.OpenFolderPanel("Select Tools Folder", "", "");
								if (!string.IsNullOrEmpty(openFilePath))
								{
									_toolsDirectories.Add(openFilePath);
									bChanged = true;
								}
							}
							if (GUILayout.Button(_clearButton, _buttonStyle, GUILayout.MaxWidth(_buttonWidth), GUILayout.MaxHeight(_buttonHeight)))
							{
								_toolsDirectories.Clear();
								bChanged = true;
							}
						}

						if (bChanged)
						{
							HEU_PluginSettings.HEngineToolsUserFolders = _toolsDirectories;
							bReloadTools = true;
						}

						GUI.backgroundColor = originalBGColor;
					}
				}

				if (bReloadTools)
				{
					// Change of UI should happend at end after all drawing
					LoadJsonTools();
				}
				else if(selectedIndex >= 0)
				{
					// User selected a tool
					if(selectedIndex < HEU_ShelfTools._tools.Count)
					{
						ProcessUserSelection(selectedIndex);
					}
				}
			}
		}

		private void LoadJsonTools()
		{
			HEU_ShelfTools.ClearTools();

			_toolsDirectories = HEU_PluginSettings.HEngineToolsUserFolders;

			int numDirs = _toolsDirectories.Count;
			for(int i = 0; i < numDirs; ++i)
			{
				HEU_ShelfTools.LoadToolsFromDirectory(_toolsDirectories[i]);
			}

			// Parse the name and icon textures
			int numTools = HEU_ShelfTools._tools.Count;

			_guiContents = new GUIContent[numTools];

			for (int i = 0; i < numTools; ++i)
			{
				_guiContents[i] = new GUIContent();
				_guiContents[i].text = HEU_ShelfTools._tools[i].name;
				_guiContents[i].image = HEU_GeneralUtility.LoadTextureFromFile(HEU_ShelfTools._tools[i].iconPath);
				_guiContents[i].tooltip = HEU_ShelfTools._tools[i].toolTip;
			}

			Debug.LogFormat("Loaded {0} tools!", numTools);

			HEU_ShelfTools._toolsLoaded = true;
		}

		private void ProcessUserSelection(int selectedIndex)
		{
			HEU_ShelfTools.ExecuteTool(selectedIndex);
		}
	}


}   // HoudiniEngineUnity