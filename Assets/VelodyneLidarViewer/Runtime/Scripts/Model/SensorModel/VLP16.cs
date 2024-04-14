namespace VelodyneLidarViewer.Model.SensorModel
{
    public static class VLP16
    {
        public static readonly VelodyneSensorModelData data = new VelodyneSensorModelData()
        {
            modelFullName = "VLP-16",
            packetRate = 754
        };

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            VelodyneSensorModelRegistry.Register(VelodyneSensorModelEnum.VLP16, data);
        }
    }
}
