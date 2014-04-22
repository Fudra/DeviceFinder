namespace DeviceFinder.Data.Interface.Vo
{
    public interface IModelVo
    {
        /// <summary>
        /// Name of the Type
        /// </summary>
        string Matnr { get; }

        /// <summary>
        ///  Part of Technology
        /// </summary>
        bool Coriolis { get; }

        /// <summary>
        /// part of Technology
        /// </summary>
        bool Rotameter { get; }

        /// <summary>
        ///  Part of Technology
        /// </summary>
        bool Magnetic { get; }

        /// <summary>
        ///  Part of Technology
        /// </summary>
        bool Ultrasonic { get; }

        /// <summary>
        ///  Part of Technology
        /// </summary>
        bool Vortex { get; }

        /// <summary>
        ///  Part of Technology
        /// </summary>
        bool Pressure { get; }

        /// <summary>
        ///  Part of Phase Type
        /// </summary>
        bool Liquide { get; }

        /// <summary>
        ///  Part of Phase Type
        /// </summary>
        bool Gas { get; }

        /// <summary>
        ///  Part of Phase Type
        /// </summary>
        bool Steam { get; }
     }
}