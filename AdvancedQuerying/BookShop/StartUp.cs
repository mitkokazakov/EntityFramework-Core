namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            string input = Console.ReadLine();

            string result = GetBooksByCategory(db,input);

            Console.WriteLine(result);
        }

        // Task 2
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context.Books.AsEnumerable().Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        // Task 3
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        // Task 4

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books.Select(b => new
            {
                b.Title,
                b.Price
            })
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }

        // Task 5

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            string result = String.Join(Environment.NewLine, books);

            return result;
        }

        // Task 6

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            List<string> cats = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            var books = context.Books.
                Where(b => b.BookCategories.Any(bc => cats.Contains(bc.Category.Name)))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

	// Task 7

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var parsedInput = DateTime.Parse(date);

            StringBuilder sb = new StringBuilder();

            var booksBeforeYear = context.Books
                .Where(b => b.ReleaseDate < parsedInput)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    BookType = b.EditionType.ToString(),
                    BookPice = b.Price
                })
                .ToList();

            foreach (var book in booksBeforeYear)
            {
                sb.AppendLine($"{book.Title} - {book.BookType} - ${book.BookPice:f2}");
            }

            return sb.ToString().TrimEnd();
        }



        // Task 8

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors.Where(a => a.FirstName.EndsWith(input))
                .Select(a => new { FullName = a.FirstName + ' ' + a.LastName })
                .OrderBy(a => a.FullName)
                .ToList();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FullName}");
            }

            return sb.ToString().TrimEnd();
        }


        // Task 9

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var bookTitles = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            return String.Join(Environment.NewLine, bookTitles);
        }

	// Task 10

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new {
                    b.Title,
                    FullName = b.Author.FirstName + ' ' + b.Author.LastName
                })
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.FullName})");
            }

            return sb.ToString().TrimEnd();
        }

 	// Task 11

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int result = 0;

            var booksWithLongerTitle = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToList();

            result = booksWithLongerTitle.Count();

            return result;
        }


        // Task 12

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var tototalBookCopies = context.Authors.Select(a => new
            {
                FullName = a.FirstName + ' ' + a.LastName,
                TotalCopies = a.Books.Sum(b => b.Copies)
            })
                .OrderByDescending(b => b.TotalCopies).ToList();

            foreach (var author in tototalBookCopies)
            {
                sb.AppendLine($"{author.FullName} - {author.TotalCopies}");
            }

            return sb.ToString().TrimEnd();
        }


        // Task 13

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var profits = context.Categories
                .Select(c => new {
                    CatName = c.Name,
                    TotalProfit = c.CategoryBooks
                                    .Select(cb => new {
                                        Profit = cb.Book.Price * cb.Book.Copies
                                    }).Sum(b => b.Profit)
                })
                .OrderByDescending(p => p.TotalProfit)
                .ThenBy(p => p.CatName);


            foreach (var p in profits)
            {
                sb.AppendLine($"{p.CatName} ${p.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
