using Microsoft.EntityFrameworkCore;

namespace Bank.Models
{
	public class BankContext : DbContext
	{
		public BankContext(DbContextOptions<BankContext> options) : base(options)
		{
		}

        public DbSet<Banner> Banner { get; set; }
        public DbSet<FeedBack> FeedBack { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Karzina> Karzina { get; set; }
        public DbSet<Клиент> Клиенты { get; set; }
		public DbSet<Счет> Счета { get; set; }
		public DbSet<Транзакция> Транзакции { get; set; }
		public DbSet<Кредит> Кредиты { get; set; }
		public DbSet<ПлатежПоКредиту> ПлатежиПоКредитам { get; set; }
		public DbSet<ОтделениеБанка> ОтделенияБанков { get; set; }
		public DbSet<Сотрудник> Сотрудники { get; set; }
	}
}
