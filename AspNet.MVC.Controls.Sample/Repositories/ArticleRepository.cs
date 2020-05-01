using AspNet.MVC.Controls.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet.MVC.Controls.Sample.Repositories
{
    public static class ArticleRepository
    {
        private static IEnumerable<ArticleViewModel> articles;

        public static IEnumerable<ArticleViewModel> GetArticles()
        {
            if (articles == null)
            {
                FillArticles();
            }

            return articles;
        }

        private static void FillArticles()
        {
            Random rnd = new Random();
            string[] words = "lorem ipsum dolor amet consetetur sadipscing elitr diam nonumy eirmod tempor invidunt labore dolore magna aliquyam erat diam voluptua vero accusam justo dolores rebum clita kasd gubergren takimata sanctus".Split(' ');
            List<ArticleViewModel> result = new List<ArticleViewModel>();

            for (int i = 1; i <= 50; i++)
            {
                string name = words[rnd.Next(words.Length)] + " " + words[rnd.Next(words.Length)];
                result.Add(new ArticleViewModel()
                {
                    Id = i,
                    Name = char.ToUpper(name[0]) + name.Substring(1),
                    ReleaseDate = new DateTime(rnd.Next(2000, 2020), rnd.Next(1, 13), rnd.Next(1, 29)),
                    Price = (decimal)(rnd.NextDouble() * 50)
                });
            }

            articles = result.ToArray();
        }
    }
}