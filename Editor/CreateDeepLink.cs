using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace TotemEditor
{
    public class CreateDeepLink : EditorWindow
    {
        Button generateButton;
        TextField gameId;


        [MenuItem("Window/Totem Generator/Generate Deep Link")]
        public static void ShowExample()
        {
            CreateDeepLink wnd = GetWindow<CreateDeepLink>();
            wnd.titleContent = new GUIContent("Generate Deep Link");
        }

        public void OnEnable()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.totem.totemcore/Editor/CreateDeepLink.uxml");
            if (visualTree == null)
            {
                visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Plugins/TotemGeneratorUnity/Editor/CreateDeepLink.uxml");
            }
            VisualElement labelFromUXML = visualTree.CloneTree();
            root.Add(labelFromUXML);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.totem.totemcore/Editor/CreateDeepLink.uss");
            if (styleSheet == null)
            {
                styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/TotemGeneratorUnity/Editor/CreateDeepLink.uss");
            }
            root.styleSheets.Add(styleSheet);

            generateButton = root.Q<Button>("generateButton");
            generateButton.clicked += OnGenerateButtonClick;

            gameId = root.Q<TextField>("gameId");
        }

        private void OnGenerateButtonClick()
        {
            if (!System.IO.Directory.Exists("Assets/Resources/"))
                System.IO.Directory.CreateDirectory("Assets/Resources/");
            System.IO.File.WriteAllText("Assets/Resources/webauth.txt", "torusapp://com.torus.Web3AuthUnity/auth_" + gameId.text);

            EditorUtility.DisplayDialog("Deep link generated", "Deep link for game id \"" + gameId.text + "\" is successfully generated", "Ok");
        }
    }
}