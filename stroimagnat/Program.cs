using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;

namespace stroimagnat
{
    static class Program
    {
		static public Form1 F1;         // приход материала
		static public Form2 F2;         // отпуск материала
		static public Form4 F4;         // материал
		static public Form5 F5;         // история выдачи
		static public Form6 F6;         // поставщики
		static public Form7 F7;         // ответственные
        static public Form3 F3;         // главная форма


        // Функция выведения форм по центру экрана
        public static void center_form(Form F)
        {
            int SW = Screen.PrimaryScreen.Bounds.Width;     // ширина разрешения
            int SH = Screen.PrimaryScreen.Bounds.Height;    // высота разрешения

            F.Location = new Point((SW / 2 - F.Width / 2), (SH / 2 - F.Height / 2));
        }
	
	
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            			
			F3 = new Form3();       // создание экземпляра формы
            F3.Show();              // сразу отображает форму при запуске

            Application.Run();
        }
    }
}
