using Bangla.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangla.Services.ProductAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
        public DbSet<Product> Products { get; set; } // Plural coupons will be the table name

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Biriyani",
                Price = 15,
                Description = "Lorem ipsum dolor sit amet. Et voluptatum laudantium eos dolorem ratione est doloribus nesciunt ea architecto unde ut consequuntur neque. Et neque illum aut vero ipsam sed quibusdam voluptatibus qui internos exercitationem qui magni animi qui fuga neque quo commodi consequatur. In repudiandae deserunt eos excepturi quibusdam et rerum voluptatem vel magni officia rem voluptatem expedita qui praesentium quod ea inventore corporis. Qui assumenda dignissimos aut voluptas veritatis ut ipsa velit est soluta quaerat in adipisci quis. </p><p>Ut consequuntur error et dolorum aperiam et velit amet aut laborum optio aut voluptas dicta cum voluptatem consequuntur. Aut rerum rerum sed iure perferendis rem optio earum et voluptate totam. </p><p>Ut dicta omnis et laboriosam repellat eos assumenda dolore est dolor temporibus aut porro eligendi qui error molestiae! At sint natus quo omnis mollitia et optio quisquam et natus nihil qui consequatur maiores. Hic repellendus consequatur et consequatur nisi qui adipisci voluptatem. Et nisi molestiae aut internos tenetur quo similique odio?",
                ImageUrl = "https://placehold.co/603x403",
                CategoryName = "Main Dish"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Fuchka",
                Price = 13.99,
                Description = "Lorem ipsum dolor sit amet. Et voluptatum laudantium eos dolorem ratione est doloribus nesciunt ea architecto unde ut consequuntur neque. Et neque illum aut vero ipsam sed quibusdam voluptatibus qui internos exercitationem qui magni animi qui fuga neque quo commodi consequatur. In repudiandae deserunt eos excepturi quibusdam et rerum voluptatem vel magni officia rem voluptatem expedita qui praesentium quod ea inventore corporis. Qui assumenda dignissimos aut voluptas veritatis ut ipsa velit est soluta quaerat in adipisci quis. </p><p>Ut consequuntur error et dolorum aperiam et velit amet aut laborum optio aut voluptas dicta cum voluptatem consequuntur. Aut rerum rerum sed iure perferendis rem optio earum et voluptate totam. </p><p>Ut dicta omnis et laboriosam repellat eos assumenda dolore est dolor temporibus aut porro eligendi qui error molestiae! At sint natus quo omnis mollitia et optio quisquam et natus nihil qui consequatur maiores. Hic repellendus consequatur et consequatur nisi qui adipisci voluptatem. Et nisi molestiae aut internos tenetur quo similique odio?",
                ImageUrl = "https://placehold.co/602x402",
                CategoryName = "Snacks"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Kala Vuna",
                Price = 10.99,
                Description = "Lorem ipsum dolor sit amet. Et voluptatum laudantium eos dolorem ratione est doloribus nesciunt ea architecto unde ut consequuntur neque. Et neque illum aut vero ipsam sed quibusdam voluptatibus qui internos exercitationem qui magni animi qui fuga neque quo commodi consequatur. In repudiandae deserunt eos excepturi quibusdam et rerum voluptatem vel magni officia rem voluptatem expedita qui praesentium quod ea inventore corporis. Qui assumenda dignissimos aut voluptas veritatis ut ipsa velit est soluta quaerat in adipisci quis. </p><p>Ut consequuntur error et dolorum aperiam et velit amet aut laborum optio aut voluptas dicta cum voluptatem consequuntur. Aut rerum rerum sed iure perferendis rem optio earum et voluptate totam. </p><p>Ut dicta omnis et laboriosam repellat eos assumenda dolore est dolor temporibus aut porro eligendi qui error molestiae! At sint natus quo omnis mollitia et optio quisquam et natus nihil qui consequatur maiores. Hic repellendus consequatur et consequatur nisi qui adipisci voluptatem. Et nisi molestiae aut internos tenetur quo similique odio?",
                ImageUrl = "https://placehold.co/601x401",
                CategoryName = "Traditional"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Vapa Pitha",
                Price = 15,
                Description = "Lorem ipsum dolor sit amet. Et voluptatum laudantium eos dolorem ratione est doloribus nesciunt ea architecto unde ut consequuntur neque. Et neque illum aut vero ipsam sed quibusdam voluptatibus qui internos exercitationem qui magni animi qui fuga neque quo commodi consequatur. In repudiandae deserunt eos excepturi quibusdam et rerum voluptatem vel magni officia rem voluptatem expedita qui praesentium quod ea inventore corporis. Qui assumenda dignissimos aut voluptas veritatis ut ipsa velit est soluta quaerat in adipisci quis. </p><p>Ut consequuntur error et dolorum aperiam et velit amet aut laborum optio aut voluptas dicta cum voluptatem consequuntur. Aut rerum rerum sed iure perferendis rem optio earum et voluptate totam. </p><p>Ut dicta omnis et laboriosam repellat eos assumenda dolore est dolor temporibus aut porro eligendi qui error molestiae! At sint natus quo omnis mollitia et optio quisquam et natus nihil qui consequatur maiores. Hic repellendus consequatur et consequatur nisi qui adipisci voluptatem. Et nisi molestiae aut internos tenetur quo similique odio?",
                ImageUrl = "https://placehold.co/600x400",
                CategoryName = "Winter Dish"
            });

        }
    }
}
