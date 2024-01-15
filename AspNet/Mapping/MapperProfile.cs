using AutoMapper;
using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using WebApi.Dto.ArticleScope;
using WebApi.Dto.UserScope;

namespace WebApi.Mapping
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<Article, ArticleDto>().ReverseMap();

			CreateMap<User, UserDto>().ReverseMap();

			CreateMap<Tag, TagDto>().ReverseMap();

			CreateMap<Review, ReviewDto>().ReverseMap();

			CreateMap<ReviewComment, ReviewCommentDto>().ReverseMap();
		}
	}
}
