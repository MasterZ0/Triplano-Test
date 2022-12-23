using UnityEngine;

namespace TriplanoTest.Shared.ExtensionMethods
{
    public static class LayerMaskExtensions
    {
        public static bool CompareLayer(this Collider2D collider, LayerMask layerMask)
        {
            return CompareLayer(collider.gameObject.layer, layerMask.value);
        }
        
        public static bool CompareLayer(this LayerMask layerMask, int standardLayer)
        {
            return CompareLayer(standardLayer, layerMask.value);
        }
        
        public static bool CompareLayer(this GameObject layerObject, LayerMask layerMask)
        {
            return CompareLayer(layerObject.layer, layerMask.value);
        }
        
        public static bool CompareLayer(this int standardLayer, LayerMask layerMask)
        {
            return CompareLayer(standardLayer, layerMask.value);
        }
        
        public static bool CompareLayer(this int standardLayer, int layerMask)
        {
            return ((1 << standardLayer) & layerMask) != 0;
        }
    }
}