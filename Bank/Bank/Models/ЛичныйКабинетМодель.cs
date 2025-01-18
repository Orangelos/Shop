namespace Bank.Models
{
    public class ЛичныйКабинетМодель
    {
        public Клиент? Клиент { get; set; }
        public string? Имя { get; set; }
        public string? Фамилия { get; set; }
        public string? ЭлектроннаяПочта { get; set; }
        public DateTime ДатаРождения { get; set; }
        public List<Счет>? Счета { get; set; }
        public List<Кредит>? Кредиты { get; set; }

        public string? otdel { get; set; }
        // Новое поле: Корзина
        public List<Karzina>? Корзина { get; set; }
    }

    //public class СчетViewModel
    //{
    //    public int ID_Счета { get; set; }
    //    public string ТипСчета { get; set; }
    //    public decimal Баланс { get; set; }
    //    public DateTime ДатаСоздания { get; set; }
    //}
    //
    //public class КредитViewModel
    //{
    //    public int ID_Кредита { get; set; }
    //    public string ТипКредита { get; set; }
    //    public decimal ОсновнаяСумма { get; set; }
    //    public decimal ПроцентнаяСтавка { get; set; }
    //    public string Статус { get; set; }
    //    public DateTime ДатаНачала { get; set; }
    //    public DateTime ДатаОкончания { get; set; }
    //}
}
