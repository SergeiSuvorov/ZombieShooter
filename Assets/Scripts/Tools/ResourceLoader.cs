using UnityEngine;

namespace Tools
{
    public static class ResourceLoader
    {
        public static Sprite LoadSprite(ResourcePath path)
        {
            return Resources.Load<Sprite>(path.PathResource);
        }

        public static GameObject LoadPrefab(ResourcePath path)
        {
            return Resources.Load<GameObject>(path.PathResource);
        }
    }
}

