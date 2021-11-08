using UnityEngine;
using UnityEditor;

namespace charcolle.MissingComponentFinder
{

    public class MissingComponentFinderEditorWindow : EditorWindow
    {

        public static MissingComponentFinderEditorWindow window;
        private static bool isInitialized = false;

        [SerializeField] private MissingComponentFinderGUI guiDrawer;

        private readonly static string WIN_TITLE = "MissingComponentFinder";
        private readonly static Vector2 WIN_SIZE = new Vector2( 150, 200 );

        [MenuItem( "Window/MissingComponentFinder" )]
        internal static void Open()
        {
            window = GetWindow<MissingComponentFinderEditorWindow>();
            window.titleContent = new GUIContent( WIN_TITLE, EditorGUIUtility.Load( "SceneAsset Icon" ) as Texture2D );
            window.minSize = WIN_SIZE;
            isInitialized = false;
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void OnFocus()
        {
            Initialize();
        }

        private void Initialize()
        {
            if ( isInitialized )
                return;

            guiDrawer = new MissingComponentFinderGUI();
            isInitialized = true;
        }

        private void OnGUI()
        {
            if ( !isInitialized )
                Initialize();

            guiDrawer.OnGUI();
        }

    }

}