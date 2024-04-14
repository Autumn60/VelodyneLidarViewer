using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VelodyneLidarViewer.Model
{
    public static class VelodyneSensorModelRegistry
    {
        private static Dictionary<VelodyneSensorModelEnum, VelodyneSensorModelData> _registry = new Dictionary<VelodyneSensorModelEnum, VelodyneSensorModelData>();

        public static void Register(VelodyneSensorModelEnum model, VelodyneSensorModelData data)
        {
            if (_registry.ContainsKey(model))
            {
                Debug.LogWarning($"More than one model was registered as \"{model}\"");
                return;
            }
            _registry.Add(model, data);
        }

        public static VelodyneSensorModelData GetModel(VelodyneSensorModelEnum model)
        {
            if (!_registry.ContainsKey(model))
            {
                Debug.LogWarning($"No model was registered as \"{model}\"");
                return default(VelodyneSensorModelData);
            }
            return _registry[model];
        }
    }
}
