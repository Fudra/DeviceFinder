using System.Text;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Vo
{
    internal class ModelVo : IModelVo
    {
        public string Matnr { get; internal set; }
        
        public bool Coriolis { get; internal set; }
        
        public bool Rotameter { get; internal set; }
        
        public bool Magnetic { get; internal set; }
        
        public bool Ultrasonic { get; internal set; }
        
        public bool Vortex { get; internal set; }
        
        public bool Pressure { get; internal set; }
        
        public bool Liquide { get; internal set; }
        
        public bool Gas { get; internal set; }
        
        public bool Steam { get; internal set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Matnr: " + Matnr);
            sb.AppendLine("Coriolis: " + Coriolis);
            sb.AppendLine("Rotameter: " + Rotameter);
            sb.AppendLine("Magnetic: " + Magnetic);
            sb.AppendLine("Ultrasonic: " + Ultrasonic);
            sb.AppendLine("Vortex: " + Vortex);
            sb.AppendLine("Pressure: " + Pressure);
            sb.AppendLine("Liquide: " + Liquide);
            sb.AppendLine("Gas:" + Gas);
            sb.AppendLine("Steam: " + Steam);
            return sb.ToString();
        }
    }
}