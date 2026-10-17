// Play mode only. One-shot snapshot of the run: what a player would see, plus the numbers behind it.
if (!UnityEditor.EditorApplication.isPlaying) return "not in Play mode";
var re = Agame.Run.RunEntry.Instance;
if (re == null) return "no RunEntry yet (scene still loading, or not the Run scene)";
var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public;
var sb = new System.Text.StringBuilder();
var entry = Entry.Instance;
var data = re.RunData;
var stats = re.BuildStats;
var tsm = entry.timeScaleManager;

sb.AppendLine("frame " + UnityEngine.Time.frameCount + ", realtime " + UnityEngine.Time.realtimeSinceStartup.ToString("0.0") + "s, gameplay time " + UnityEngine.Time.time.ToString("0.0") + "s");
sb.AppendLine("runState: " + re.runStateManager.CurrentState + ", timeScale: " + UnityEngine.Time.timeScale.ToString("0.##") + (tsm.IsGameplayBeingPaused ? " (PAUSED)" : ""));
sb.AppendLine("slot: " + data.SlotId + ", finishedTutorial: " + data.FinishedTutorial + ", baseBuildStats: " + re.BaseBuildStatsName + (re.IsUsingTestBuildStats ? " (TEST)" : "") + ", savable: " + entry.playerDataSaver.IsSavable);

// screens currently on the UI stack, bottom to top
var stack = (System.Collections.IList)typeof(UIScreenManager).GetField("screenStack", flags).GetValue(entry.uiScreenManager);
var names = new System.Collections.Generic.List<string>();
foreach (UIScreen s in stack) names.Add(s.name);
sb.AppendLine("screenStack: [" + string.Join(" > ", names) + "]");

// economy
sb.AppendLine("cash: " + data.GetCurrencyValue(Agame.Run.Currency.CASH).ToString("0.##") + ", stars: " + data.GetCurrencyValue(Agame.Run.Currency.STAR)
    + ", level " + data.Level + " (exp " + data.LevelExp.ToString("0.#") + "/" + entry.gameBalance.GetRequiredExpForNextLevel(data.Level) + ")"
    + ", chestLevel " + data.ChestLevel + " (exp " + data.ChestLevelExp.ToString("0.#") + "/" + Agame.Balancing.GameBalance.GetRequiredExpForNextChestLevel(data.ChestLevel) + ")");
sb.AppendLine("mouse: " + (stats.enabledMouseHover ? "hover" : "click") + (stats.enabledMouseRadius ? ", radius " + stats.mouseRadius : ", point") + "; skillTreeUnlocked: " + stats.UnlockedSkillTree + "; activeBackground: " + data.ActiveBackgroundId);

// playfield
var byTier = new System.Collections.Generic.SortedDictionary<string, int[]>();
foreach (var rock in re.rockInstanceManager.ActiveRocks)
{
    var key = rock.Tier.ToString();
    if (!byTier.TryGetValue(key, out var c)) byTier[key] = c = new int[3];
    c[0]++;
    if (rock.IsPure) c[1]++;
    if (rock.Flippable.IsFlipping) c[2]++;
}
var rockParts = new System.Collections.Generic.List<string>();
foreach (var kv in byTier) rockParts.Add(kv.Key + "=" + kv.Value[0] + " (pure " + kv.Value[1] + ", airborne " + kv.Value[2] + ")");
sb.AppendLine("rocks: " + (rockParts.Count == 0 ? "none" : string.Join(", ", rockParts)));
var chestParts = new System.Collections.Generic.List<string>();
foreach (var chest in re.chestInstanceManager.ActiveChests) chestParts.Add(chest.Rarity + " " + chest.CurrentHP + "/" + chest.MaxHP + "hp");
sb.AppendLine("chests: " + (chestParts.Count == 0 ? "none" : string.Join(", ", chestParts)));
sb.AppendLine("flipperBots: " + re.flipperBotInstanceManager.ActiveFlipperBotCount);

// shop buttons (Shop tab), whether or not the tab is showing
foreach (var b in UnityEngine.Object.FindObjectsByType<Agame.Run.Shop.StatBuilderButton>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None))
{
    var id = (string)typeof(Agame.Run.Shop.StatBuilderButton).GetField("buttonId", flags).GetValue(b);
    var button = (UnityEngine.UI.Button)typeof(Agame.Run.Shop.StatBuilderButton).GetField("button", flags).GetValue(b);
    var cost = typeof(Agame.Run.Shop.StatBuilderButton).GetField("costText", flags).GetValue(b) as UnifiedText;
    var costText = cost != null ? System.Text.RegularExpressions.Regex.Replace(cost.Text, "<sprite[^>]*>", "$") : "?";
    sb.AppendLine("  shop " + id + ": level " + data.GetBuilderButtonLevel(id) + ", cost " + costText + ", interactable " + button.interactable + (b.gameObject.activeInHierarchy ? "" : " (tab hidden)"));
}

// skill nodes the player can afford right now; "hidden" = not activated yet (parent not bought, or the tree is still locked)
var upgradable = new System.Collections.Generic.List<Agame.Run.SkillNode>();
re.skillTree.GetUpgradableSkillNodes(upgradable);
var nodeNames = new System.Collections.Generic.List<string>();
foreach (var n in upgradable) nodeNames.Add(n.name + " L" + n.Level + "/" + n.LevelCount + " (" + System.Text.RegularExpressions.Regex.Replace(n.GetCostString(), "<sprite[^>]*>", "$") + ")" + (n.IsActivated ? "" : " hidden") + (n.IsDemoLimited ? " DEMO-LIMITED" : ""));
sb.AppendLine("affordableSkillNodes: " + (nodeNames.Count == 0 ? "none" : string.Join(", ", nodeNames)));
return sb.ToString();
