using System.Collections.Generic;
using System.Linq;
using DeviceFinder.Business.Interface.Models;
using DeviceFinder.ViewModel.Models;


namespace DeviceFinder.ViewModel
{
    public class ViewModelFactory
    {
        internal IEnumerable<ModelViewModel> Create(IModel[] results)
        {
            return results.Select(i => new ModelViewModel()
            {
                Matnr = i.Matnr,
                Gas = i.Gas,
                Magnetic = i.Magnetic,
                Pressure = i.Pressure,
                Rotameter = i.Rotameter,
                Steam = i.Steam,
                Ultrasonic = i.Ultrasonic,
                Vortex = i.Vortex,
                Coriolis = i.Coriolis,
                Liquide = i.Liquide
            });
        }

        //internal IEnumerable<ProductLineViewModel> Create(IProductLine[] results)
        //{
        //    return results.Select(i => new ProductLineViewModel()
        //    {
        //       Name = i.Name,
        //       Ultrasonic = i.Technology.Ultrasonic,
        //       Rotameter = i.Technology.Rotameter,
        //       Magnetic = i.Technology.Magnetic,
        //       Pressure = i.Technology.Pressure,
        //       Coriolis = i.Technology.Coriolis,
        //       Vortex = i.Technology.Vortex,
        //       Liquide = i.PhaseType.Liquide,
        //       Gas = i.PhaseType.Gas,
        //       Steam = i.PhaseType.Steam,
        //    });
        //}

        internal IEnumerable<ProductLineViewModel> Create(IProductLine[] results)
        {
            var  data =results.Select(i => new ProductLineViewModel()
            {
                Name = i.Name,
                ProductLineProperetyViewModels = i.ProductLineProperties.Select(j => new ProductLineProperetyViewModel()
                {
                    Group = j.Group,
                    Name = j.Name,
                    Value = j.Value
                })
            });
            return data;
        }


    }
}