using System.Collections.Generic;

namespace Scene
{
    public static class SceneDatabase
    {
        public static readonly Dictionary<SceneID, string> Names = new()
        {
            { SceneID.Bootstrap, "BootstrapScene" },
            { SceneID.MainMenu, "MainMenuScene" },
            { SceneID.Gameplay, "GameScene" },
            { SceneID.Loading, "LoadingScene" },
            { SceneID.Test, "TestScene" }
        };
    }
}