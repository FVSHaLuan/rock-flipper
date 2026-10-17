using Agame.Balancing;
using UnityEngine;
using UnityEditor;

namespace Agame.Run
{
    // Shown through PopupWindow.Show, which closes it on its own when the user clicks outside.
    public class ChangeCashTierPopup : PopupWindowContent
    {
        private readonly SkillGraphNode skillGraphNode;
        private CashTier selectedCashTier;

        public ChangeCashTierPopup(SkillGraphNode skillGraphNode)
        {
            this.skillGraphNode = skillGraphNode;
            this.selectedCashTier = skillGraphNode.CashTier;
        }

        public override Vector2 GetWindowSize()
        {
            // Tier drop list, b preview, warning/error line, OK/Cancel
            var lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            return new Vector2(300, lineHeight * 4 + 12);
        }

        public override void OnGUI(Rect rect)
        {
            ///
            selectedCashTier = (CashTier)EditorGUILayout.EnumPopup("Cash Tier", selectedCashTier);

            ///
            var oldB = skillGraphNode.Editor_PriceB;
            var canChange = skillGraphNode.Editor_TryGetRescaledPriceB(selectedCashTier, out var newB, out var error);
            if (canChange)
            {
                EditorGUILayout.LabelField("b", $"{FormatB(oldB)} -> {FormatB(newB)}");

                if (newB < 1)
                {
                    EditorGUILayout.LabelField(EditorGUIUtility.TrTextContentWithIcon("New b is below 1", MessageType.Warning));
                }
            }
            else
            {
                EditorGUILayout.LabelField(EditorGUIUtility.TrTextContentWithIcon(error, MessageType.Error));
            }

            ///
            GUILayout.FlexibleSpace();
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledGroupScope(!canChange))
                {
                    if (GUILayout.Button("OK"))
                    {
                        OnOkClicked(oldB, newB);
                    }
                }

                if (GUILayout.Button("Cancel"))
                {
                    editorWindow.Close();
                }
            }
        }

        private void OnOkClicked(double oldB, double newB)
        {
            ///
            var node = skillGraphNode;
            var cashTier = selectedCashTier;

            ///
            if (newB >= 1 || cashTier == node.CashTier)
            {
                node.Editor_ChangeCashTier(cashTier);
                editorWindow.Close();
                return;
            }

            // The confirm dialog takes focus, which closes this popup anyway, so close it first
            // and show the dialog once the popup is gone.
            editorWindow.Close();
            EditorApplication.delayCall += () =>
            {
                var confirmed = EditorUtility.DisplayDialog(
                    "Change Cash Tier",
                    $"New b is below 1 ({FormatB(oldB)} -> {FormatB(newB)}).\nChange cash tier to {cashTier} anyway?",
                    "Change",
                    "Cancel");

                if (confirmed && node != null)
                {
                    node.Editor_ChangeCashTier(cashTier);
                }
            };
        }

        private static string FormatB(double b)
        {
            return b.ToString("G6");
        }
    }

}
