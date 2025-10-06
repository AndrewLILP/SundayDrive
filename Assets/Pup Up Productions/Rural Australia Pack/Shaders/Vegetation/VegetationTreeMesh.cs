using System.Linq;
using UnityEngine;

namespace Shaders.Vegetation_Tree_v1
{
    [RequireComponent(typeof(MeshRenderer))]
    public class VegetationTreeV1Mesh : MonoBehaviour
    {
        public MeshType type;
        
        private Material[] _materialCache = null;

        private Material[] FindAllChildSharedMaterials()
        {
            var meshes = GetComponents<MeshRenderer>();
            var materials = meshes.SelectMany(m => m.sharedMaterials).ToArray();
            return materials;
        }
        
        public Material[] AllMaterials
        {
            get
            {
                if (Application.isPlaying)
                {
                    _materialCache ??= FindAllChildSharedMaterials();
                    return _materialCache;
                }

                return FindAllChildSharedMaterials();
            }
        }

        public enum MeshType
        {
            Trunk,
            Leaves
        }

        public void ResetMaterialCache()
        {
            _materialCache = null;
        }
    }
}