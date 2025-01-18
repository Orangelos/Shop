using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
	public class Сотрудник
	{
		[Key]
		public int ID_Сотрудника { get; set; }
		public string? Имя { get; set; }
		public string? Фамилия { get; set; }
		public string? Должность { get; set; }
		public string? ЭлектроннаяПочта { get; set; }
		public string? НомерТелефона { get; set; }
		public string? ДатаНаима { get; set; }
        public string? Photo { get; set; }

        [ForeignKey("ОтделениеБанка")]
        public int ID_Отделения{ get; set; }
        public virtual ОтделениеБанка? ОтделениеБанка { get; set; }


    }
}
