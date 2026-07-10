using UnityEngine;

namespace I2.Loc
{
	public static class ScriptLocalization
	{

		public static class BuildItem
		{
			public static string Can_be_bought_repeatedly 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Can be bought repeatedly",applyParameters:true); } }
		}

		public static class BuildItem_Line
		{
			public static string X_MaxHP 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/X_MaxHP",applyParameters:true); } }
			public static string X_PercentTowerBulletDamage 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/X_PercentTowerBulletDamage",applyParameters:true); } }
			public static string X_PercentTowerRange 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/X_PercentTowerRange",applyParameters:true); } }
			public static string ForEachNegativeMainStat_X 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/ForEachNegativeMainStat_X",applyParameters:true); } }
			public static string ForEachNegativeMainStat_X_YouAreHaving_Y 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/ForEachNegativeMainStat_X_YouAreHaving_Y",applyParameters:true); } }
			public static string ForEachSkillYouHave_X 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/ForEachSkillYouHave_X",applyParameters:true); } }
			public static string ForEachGlitchy_X_YouAreHaving_Y_Glitchy 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/ForEachGlitchy_X_YouAreHaving_Y_Glitchy",applyParameters:true); } }
			public static string ForEachGlitchy_X 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/ForEachGlitchy_X",applyParameters:true); } }
			public static string Up_on_pickup__if_X_is_less_than_or_equal_to_Y 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/Up on pickup, if X is less than or equal to Y",applyParameters:true); } }
			public static string Double_your_current_X 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/Double your current X",applyParameters:true); } }
			public static string For_each_X_Stat 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/For each X Stat",applyParameters:true); } }
			public static string Can_not_be_locked_in_the_shop 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/Can not be locked in the shop",applyParameters:true); } }
			public static string for_each_Support_Unit_you_have 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/for each Support Unit you have",applyParameters:true); } }
			public static string For_each_item_with_Unique_tag 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/For each item with Unique tag",applyParameters:true); } }
			public static string After_each_fight 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/After each fight",applyParameters:true); } }
			public static string Each_time_the_tower_levels_up 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/Each time the tower levels up",applyParameters:true); } }
			public static string Remove_X_from_the_item_pool_this_run 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/Remove X from the item pool this run",applyParameters:true); } }
			public static string Get_a_random_X_item_after_the_next_Y_fights 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/Get a random X item after the next Y fights",applyParameters:true); } }
		}

		public static class BuildItem_Line_Skill
		{
			public static string Cool_down_time__X 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/Skill/Cool down time: X",applyParameters:true); } }
			public static string HPCost 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Line/Skill/HPCost",applyParameters:true); } }
		}

		public static class BuildItem_Tag
		{
			public static string Artifact 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/Artifact",applyParameters:true); } }
			public static string Glitchy 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/Glitchy",applyParameters:true); } }
			public static string LevelUp 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/LevelUp",applyParameters:true); } }
			public static string Limited 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/Limited",applyParameters:true); } }
			public static string Skill 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/Skill",applyParameters:true); } }
			public static string SkillEnhancement 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/SkillEnhancement",applyParameters:true); } }
			public static string SkillTrait 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/SkillTrait",applyParameters:true); } }
			public static string Tower 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/Tower",applyParameters:true); } }
			public static string Trap 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/Trap",applyParameters:true); } }
			public static string Unique 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/Unique",applyParameters:true); } }
			public static string Support_Unit 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/Support Unit",applyParameters:true); } }
			public static string Difficulty 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/Difficulty",applyParameters:true); } }
			public static string Cursed 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/Cursed",applyParameters:true); } }
			public static string BonusContent 		{ get{ return LocalizationManager.GetTranslation ("BuildItem/Tag/BonusContent",applyParameters:true); } }
		}

		public static class Common
		{
			public static string Bosses 		{ get{ return LocalizationManager.GetTranslation ("Common/Bosses",applyParameters:true); } }
			public static string Difficulties 		{ get{ return LocalizationManager.GetTranslation ("Common/Difficulties",applyParameters:true); } }
			public static string Elite_enemies 		{ get{ return LocalizationManager.GetTranslation ("Common/Elite enemies",applyParameters:true); } }
			public static string Normal_enemies 		{ get{ return LocalizationManager.GetTranslation ("Common/Normal enemies",applyParameters:true); } }
			public static string Shop_Items 		{ get{ return LocalizationManager.GetTranslation ("Common/Shop Items",applyParameters:true); } }
			public static string Skills 		{ get{ return LocalizationManager.GetTranslation ("Common/Skills",applyParameters:true); } }
			public static string Towers 		{ get{ return LocalizationManager.GetTranslation ("Common/Towers",applyParameters:true); } }
			public static string Enemy_Damage 		{ get{ return LocalizationManager.GetTranslation ("Common/Enemy Damage",applyParameters:true); } }
			public static string Enemy_HP 		{ get{ return LocalizationManager.GetTranslation ("Common/Enemy HP",applyParameters:true); } }
			public static string Enemy_speed 		{ get{ return LocalizationManager.GetTranslation ("Common/Enemy speed",applyParameters:true); } }
			public static string Recommended 		{ get{ return LocalizationManager.GetTranslation ("Common/Recommended",applyParameters:true); } }
			public static string Off 		{ get{ return LocalizationManager.GetTranslation ("Common/Off",applyParameters:true); } }
			public static string Default 		{ get{ return LocalizationManager.GetTranslation ("Common/Default",applyParameters:true); } }
			public static string Custom 		{ get{ return LocalizationManager.GetTranslation ("Common/Custom",applyParameters:true); } }
			public static string No_Curses 		{ get{ return LocalizationManager.GetTranslation ("Common/No Curses",applyParameters:true); } }
			public static string Endless 		{ get{ return LocalizationManager.GetTranslation ("Common/Endless",applyParameters:true); } }
			public static string CommonRarity 		{ get{ return LocalizationManager.GetTranslation ("Common/CommonRarity",applyParameters:true); } }
			public static string RareRarity 		{ get{ return LocalizationManager.GetTranslation ("Common/RareRarity",applyParameters:true); } }
			public static string UncommonRarity 		{ get{ return LocalizationManager.GetTranslation ("Common/UncommonRarity",applyParameters:true); } }
			public static string Spend_X_more_Talent_Points_to_unlock 		{ get{ return LocalizationManager.GetTranslation ("Common/Spend X more Talent Points to unlock",applyParameters:true); } }
			public static string GameMode 		{ get{ return LocalizationManager.GetTranslation ("Common/GameMode",applyParameters:true); } }
		}

		public static class Difficulty
		{
			public static string Hostility_0 		{ get{ return LocalizationManager.GetTranslation ("Difficulty/Hostility 0",applyParameters:true); } }
			public static string Hostility_1 		{ get{ return LocalizationManager.GetTranslation ("Difficulty/Hostility 1",applyParameters:true); } }
			public static string Hostility_2 		{ get{ return LocalizationManager.GetTranslation ("Difficulty/Hostility 2",applyParameters:true); } }
			public static string Hostility_3 		{ get{ return LocalizationManager.GetTranslation ("Difficulty/Hostility 3",applyParameters:true); } }
			public static string Hostility_4 		{ get{ return LocalizationManager.GetTranslation ("Difficulty/Hostility 4",applyParameters:true); } }
			public static string Hostility_5 		{ get{ return LocalizationManager.GetTranslation ("Difficulty/Hostility 5",applyParameters:true); } }
			public static string Onboarding 		{ get{ return LocalizationManager.GetTranslation ("Difficulty/Onboarding",applyParameters:true); } }
		}

		public static class Enemy_Tag_Regular
		{
			public static string Boss 		{ get{ return LocalizationManager.GetTranslation ("Enemy/Tag/Regular/Boss",applyParameters:true); } }
			public static string Regular 		{ get{ return LocalizationManager.GetTranslation ("Enemy/Tag/Regular/Regular",applyParameters:true); } }
			public static string Special 		{ get{ return LocalizationManager.GetTranslation ("Enemy/Tag/Regular/Special",applyParameters:true); } }
		}

		public static class Run
		{
			public static string X_seconds 		{ get{ return LocalizationManager.GetTranslation ("Run/X seconds",applyParameters:true); } }
			public static string Wave 		{ get{ return LocalizationManager.GetTranslation ("Run/Wave",applyParameters:true); } }
		}

		public static class Run_Combat
		{
			public static string Missed 		{ get{ return LocalizationManager.GetTranslation ("Run/Combat/Missed",applyParameters:true); } }
		}

		public static class Setting
		{
			public static string Custom_difficulty 		{ get{ return LocalizationManager.GetTranslation ("Setting/Custom difficulty",applyParameters:true); } }
		}
	}

    public static class ScriptTerms
	{

		public static class BuildItem
		{
		    public const string Can_be_bought_repeatedly = "BuildItem/Can be bought repeatedly";
		}

		public static class BuildItem_Line
		{
		    public const string X_MaxHP = "BuildItem/Line/X_MaxHP";
		    public const string X_PercentTowerBulletDamage = "BuildItem/Line/X_PercentTowerBulletDamage";
		    public const string X_PercentTowerRange = "BuildItem/Line/X_PercentTowerRange";
		    public const string ForEachNegativeMainStat_X = "BuildItem/Line/ForEachNegativeMainStat_X";
		    public const string ForEachNegativeMainStat_X_YouAreHaving_Y = "BuildItem/Line/ForEachNegativeMainStat_X_YouAreHaving_Y";
		    public const string ForEachSkillYouHave_X = "BuildItem/Line/ForEachSkillYouHave_X";
		    public const string ForEachGlitchy_X_YouAreHaving_Y_Glitchy = "BuildItem/Line/ForEachGlitchy_X_YouAreHaving_Y_Glitchy";
		    public const string ForEachGlitchy_X = "BuildItem/Line/ForEachGlitchy_X";
		    public const string Up_on_pickup__if_X_is_less_than_or_equal_to_Y = "BuildItem/Line/Up on pickup, if X is less than or equal to Y";
		    public const string Double_your_current_X = "BuildItem/Line/Double your current X";
		    public const string For_each_X_Stat = "BuildItem/Line/For each X Stat";
		    public const string Can_not_be_locked_in_the_shop = "BuildItem/Line/Can not be locked in the shop";
		    public const string for_each_Support_Unit_you_have = "BuildItem/Line/for each Support Unit you have";
		    public const string For_each_item_with_Unique_tag = "BuildItem/Line/For each item with Unique tag";
		    public const string After_each_fight = "BuildItem/Line/After each fight";
		    public const string Each_time_the_tower_levels_up = "BuildItem/Line/Each time the tower levels up";
		    public const string Remove_X_from_the_item_pool_this_run = "BuildItem/Line/Remove X from the item pool this run";
		    public const string Get_a_random_X_item_after_the_next_Y_fights = "BuildItem/Line/Get a random X item after the next Y fights";
		}

		public static class BuildItem_Line_Skill
		{
		    public const string Cool_down_time__X = "BuildItem/Line/Skill/Cool down time: X";
		    public const string HPCost = "BuildItem/Line/Skill/HPCost";
		}

		public static class BuildItem_Tag
		{
		    public const string Artifact = "BuildItem/Tag/Artifact";
		    public const string Glitchy = "BuildItem/Tag/Glitchy";
		    public const string LevelUp = "BuildItem/Tag/LevelUp";
		    public const string Limited = "BuildItem/Tag/Limited";
		    public const string Skill = "BuildItem/Tag/Skill";
		    public const string SkillEnhancement = "BuildItem/Tag/SkillEnhancement";
		    public const string SkillTrait = "BuildItem/Tag/SkillTrait";
		    public const string Tower = "BuildItem/Tag/Tower";
		    public const string Trap = "BuildItem/Tag/Trap";
		    public const string Unique = "BuildItem/Tag/Unique";
		    public const string Support_Unit = "BuildItem/Tag/Support Unit";
		    public const string Difficulty = "BuildItem/Tag/Difficulty";
		    public const string Cursed = "BuildItem/Tag/Cursed";
		    public const string BonusContent = "BuildItem/Tag/BonusContent";
		}

		public static class Common
		{
		    public const string Bosses = "Common/Bosses";
		    public const string Difficulties = "Common/Difficulties";
		    public const string Elite_enemies = "Common/Elite enemies";
		    public const string Normal_enemies = "Common/Normal enemies";
		    public const string Shop_Items = "Common/Shop Items";
		    public const string Skills = "Common/Skills";
		    public const string Towers = "Common/Towers";
		    public const string Enemy_Damage = "Common/Enemy Damage";
		    public const string Enemy_HP = "Common/Enemy HP";
		    public const string Enemy_speed = "Common/Enemy speed";
		    public const string Recommended = "Common/Recommended";
		    public const string Off = "Common/Off";
		    public const string Default = "Common/Default";
		    public const string Custom = "Common/Custom";
		    public const string No_Curses = "Common/No Curses";
		    public const string Endless = "Common/Endless";
		    public const string CommonRarity = "Common/CommonRarity";
		    public const string RareRarity = "Common/RareRarity";
		    public const string UncommonRarity = "Common/UncommonRarity";
		    public const string Spend_X_more_Talent_Points_to_unlock = "Common/Spend X more Talent Points to unlock";
		    public const string GameMode = "Common/GameMode";
		}

		public static class Difficulty
		{
		    public const string Hostility_0 = "Difficulty/Hostility 0";
		    public const string Hostility_1 = "Difficulty/Hostility 1";
		    public const string Hostility_2 = "Difficulty/Hostility 2";
		    public const string Hostility_3 = "Difficulty/Hostility 3";
		    public const string Hostility_4 = "Difficulty/Hostility 4";
		    public const string Hostility_5 = "Difficulty/Hostility 5";
		    public const string Onboarding = "Difficulty/Onboarding";
		}

		public static class Enemy_Tag_Regular
		{
		    public const string Boss = "Enemy/Tag/Regular/Boss";
		    public const string Regular = "Enemy/Tag/Regular/Regular";
		    public const string Special = "Enemy/Tag/Regular/Special";
		}

		public static class Run
		{
		    public const string X_seconds = "Run/X seconds";
		    public const string Wave = "Run/Wave";
		}

		public static class Run_Combat
		{
		    public const string Missed = "Run/Combat/Missed";
		}

		public static class Setting
		{
		    public const string Custom_difficulty = "Setting/Custom difficulty";
		}
	}
}