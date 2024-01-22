using System;
using System.Collections;
using System.Collections.Generic;

namespace AspNet.Dto.Response
{
    public class ArticleResponse
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public UserResponse Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Content { get; set; }

        public ICollection<TagResponse> Tags { get; set; }
    }
}
