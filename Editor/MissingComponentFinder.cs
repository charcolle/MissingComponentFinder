using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace charcolle.MissingComponentFinder
{
    internal class MissingComponentFinder
    {

        public Dictionary<string, List<GameObject>> activeSceneObjectsDic;

        internal MissingComponentFinder()
        {
            activeSceneObjectsDic = new Dictionary<string, List<GameObject>>();
        }

        internal bool IsFindComponents()
        {
            return activeSceneObjectsDic.Count > 0;
        }

        internal void FindMissingComponent()
        {
            var allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

            for( int i = 0; i < allObjects.Length; i++ )
            {
                var sceneName = allObjects[ i ].scene.name;
                if ( !activeSceneObjectsDic.ContainsKey( sceneName ) )
                    activeSceneObjectsDic.Add( sceneName, new List<GameObject>() );

                var components = allObjects[ i ].GetComponents<Component>();
                for( int j = 0; j < components.Length; j++ )
                {
                    if ( components[ j ] == null )
                    {
                        activeSceneObjectsDic[ sceneName ].Add( allObjects[ i ] );
                        break;
                    }
                }
            }
        }

        internal void DeleteMissingComponents()
        {
            var hasAnythingToDo = false;
            foreach( var keyVal in activeSceneObjectsDic )
            {
                var objs = keyVal.Value;
                for( int i = 0; i < objs.Count; i++ )
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript( objs[i] );
                    Debug.Log( "MissingComponentFinder: Removed missing component from " + objs[ i ].name );
                    hasAnythingToDo = true;
                }
            }
            if ( !hasAnythingToDo )
                Debug.Log( "MissingComponentFinder: Nothing to be removed!" );
            activeSceneObjectsDic.Clear();
        }

    }
}