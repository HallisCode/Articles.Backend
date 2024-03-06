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
			CreateMap<Article, ArticleResponse>();

			CreateMap<User, UserResponse>();

			CreateMap<Tag, TagResponse>();

			CreateMap<Review, ReviewResponse>();

			CreateMap<ReviewComment, ReviewCommentResponse>();
		}
	}
}
