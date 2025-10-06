using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Shaders.Vegetation_Tree_v1
{
    [ExecuteAlways]
    public class VegetationTreeV1 : MonoBehaviour
    {
        public Vector2 windVector;

        public WindGust windGust;

        public LeafFlutter leafFlutter;
        
        public bool apply;

        private float _elapsedTime;
        
        private static readonly int WindVector = Shader.PropertyToID("_Wind_Vector");
        private static readonly int WindGustFrequency = Shader.PropertyToID("_Wind_Gust_Frequency");
        private static readonly int WindGustNoiseRoughness = Shader.PropertyToID("_Wind_Gust_Noise_Roughness");
        private static readonly int WindGustNoiseSize = Shader.PropertyToID("_Wind_Gust_Noise_Size");
        private static readonly int WindVerticalDisplacementSize = Shader.PropertyToID("_Wind_Vertical_Displacement_Size");
        
        private static readonly int LeafFlutterFrequency = Shader.PropertyToID("_Leaf_Flutter_Frequency");
        private static readonly int LeafFlutterScale = Shader.PropertyToID("_Leaf_Flutter_Scale");
        private static readonly int LeafFlutterNoiseSize = Shader.PropertyToID("_Leaf_Flutter_Noise_Size");
        private static readonly int LeafFlutterNoiseRoughness = Shader.PropertyToID("_Leaf_Flutter_Noise_Roughness");
        
        private VegetationTreeV1Mesh[] _meshCache;

        public void ResetMeshCache()
        {
            _meshCache = null;
        }
        
        void Update()
        {
            if (!apply) return;
            apply = false;

            ApplyShaderProperties();
        }

        private void ApplyShaderProperties()
        {
            foreach (var mesh in AllMeshes)
            {
                foreach (var material in mesh.AllMaterials)
                {
                    material.SetVector(WindVector, windVector);
                    material.SetFloat(WindGustFrequency, windGust.windGustFrequency);
                    material.SetFloat(WindGustNoiseRoughness, windGust.windGustNoiseRoughness);
                    material.SetFloat(WindGustNoiseSize, windGust.windGustNoiseSize);
                    material.SetFloat(WindVerticalDisplacementSize, windGust.windVerticalDisplacementSize);
                    if (mesh.type == VegetationTreeV1Mesh.MeshType.Leaves)
                    {
                        material.SetFloat(LeafFlutterFrequency, leafFlutter.leafFlutterFrequency);
                        material.SetFloat(LeafFlutterScale, leafFlutter.leafFlutterScale);
                        material.SetFloat(LeafFlutterNoiseSize, leafFlutter.leafFlutterNoiseSize);
                        material.SetFloat(LeafFlutterNoiseRoughness, leafFlutter.leafFlutterNoiseRoughness);
                    }
                    else
                    {
                        material.SetFloat(LeafFlutterScale, 0f);
                    }
                }
            }
        }

        private VegetationTreeV1Mesh[] AllMeshes
        {
            get
            {
                if (Application.isPlaying)
                {
                    _meshCache ??= FindAllChildMeshes();
                    return _meshCache;
                }

                return FindAllChildMeshes();
            }
        }

        private VegetationTreeV1Mesh[] FindAllChildMeshes()
        {
            var meshes = GetComponentsInChildren<VegetationTreeV1Mesh>();
            foreach (var mesh in meshes)
            {
                mesh.ResetMaterialCache();
            }

            return meshes;
        }


        [System.Serializable]
        public class WindGust
        {
            [Tooltip("How quick the back and force flow of wind gusts is")]
            public float windGustFrequency = 0.5f;
            
            [Tooltip("Every tree gets a unique gust frequency with this must noise based on its world position")]
            public float windGustNoiseSize = 0.2f;
            
            [Tooltip("How smooth is the 'world noise'; low values mean trees near each other will have similar gusts, high means they'll be different")]
            public float windGustNoiseRoughness = 0.01f;

            [Tooltip("If you want your tree to have vertical displacement as well, put the scale for that here; it is independent of the wind vector")]
            public float windVerticalDisplacementSize = 0.2f;
        }
        
        [System.Serializable]
        public class LeafFlutter
        {
            [Tooltip("How quick the back and force flow of wind gusts is on leaves")]
            public float leafFlutterFrequency = 0.5f;
            
            [Tooltip("How much the leaves flutter? This is independent of the wind vector! Set it appropriately")]
            public float leafFlutterScale = 1;
            
            [Tooltip("Every leaf vertex gets a unique gust frequency with this must noise based on its world position")]
            public float leafFlutterNoiseSize = 0.2f;
            
            [Tooltip("How smooth is the 'leaf noise'; low values mean leaves near each other will have similar gusts, high means they'll be different")]
            public float leafFlutterNoiseRoughness = 0.01f;
        }
    }
}