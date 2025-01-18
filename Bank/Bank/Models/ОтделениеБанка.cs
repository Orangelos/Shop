using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
	public class ОтделениеБанка
	{
		[Key]
		public int ID_Отделения { get; set; }
		public string? НазваниеОтделения { get; set; }
		public string? Адрес { get; set; }
		public string? НомерТелефона { get; set; }
        public string? Photo { get; set; }

        
        //public virtual ICollection<Сотрудник> Сотрудники { get; set; }
       // public virtual ICollection<Клиент> Клиенты { get; set; }

    }
}
