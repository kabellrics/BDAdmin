using AutoMapper;
using Common;
using DBConnector;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Business
{
    public static class MyMapper
    {
        private static bool _isInitialized;
        public static IMapper Mapper;
        public static void Initialize()
        {
            if (!_isInitialized)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Fichier, DBFichier>();
                    cfg.CreateMap<DBFichier, Fichier>();
                    cfg.CreateMap<Serie, DBSerie>()
                    .ForMember(destination => destination.Extension, opts => opts.MapFrom(source => source.ImgExtension));
                    cfg.CreateMap<DBSerie, Serie>()
                    .ForMember(destination => destination.ImgExtension, opts => opts.MapFrom(source => source.Extension));
                });

                Mapper = config.CreateMapper();
                _isInitialized = true;
            }
        }
    }
}
