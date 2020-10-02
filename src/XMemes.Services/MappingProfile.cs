using AutoMapper;

using System;
using System.Collections.Generic;

using XMemes.Models.Domain;
using XMemes.Models.InputModels;
using XMemes.Models.Paging;
using XMemes.Models.ViewModels;

using Meme = XMemes.Models.Domain.Meme;
using Tag = XMemes.Models.Domain.Tag;

namespace XMemes.Services
{
    public class PagedListConverter<TIn, TOut> : ITypeConverter<IPagedList<TIn>, IPagedList<TOut>>
    {
        public IPagedList<TOut> Convert(
            IPagedList<TIn> source, 
            IPagedList<TOut> _, 
            ResolutionContext context)
        {
            var mapped = context.Mapper.Map<List<TOut>>(source);
            return new PagedList<TOut>(mapped, source.PageIndex, source.PageSize, source.TotalCount);
        }
    }

    public static class MappingConfig {
        public static void Config(IMapperConfigurationExpression config) {
            config.CreateMap<string?, string?>()
                .ConvertUsing(s => s ?? string.Empty);

            config.CreateMap<string?, string>()
                .ConvertUsing(s => s ?? string.Empty);
            
            config.CreateMap<string, string?>() 
                .ConvertUsing(s => string.IsNullOrWhiteSpace(s) ? string.Empty : s);

            config.CreateMap<string, Guid>()
                .ConvertUsing(s =>
                    string.IsNullOrWhiteSpace(s) ? Guid.NewGuid() : Guid.Parse(s));

            config.CreateMap<string?, Guid>()
                .ConvertUsing(s => 
                    string.IsNullOrWhiteSpace(s) ? Guid.NewGuid() : Guid.Parse(s));

            config.CreateMap<Guid, string?>()
                .ConvertUsing(g => g.ToString());

            config.CreateMap<Guid, string>()
                .ConvertUsing(g => g.ToString());

            #region Tag
            
            config.CreateMap<TagInput, Tag>();
            config.CreateMap<Tag, TagInput>();
            config.CreateMap<Tag, TagViewModel>();
            config.CreateMap<IPagedList<Tag>, IPagedList<TagViewModel>>()
                .ConvertUsing(new PagedListConverter<Tag, TagViewModel>());

            #endregion

            #region Memer

            config.CreateMap<MemerInput, Memer>();
            config.CreateMap<Memer, MemerInput>();
            config.CreateMap<Memer, MemerViewModel>();
            config.CreateMap<IPagedList<Memer>, IPagedList<MemerViewModel>>()
                .ConvertUsing(new PagedListConverter<Memer, MemerViewModel>());

            #endregion

            config.CreateMap<MemeInput, Meme>();
            config.CreateMap<Meme, MemeInput>();
            config.CreateMap<TemplateInput, Template>();
            config.CreateMap<Template, TemplateInput>();


        }
    }
}
