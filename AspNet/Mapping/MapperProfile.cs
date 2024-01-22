using AspNet.Dto.Response;
using AutoMapper;
using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;

namespace WebApi.Mapping
{
    public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<Article, ArticleResponse>().ReverseMap();

			CreateMap<User, UserResponse>().ReverseMap();

			CreateMap<Tag, TagResponse>().ReverseMap();

			CreateMap<Review, ReviewResponse>().ReverseMap();

			CreateMap<ReviewComment, ReviewCommentResponse>().ReverseMap();
		}
	}
}
