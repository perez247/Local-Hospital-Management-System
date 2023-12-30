using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public static class AppMapper
    {
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            //Provide all the Mapping Configuration
            var config = new MapperConfiguration(cfg =>
            {
                //Configuring Employee and EmployeeDTO
                cfg.CreateMap<TSource, TDestination>();
                //Any Other Mapping Configuration ....
            });

            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper.Map<TDestination>(source);
        }
    }
}
