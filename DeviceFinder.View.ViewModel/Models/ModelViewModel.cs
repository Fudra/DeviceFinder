namespace DeviceFinder.ViewModel.Models
{
    public class ModelViewModel
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

        /// <summary>
        ///  Property um die Beide Werte zu vergleichen
        /// --> fabe weiß
        /// </summary>
        public bool IsEqual { get; internal set; }

        /// <summary>
        /// Um zu prüfen, ob es sich um einen Neuen Eintrag handelt
        /// --> farbe Grün
        /// </summary>
        public bool IsNew { get; internal set; }

        /// <summary>
        /// Prüft, ob sich werte geändert haben
        /// --> farbe gelb
        /// </summary>
        public bool IsEdit { get; internal set; }

        /// <summary>
        /// ob das produkt nicht mehr exitiert
        /// --> farbe rot
        /// </summary>
        public bool IsDeleted { get; internal set; }
    }
}