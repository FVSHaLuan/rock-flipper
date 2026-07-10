using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI.Tips
{
    [CreateAssetMenu(menuName = "BSB/SingleInstance/GameTipManager")]
    public class GameTipManager : ScriptableObjectWithInit
    {
        private const string TipFolder = "Assets/_AMainGame/Prefabs/GameTips/Final";

        [SerializeField]
        private List<GameTip> gameTips;

        public GameTip GetRandomGameTip()
        {
            return gameTips.GetRandomItem();
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_GetAllTips")]
        private void Editor_GetAllTips()
        {
            ///
            UnityEditor.Undo.RecordObject(this, "Editor_GetAllTips");

            ///
            gameTips = new List<GameTip>();

            ///
            EditorHelper.GetAllObjetsFromPath(TipFolder, gameTips);

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }

}