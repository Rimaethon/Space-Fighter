﻿using UnityEditor;

namespace Rimaethon.TileMapping.com.unity._2d.tilemap.extras.Editor.Tiles.RuleTile
{
    static class CustomRuleTileMenu
    {
        [MenuItem("Assets/Create/Custom Rule Tile Script", false, 89)]
        static void CreateCustomRuleTile()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile("Packages/com.unity.2d.tilemap.extras/Editor/Tiles/RuleTile/ScriptTemplates/NewCustomRuleTile.cs.txt", "NewCustomRuleTile.cs");
        }
    }
}
