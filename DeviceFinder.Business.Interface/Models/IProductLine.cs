using System.Collections.Generic;

namespace DeviceFinder.Business.Interface.Models
{
    public interface IProductLine
    {
        int Id { get; }

        string Name { get; }
        
        // old
        //IPhaseType PhaseType { get; set; }
        
        //ITechnology Technology { get; set; }

        // new
        IEnumerable<IProductLineProperty> ProductLineProperties { get; } 

     }
}