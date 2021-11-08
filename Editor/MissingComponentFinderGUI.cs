using UnityEngine;
using UnityEditor;

namespace charcolle.MissingComponentFinder
{
    internal class MissingComponentFinderGUI
    {

        private MissingComponentFinder missingReferenceFinder;
        private GUIStyle NoSpaceBox;
        private Texture2D sceneTexture;

        internal MissingComponentFinderGUI()
        {
            missingReferenceFinder = new MissingComponentFinder();
            missingReferenceFinder.FindMissingComponent();
            sceneTexture = EditorGUIUtility.Load( "SceneAsset Icon" ) as Texture2D;
        }

        public void OnGUI()
        {
            if( NoSpaceBox == null )
            {

                NoSpaceBox = new GUIStyle( GUI.skin.box )
                {
                    margin = new RectOffset( 0, 0, 0, 0 ),
                    padding = new RectOffset( 1, 1, 1, 1 )
                };
            }

            EditorGUILayout.BeginVertical();
            {
                MissingComponentGUI();
                FooterGUI();
            }
            EditorGUILayout.EndVertical();
        }

        private void MissingComponentGUI()
        {
            EditorGUILayout.BeginVertical( NoSpaceBox );
            {
                if( !missingReferenceFinder.IsFindComponents() )
                {
                    GUILayout.Label( "Press Find Missing Components Button." );
                } else
                {
                    foreach( var keyValue in missingReferenceFinder.activeSceneObjectsDic )
                    {
                        var sceneName = keyValue.Key;
                        var objs = keyValue.Value;
                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.BeginHorizontal( NoSpaceBox );
                            GUILayout.Label( new GUIContent( sceneName, sceneTexture ), GUILayout.Height( 20f ) );
                            EditorGUILayout.EndHorizontal();

                            if ( objs.Count == 0 )
                            {

                                EditorGUILayout.BeginHorizontal();
                                {
                                    GUILayout.Space( 40f );
                                    GUILayout.Label( "Nothing!" );
                                }
                                EditorGUILayout.EndHorizontal();
                            } else
                            {
                                for ( int i = 0; i < objs.Count; i++ )
                                {
                                    if( objs[ i ] == null ) // obj will be null if the scene has been unloaded.
                                    {
                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            GUILayout.Space( 40f );
                                            GUILayout.Label( "The scene has been unloaded." );
                                        }
                                        EditorGUILayout.EndHorizontal();
                                    } else
                                    {
                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            GUILayout.Space( 40f );
                                            GUILayout.Label( i + 1 + ": " + objs[ i ].name );
                                            GUILayout.FlexibleSpace();
                                            if ( GUILayout.Button( "select", GUILayout.Width( 50f ) ) )
                                            {
                                                Selection.activeGameObject = objs[ i ];
                                            }
                                        }
                                        EditorGUILayout.EndHorizontal();
                                    }
                                }
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndVertical();
        }

        private void FooterGUI()
        {

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if ( missingReferenceFinder.IsFindComponents() )
                {
                    GUI.backgroundColor = Color.red;
                    if ( GUILayout.Button( "Delete Missing Components", GUILayout.Width( 200f ), GUILayout.Height( 25f ) ) )
                    {
                        missingReferenceFinder.DeleteMissingComponents();
                        missingReferenceFinder.FindMissingComponent();
                    }
                }
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndHorizontal();
        }

    }
}