﻿using Domain.Entities.ArticleScope;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.Repositories
{
	public class TagRepository
	{
		private readonly ApplicationDbContext context;

		public TagRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		#region NullableMethods

		public async Task<Tag?> TryGetByAsync(long id)
		{
			Tag? tag = await context.Tags.AsNoTracking()
				.FirstOrDefaultAsync(tag => tag.Id == id);

			return tag;
		}

		public async Task<List<Tag>?> TryGetByAsync(ICollection<long> tagsId)
		{
			List<Tag> _tags = new List<Tag>();

			foreach (long tagId in tagsId)
			{
				Tag? tag = await TryGetByAsync(tagId);

				if (tag is not null) _tags.Add(tag);
			}

			return _tags;
		}


		public async Task<Tag?> TryGetByAsync(string title)
		{
			Tag? tag = await context.Tags.AsNoTracking()
				.FirstOrDefaultAsync(tag => EF.Functions.ILike(tag.Title, title));

			return tag;
		}

		#endregion

		#region NotNullableMehdos



		#endregion

	}
}
