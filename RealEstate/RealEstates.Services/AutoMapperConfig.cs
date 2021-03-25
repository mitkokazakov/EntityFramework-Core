using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Services
{
    public abstract class AutoMapperConfig
    {
        public AutoMapperConfig()
        {
            this.Initializer();
        }

        protected IMapper Mapper { get; private set; }

        private void Initializer()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RealEstatesProfiler>();
            });

            this.Mapper = config.CreateMapper();
        }
    }
}
