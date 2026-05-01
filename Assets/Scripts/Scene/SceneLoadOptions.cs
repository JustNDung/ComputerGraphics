using System;

namespace Scene
{
    public class SceneLoadOptions
    {
        public SceneID scene;

        public bool useFade = true;
        public bool useLoadingUI = true;

        public float minLoadTime = 0.5f;

        public Action onComplete;

        public SceneLoadOptions(SceneID scene)
        {
            this.scene = scene;
        }
    }
}