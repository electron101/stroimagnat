using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop;


namespace stroimagnat
{
    public partial class Form3 : Form
    {
        static public SqlConnection cn;
        static public DataSet ds = new DataSet();
        static public SqlDataAdapter SQLAdapter = new SqlDataAdapter(strSQL, cn);
        static public string strSQL;

        static public BindingSource bs_product = new BindingSource();
        BindingSource bs_post = new BindingSource();
        BindingSource bs_mol = new BindingSource();
        BindingSource bs_prihod = new BindingSource();
        BindingSource bs_otpusk = new BindingSource();
        BindingSource bs_spid_vid = new BindingSource();
        BindingSource bs_rashod = new BindingSource();
        
        //// для перемещиния формы ----------
        //private Point mouseOffset;
        //private bool isMouseDown = false;
        //// --------------------------------

        public Form3()
        {
            InitializeComponent();
        }

        void load_product()                   // функция для отображения информации о продукте в datagridview
        {
            ds.Tables["PRODUCT"].Clear();
            strSQL = "SELECT id_product AS '№_Продукта', name AS 'Наименование', ed_izmer AS 'Единица измерения', cena AS 'Цена' FROM product";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "PRODUCT");

            bs_product.DataSource = ds.Tables["PRODUCT"];
            dataGridView1.DataSource = bs_product;
        }

        void load_post()                   // функция для отображения информации
        {
            ds.Tables["POST"].Clear();
            strSQL = " SELECT id_post AS '№_Поставщика', name AS 'Наименование', adres AS 'Адрес', " +
                     " tel AS 'Телефон', bank_schet AS 'Банковский счёт' FROM postav";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "POST");

            bs_post.DataSource = ds.Tables["POST"];
            dataGridView2.DataSource = bs_post;
        }

        void load_mol()                   // функция для отображения информации
        {
            ds.Tables["MOL"].Clear();
            strSQL = " SELECT id_mol AS '№_Ответсвенного лица', name AS 'ФИО', adres AS 'Адрес', tel AS 'Телефон' FROM mol";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "MOL");

            bs_mol.DataSource = ds.Tables["MOL"];
            dataGridView4.DataSource = bs_mol;
        }

        void load_prihod()                   // функция для отображения информации
        {
            ds.Tables["PRIHOD"].Clear();
            strSQL = " SELECT prihod.id_prihod AS '№_Прихода', postav.name AS 'Поставщик', mol.name AS 'Ответственный', " +
                     " product.name AS 'Продукт', prihod.kolvo AS 'Количество', prihod.cena_zakup AS 'Цена закупочная', " +
                     " prihod.date_time AS 'Дата_прихода' FROM prihod JOIN postav ON prihod.id_post = postav.id_post " +
                     " JOIN mol ON prihod.id_mol = mol.id_mol JOIN product ON prihod.id_product = product.id_product";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "PRIHOD");

            bs_prihod.DataSource = ds.Tables["PRIHOD"];
            dataGridView3.DataSource = bs_prihod;
        }


        void load_otpusk()                   // функция для отображения информации
        {
            ds.Tables["OTPUSK"].Clear();
            strSQL = " SELECT nalichie.id_product AS '№_Продукта', product.name AS 'Наименование', " +
                     " nalichie.kolvo AS 'Количество', product.ed_izmer AS 'Единица измерения' FROM nalichie " +
                     " JOIN product ON nalichie.id_product = product.id_product WHERE nalichie.kolvo > 0";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "OTPUSK");

            bs_otpusk.DataSource = ds.Tables["OTPUSK"];
            dataGridView5.DataSource = bs_otpusk;
        }

        void load_rashod()                   // функция для отображения информации
        {
            ds.Tables["RASHOD"].Clear();
            strSQL = " SELECT rashod.id_rashod AS '№_Расхода', product.id_product AS 'Продукт', " +
                     " rashod.kolvo AS 'Количество', rashod.date_time AS 'Дата_выдачи' FROM rashod " +
                     " JOIN product ON rashod.id_product = product.id_product";
            SQLAdapter = new SqlDataAdapter(strSQL, cn);

            SQLAdapter.Fill(ds, "RASHOD");

            bs_rashod.DataSource = ds.Tables["RASHOD"];
            dataGridView7.DataSource = bs_rashod;
        }


        private void Form3_Load(object sender, EventArgs e)
        {
            Program.center_form(Program.F3);

            SqlConnectionStringBuilder bdr = new SqlConnectionStringBuilder();
            bdr.DataSource = @".\SQLExpress";
            bdr.InitialCatalog = "vit";
            bdr.IntegratedSecurity = true;

            cn = new SqlConnection(bdr.ConnectionString);
            try
            {
                cn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            label17.Text = "Приход материала";
            panel3.Dock = DockStyle.Fill;

            //
            // --- [ ЗАГРУЗКА ] ---   ПРОДУКТЫ ------------------------------------------------------
            ds.Tables.Add("PRODUCT");
            load_product();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            textBox_prod_name.DataBindings.Add(new Binding("Text", bs_product, "Наименование", false, DataSourceUpdateMode.Never));
            textBox_prod_ediz.DataBindings.Add(new Binding("Text", bs_product, "Единица измерения", false, DataSourceUpdateMode.Never));
            textBox_prod_cena.DataBindings.Add(new Binding("Text", bs_product, "Цена", false, DataSourceUpdateMode.Never));

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   ПОСТАВЩИКИ ----------------------------------------------------
            ds.Tables.Add("POST");
            load_post();
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            textBox_post_name.DataBindings.Add(new Binding("Text", bs_post, "Наименование", false, DataSourceUpdateMode.Never));
            textBox_post_adres.DataBindings.Add(new Binding("Text", bs_post, "Адрес", false, DataSourceUpdateMode.Never));
            textBox_post_tel.DataBindings.Add(new Binding("Text", bs_post, "Телефон", false, DataSourceUpdateMode.Never));
            textBox_post_bank.DataBindings.Add(new Binding("Text", bs_post, "Банковский счёт", false, DataSourceUpdateMode.Never));

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   ЛИЦА (МОЛ) ----------------------------------------------------
            ds.Tables.Add("MOL");
            load_mol();
            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            textBox_mol_fio.DataBindings.Add(new Binding("Text", bs_mol, "ФИО", false, DataSourceUpdateMode.Never));
            textBox_mol_adres.DataBindings.Add(new Binding("Text", bs_mol, "Адрес", false, DataSourceUpdateMode.Never));
            textBox_mol_tel.DataBindings.Add(new Binding("Text", bs_mol, "Телефон", false, DataSourceUpdateMode.Never));

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   ПРИХОД ПРОДУКТОВ ----------------------------------------------
            ds.Tables.Add("PRIHOD");
            load_prihod();
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            comboBox_prod_pri.DataBindings.Add(new Binding("Text", bs_prihod, "Продукт", false, DataSourceUpdateMode.Never));
            comboBox_post_pri.DataBindings.Add(new Binding("Text", bs_prihod, "Поставщик", false, DataSourceUpdateMode.Never));
            comboBox_mol_pri.DataBindings.Add(new Binding("Text", bs_prihod, "Ответственный", false, DataSourceUpdateMode.Never));
            numericUpDown_pri_kolvo.DataBindings.Add(new Binding("Value", bs_prihod, "Количество", false, DataSourceUpdateMode.Never));
            textBox_pri_cena.DataBindings.Add(new Binding("Text", bs_prihod, "Цена закупочная", false, DataSourceUpdateMode.Never));

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   ОТПУСК ПРОДУКТОВ ----------------------------------------------
            try
            {
                ds.Tables.Add("OTPUSK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            load_otpusk();
            dataGridView5.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   СПИСОК ВЫДАЧИ -------------------------------------------------
            ds.Tables.Add("SPIS_VID");
            dataGridView6.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // добавляем нужные столбцы в ручную
            ds.Tables["SPIS_VID"].Columns.Add("№_Продукта");
            ds.Tables["SPIS_VID"].Columns.Add("Наименование");
            ds.Tables["SPIS_VID"].Columns.Add("Колличество");
            ds.Tables["SPIS_VID"].Columns.Add("Единица измерения");

            bs_spid_vid.DataSource = ds.Tables["SPIS_VID"];
            dataGridView6.DataSource = bs_spid_vid;

            // --------------------------------------------------------------------------------------

            //
            // --- [ ЗАГРУЗКА ] ---   ВЫДАННЫЕ ПРОДУКТЫ ---------------------------------------------
            ds.Tables.Add("RASHOD");
            load_rashod();
            dataGridView7.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView7.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // --------------------------------------------------------------------------------------

            comboBox_prod_pri.DataSource = bs_product;
            comboBox_prod_pri.DisplayMember = "Наименование";
            comboBox_prod_pri.ValueMember = "№_Продукта";

            comboBox_post_pri.DataSource = bs_post;
            comboBox_post_pri.DisplayMember = "Наименование";
            comboBox_post_pri.ValueMember = "№_Поставщика";

            comboBox_mol_pri.DataSource = bs_mol;
            comboBox_mol_pri.DisplayMember = "ФИО";
            comboBox_mol_pri.ValueMember = "№_Ответсвенного лица";


        }
                
        //// для перемещения формы мышью ----------------------------------------------
        ////
        //private void Form3_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        isMouseDown = false;
        //    }
        //}
       
        //private void Form3_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (isMouseDown)
        //    {
        //        Point mousePos = Control.MousePosition;
        //        mousePos.Offset(mouseOffset.X, mouseOffset.Y);
        //        Location = mousePos;
        //    }
        //}
       
        //private void Form3_MouseDown(object sender, MouseEventArgs e)
        //{
        //    int xOffset;
        //    int yOffset;

        //    if (e.Button == MouseButtons.Left)
        //    {
        //        xOffset = -e.X - SystemInformation.FrameBorderSize.Width;
        //        yOffset = -e.Y - SystemInformation.CaptionHeight -
        //            SystemInformation.FrameBorderSize.Height;
        //        mouseOffset = new Point(xOffset, yOffset);
        //        isMouseDown = true;
        //    }
        //}
      
        //private void Form3_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Escape) this.Close();
        //}

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label17.Text = (sender as Button).Text;
            // запустить форму прихода материалов
            panel3.Visible = false;
            panel1.Dock = DockStyle.Fill;
            panel1.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label17.Text = (sender as Button).Text;
            // запустить форму материалов
            panel3.Visible = false;
            panel4.Dock = DockStyle.Fill;
            panel4.Visible = true;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            label17.Text = (sender as Button).Text;
            // запустить форму отпуска материалов
            panel3.Visible = false;
            panel2.Dock = DockStyle.Fill;
            panel2.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label17.Text = (sender as Button).Text;
            // запустить форму истории выдачи
            panel3.Visible = false;
            panel5.Dock = DockStyle.Fill;
            panel5.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label17.Text = (sender as Button).Text;
            // запустить форму поставщиков
            panel3.Visible = false;
            panel6.Dock = DockStyle.Fill;
            panel6.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label17.Text = (sender as Button).Text;
            // запустить форму ответственных
            panel3.Visible = false;
            panel7.Dock = DockStyle.Fill;
            panel7.Visible = true;
        }

        bool filter = false;  // переменная для заголовка в отчёте в excel была ли фильтрация или нет 
        private void button23_Click(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = bs_prihod;
            // Выводи все записи с... по...

            bs.Filter = string.Format(" CONVERT(Дата_прихода, 'System.DateTime') >= '{0:dd.MM.yyyy}' AND CONVERT(Дата_прихода, 'System.DateTime') <= '{1:dd.MM.yyyy}'",
            dateTimePicker4.Value.ToShortDateString(), dateTimePicker3.Value.ToShortDateString());

            filter = true;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = bs_prihod;
            bs.RemoveFilter();

            filter = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // --- [ УДАЛЕНИЕ ВЫБРАННЫХ ] ---   ПРИЁМ ПРОДУКТОВ

            if (ds.Tables["PRIHOD"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                strSQL = " DELETE FROM prihod WHERE id_prihod = @ID_P ";

                string strSQLU = " UPDATE nalichie SET kolvo = kolvo - @KOLVO WHERE id_product = @ID_PR ";

                SQLAdapter.DeleteCommand = new SqlCommand(strSQL, cn);

                SQLAdapter.UpdateCommand = new SqlCommand(strSQLU, cn);

                // Если нажата кномка да, удаления не избежать.
                if (DialogResult.Yes == MessageBox.Show("Вы уверены в удалении? \nЗаписей:  "
                    + dataGridView3.SelectedRows.Count.ToString(), "Удаление", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    try
                    {
                        foreach (DataGridViewRow drv in dataGridView3.SelectedRows)
                        {
                            int ID_prod = 0;
                            strSQL = "SELECT id_product AS 'ID' FROM prihod WHERE id_prihod = @ID_P";
                            using (SqlCommand cm = new SqlCommand(strSQL, cn))
                            {
                                cm.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                                    Convert.ToInt32(ds.Tables["PRIHOD"].Rows[drv.Index][0]);
                                try
                                {
                                    using (SqlDataReader rd = cm.ExecuteReader())
                                    {
                                        while (rd.Read())
                                        {
                                            ID_prod = Convert.ToInt32(rd["ID"]);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }

                            SQLAdapter.DeleteCommand.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                                Convert.ToInt32(ds.Tables["PRIHOD"].Rows[drv.Index][0]);
                            SQLAdapter.DeleteCommand.ExecuteNonQuery();
                            SQLAdapter.DeleteCommand.Parameters.Clear();
                            try
                            {
                                SQLAdapter.UpdateCommand.Parameters.Add("@ID_PR", SqlDbType.Int).Value = ID_prod;
                                SQLAdapter.UpdateCommand.Parameters.Add("@KOLVO", SqlDbType.Int).Value =
                                    Convert.ToInt32(ds.Tables["PRIHOD"].Rows[drv.Index][4]);
                                SQLAdapter.UpdateCommand.ExecuteNonQuery();
                                SQLAdapter.UpdateCommand.Parameters.Clear();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        load_prihod();           // обновим таблицу
                        MessageBox.Show("Успешно удалено!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
                MessageBox.Show("Таблица пуста", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            // В EXCEL на печать --------------------------------------------------------------------------------
            //
            // объект класса для запуска Excel из программы
            Microsoft.Office.Interop.Excel.Application ExApp = new Microsoft.Office.Interop.Excel.Application();

            ExApp.Workbooks.Add(Type.Missing); // создаю рабочую книгу

            ExApp.Worksheets.get_Item(1);       // создаю лист 1

            // шапка корзины
            for (int k = 0; k < dataGridView3.ColumnCount; k++)
                ExApp.Cells[2, k + 1] = dataGridView3.Columns[k].Name.ToString();

            int i = 0;
            int j = 0;
            decimal count = 0;
            for (i = 0; i < dataGridView3.RowCount; i++)
            {
                for (j = 0; j < dataGridView3.ColumnCount; j++)
                    ExApp.Cells[i + 3, j + 1] = dataGridView3.Rows[i].Cells[j].Value.ToString();

                count = count + (Convert.ToInt32(dataGridView3.Rows[i].Cells[4].Value) * Convert.ToDecimal(dataGridView3.Rows[i].Cells[5].Value));
            }

            ExApp.Rows[1].Font.Bold = true;         // первая строка будет жирным шрифтом
            ExApp.Rows[i + 3].Font.Bold = true;     // последняя строка будет жирым шрифтом

            ExApp.Range[ExApp.Cells[i + 3, 1], ExApp.Cells[i + 3, j - 1]].Merge(); // объединяет ячейки последней строки

            ExApp.Cells[i + 3, 1] = "Итого (руб)";  // Итого:
            ExApp.Cells[i + 3, j] = count;   // итоговая сумма, i + 2, т.к. прибовляем шапку и новую строку

            ExApp.Range[ExApp.Cells[1, 1], ExApp.Cells[1, j]].Merge(); // объединяет ячейки первой строки
            if (filter)
                ExApp.Cells[1, 1] = "Очёт по приходу товара с   " + dateTimePicker4.Text + "    по  " + dateTimePicker3.Text;
            else
                ExApp.Cells[1, 1] = "Очёт по приходу товара за всё время";

            /*
            ExApp.Worksheets.PrintPreview();
            ExApp.ActiveSheet.PrintOut(1, 2, 1, false, false, true, false, false);
                    
            ExApp.Range[ExApp.Cells[1, 1], ExApp.Cells[i, j]].Select();
            ExApp.ActiveSheet.PrintOut(1, 2, 1, false, false, true, false, false);
            */
            //((Microsoft.Office.Interop.Excel.Worksheet)ExApp.ActiveSheet).PrintOut
            //(1, 1, 2, true);

            ExApp.Columns.EntireColumn.AutoFit();   // ширина столбцов растягивается по содержимому

            ExApp.Visible = true;   // показать созданный документ


            //
            // --------------------------------------------------------------------------------------------------

        }

        private void button9_Click(object sender, EventArgs e)
        {
            // --- [ ДОБАВЛЕНИЕ ] ---  ПРИЁМ ПРОДУКТОВ

            // проверим все поля на заполненность
            if (textBox_pri_cena.Text == "" || comboBox_prod_pri.Text == ""
                || comboBox_post_pri.Text == "" || comboBox_mol_pri.Text == "" || numericUpDown_pri_kolvo.Value <= 0)
            {
                MessageBox.Show("Заполните все поля\nКолличество не может быть равным 0 или быть отрицательным",
                    "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // есть ли такой продукт в наличии если есть то в таблицу наличие не будем заносить а будем
            // изменять его колличество иначе внесём 
            int kolvo = 0;
            strSQL = "SELECT count(*) AS 'count' FROM nalichie WHERE id_product = @ID_P";
            using (SqlCommand cm = new SqlCommand(strSQL, cn))
            {
                cm.Parameters.Add("@ID_P", SqlDbType.Int).Value = comboBox_prod_pri.SelectedValue;
                try
                {
                    using (SqlDataReader rd = cm.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            kolvo = Convert.ToInt32(rd["count"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // запрос на добавление
            strSQL = " INSERT INTO prihod VALUES (@ID_POST, @ID_MOL, @ID_PROD, @KOLVO, @CENA, @DATE) ";

            // работаем через адаптер и свойство добавления
            SQLAdapter.InsertCommand = new SqlCommand(strSQL, cn);  // новая команда создана
            // определим параметры и зададим им значения
            SQLAdapter.InsertCommand.Parameters.Add("@ID_POST", SqlDbType.Int).Value = comboBox_post_pri.SelectedValue;
            SQLAdapter.InsertCommand.Parameters.Add("@ID_MOL", SqlDbType.Int).Value = comboBox_mol_pri.SelectedValue;
            SQLAdapter.InsertCommand.Parameters.Add("@ID_PROD", SqlDbType.Int).Value = comboBox_prod_pri.SelectedValue;
            SQLAdapter.InsertCommand.Parameters.Add("@KOLVO", SqlDbType.Int).Value = numericUpDown_pri_kolvo.Value;
            SQLAdapter.InsertCommand.Parameters.Add("@CENA", SqlDbType.Decimal).Value = textBox_pri_cena.Text;
            SQLAdapter.InsertCommand.Parameters.Add("@DATE", SqlDbType.DateTime).Value = DateTime.Now;

            try
            {
                SQLAdapter.InsertCommand.ExecuteNonQuery(); // выполним запрос
                // если удачно то...
                if (kolvo != 0)
                {
                    strSQL = " UPDATE nalichie SET kolvo = kolvo + @KOLVO WHERE id_product = @ID_PROD";
                    SQLAdapter.UpdateCommand = new SqlCommand(strSQL, cn);  // новая команда создана
                    SQLAdapter.UpdateCommand.Parameters.Add("@ID_PROD", SqlDbType.Int).Value = comboBox_prod_pri.SelectedValue;
                    SQLAdapter.UpdateCommand.Parameters.Add("@KOLVO", SqlDbType.Int).Value = numericUpDown_pri_kolvo.Value;
                    try
                    {
                        SQLAdapter.UpdateCommand.ExecuteNonQuery(); // выполним запрос
                    }
                    catch (Exception ex)
                    {
                        // если не удачно обшика с инфой
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    strSQL = " INSERT INTO nalichie VALUES (@ID_PROD, @KOLVO) ";
                    SQLAdapter.InsertCommand = new SqlCommand(strSQL, cn);  // новая команда создана
                    SQLAdapter.InsertCommand.Parameters.Add("@ID_PROD", SqlDbType.Int).Value = comboBox_prod_pri.SelectedValue;
                    SQLAdapter.InsertCommand.Parameters.Add("@KOLVO", SqlDbType.Int).Value = numericUpDown_pri_kolvo.Value;
                    try
                    {
                        SQLAdapter.InsertCommand.ExecuteNonQuery(); // выполним запрос
                    }
                    catch (Exception ex)
                    {
                        // если не удачно обшика с инфой
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                load_prihod();           // обновим таблицу
                MessageBox.Show("Успешно добавлен!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // если не удачно обшика с инфой
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            // --- [ ОБНОВЛЕНИЕ ] ---   ПРИЁМ ПРОДУКТОВ

            if (ds.Tables["PRIHOD"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                // проверим все поля на заполненность
                if (textBox_pri_cena.Text == "" || comboBox_prod_pri.Text == ""
                || comboBox_post_pri.Text == "" || comboBox_mol_pri.Text == "" || numericUpDown_pri_kolvo.Value <= 0)
                {
                    MessageBox.Show("Заполните все поля", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // вытащим прежнее значение количество товара из прихода перед тем как заменить его новым
                int kolvo = 0;
                strSQL = "SELECT kolvo AS 'kolvo' FROM prihod WHERE id_prihod = @ID_P";
                using (SqlCommand cm = new SqlCommand(strSQL, cn))
                {
                    cm.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                        Convert.ToInt32(ds.Tables["PRIHOD"].Rows[dataGridView3.CurrentRow.Index][0]);
                    try
                    {
                        using (SqlDataReader rd = cm.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                kolvo = Convert.ToInt32(rd["kolvo"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // расчитаем разницу между старым и новым значением колличества продукта перед тем как изменить 
                // значение количества в таблице наличие
                kolvo = kolvo - Convert.ToInt32(numericUpDown_pri_kolvo.Value);

                // запрос на обновление
                strSQL = " UPDATE prihod SET id_post = @ID_POST, id_mol = @ID_MOL, id_product = @ID_PROD, " +
                         " kolvo = @KOLVO, cena_zakup = @CENA, date_time = @DATE WHERE id_prihod = @ID_P; " +
                         " UPDATE nalichie SET kolvo = kolvo - @KOLVO_DELTA WHERE id_product = @ID_PROD";

                SQLAdapter.UpdateCommand = new SqlCommand(strSQL, cn);  // команда для обноления создана
                // зададим значения параметрам 
                SQLAdapter.UpdateCommand.Parameters.Add("@ID_POST", SqlDbType.Int).Value = comboBox_post_pri.SelectedValue;
                SQLAdapter.UpdateCommand.Parameters.Add("@ID_MOL", SqlDbType.Int).Value = comboBox_mol_pri.SelectedValue;
                SQLAdapter.UpdateCommand.Parameters.Add("@ID_PROD", SqlDbType.Int).Value = comboBox_prod_pri.SelectedValue;
                SQLAdapter.UpdateCommand.Parameters.Add("@KOLVO", SqlDbType.Int).Value = numericUpDown_pri_kolvo.Value;
                SQLAdapter.UpdateCommand.Parameters.Add("@KOLVO_DELTA", SqlDbType.Int).Value = kolvo;
                SQLAdapter.UpdateCommand.Parameters.Add("@CENA", SqlDbType.Decimal).Value = textBox_pri_cena.Text;
                SQLAdapter.UpdateCommand.Parameters.Add("@DATE", SqlDbType.DateTime).Value = DateTime.Now;
                SQLAdapter.UpdateCommand.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                    Convert.ToInt32(ds.Tables["PRIHOD"].Rows[dataGridView3.CurrentRow.Index][0]);
                try
                {
                    SQLAdapter.UpdateCommand.ExecuteNonQuery(); // выполним запрос
                    // если удачно то...
                    load_prihod();           // обновим таблицу
                    MessageBox.Show("Запись успешно обновлена!", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // если запрос выполнился не удачно то ошибка с инфой
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Таблица пуста", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // --- [ ДОБАВЛЕНИЕ В СПИСОК ВЫДАЧИ ] ---   

            if (ds.Tables["OTPUSK"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                // если количество больше 0
                if (Convert.ToInt32(ds.Tables["OTPUSK"].Rows[dataGridView5.CurrentRow.Index][2]) > 0)
                {
                    // если количесвто добовляемых меньше или = количеству в наличии
                    if (Convert.ToInt32(numericUpDown_vid.Value) <=
                        Convert.ToInt32(ds.Tables["OTPUSK"].Rows[dataGridView5.CurrentRow.Index][2]))
                    {
                        if (ds.Tables["SPIS_VID"].Rows.Count > 0)    // елси в списке выдачи есть хотя бы один продукт
                        {
                            bool _id = false;
                            // пройдёмся по всей таблице списка выдачи
                            for (int i = 0; i < ds.Tables["SPIS_VID"].Rows.Count; i++)
                            {
                                // если эта книга уже есть в списке выдачи
                                if (Convert.ToInt32(ds.Tables["OTPUSK"].Rows[dataGridView5.CurrentRow.Index][0]) ==
                                    Convert.ToInt32(ds.Tables["SPIS_VID"].Rows[i][0]))
                                {
                                    // увеличим колличество продукта не добавляя новую строку
                                    ds.Tables["SPIS_VID"].Rows[i][2] =
                                        Convert.ToInt32(ds.Tables["SPIS_VID"].Rows[i][2]) +
                                        Convert.ToInt32(numericUpDown_vid.Value);
                                    _id = true;             // ID найден, количество увеличено
                                    break;
                                }
                            }

                            if (_id == false)   // если продукт не был в списке выдачи то добавим его
                                ds.Tables["SPIS_VID"].Rows.Add(ds.Tables["OTPUSK"].Rows[dataGridView5.CurrentRow.Index][0],
                                                           ds.Tables["OTPUSK"].Rows[dataGridView5.CurrentRow.Index][1],
                                                           numericUpDown_vid.Value,
                                                           ds.Tables["OTPUSK"].Rows[dataGridView5.CurrentRow.Index][3]);
                        }
                        else        // добавим книгку
                            ds.Tables["SPIS_VID"].Rows.Add(ds.Tables["OTPUSK"].Rows[dataGridView5.CurrentRow.Index][0],
                                                           ds.Tables["OTPUSK"].Rows[dataGridView5.CurrentRow.Index][1],
                                                           numericUpDown_vid.Value,
                                                           ds.Tables["OTPUSK"].Rows[dataGridView5.CurrentRow.Index][3]);

                        // уменьшим колличество в наличии на велечину только что добавленного продукта
                        ds.Tables["OTPUSK"].Rows[dataGridView5.CurrentRow.Index][2] =
                                Convert.ToInt32(ds.Tables["OTPUSK"].Rows[dataGridView5.CurrentRow.Index][2]) -
                                Convert.ToInt32(numericUpDown_vid.Value);
                    }
                    else
                        MessageBox.Show("Столько нет!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Этоого продукта сейчас нет в наличии", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Таблица пуста", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            // --- [ УДАЛЕНИЕ ИЗ СПИСОКА ВЫДАЧИ ] ---

            if (ds.Tables["SPIS_VID"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                // если колличество продуктов в списке выдачи больше 0
                if (Convert.ToInt32(ds.Tables["SPIS_VID"].Rows[dataGridView6.CurrentRow.Index][2]) > 0)
                {
                    // если удаляемых продуктов меньше или = чем в наличии в списке выдачи
                    if (Convert.ToInt32(numericUpDown_vid2.Value) <=
                        Convert.ToInt32(ds.Tables["SPIS_VID"].Rows[dataGridView6.CurrentRow.Index][2]))
                    {
                        if (ds.Tables["OTPUSK"].Rows.Count > 0)    // елси в таблице поиска есть хотя бы один продукт
                        {
                            // идём по всей таблице отпуска
                            for (int i = 0; i < ds.Tables["OTPUSK"].Rows.Count; i++)
                            {
                                // если такой продукт есть сразу в дувх таблицах
                                if (Convert.ToInt32(ds.Tables["SPIS_VID"].Rows[dataGridView6.CurrentRow.Index][0]) ==
                                    Convert.ToInt32(ds.Tables["OTPUSK"].Rows[i][0]))
                                {
                                    // добавим к количеству продукта в отпуске количество только что удалённое из списка выдачи
                                    ds.Tables["OTPUSK"].Rows[i][2] =
                                        Convert.ToInt32(ds.Tables["OTPUSK"].Rows[i][2]) +
                                        Convert.ToInt32(numericUpDown_vid2.Value);
                                    // уменьшим количесвто в списке выдачи на удалённое 
                                    ds.Tables["SPIS_VID"].Rows[dataGridView6.CurrentRow.Index][2] =
                                        Convert.ToInt32(ds.Tables["SPIS_VID"].Rows[dataGridView6.CurrentRow.Index][2]) -
                                        Convert.ToInt32(numericUpDown_vid2.Value);

                                    break;
                                }
                            }
                        }
                        // если у продукта не осталось количества то удалить запись

                        if (Convert.ToInt32(ds.Tables["SPIS_VID"].Rows[dataGridView6.CurrentRow.Index][2]) == 0)
                            ds.Tables["SPIS_VID"].Rows.RemoveAt(dataGridView6.CurrentRow.Index);
                    }
                    else
                        MessageBox.Show("Столько нет!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Таблица пуста", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (ds.Tables["SPIS_VID"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                strSQL = " INSERT INTO rashod VALUES (@ID_P, @KOLVO, @DATE_VID); " +
                         " UPDATE nalichie SET kolvo -= @KOLVO WHERE id_product = @ID_P; ";

                using (SqlCommand cm = new SqlCommand(strSQL, cn))
                {
                    try
                    {
                        for (int i = 0; i < ds.Tables["SPIS_VID"].Rows.Count; i++)
                        {
                            cm.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                                Convert.ToInt32(ds.Tables["SPIS_VID"].Rows[i][0]);
                            cm.Parameters.Add("@DATE_VID", SqlDbType.DateTime).Value = DateTime.Now;
                            cm.Parameters.Add("@KOLVO", SqlDbType.Int).Value =
                                Convert.ToInt32(ds.Tables["SPIS_VID"].Rows[i][2]);

                            try
                            {
                                cm.ExecuteNonQuery();
                                cm.Parameters.Clear();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                MessageBox.Show("Все продукты выданы!", "Выдача", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ds.Tables["SPIS_VID"].Rows.Clear();
                load_prihod();
            }
            else
                MessageBox.Show("Нет продуктов для выдачи", "Выдача", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            // фильтрация по дате
            //
            BindingSource bs = new BindingSource();
            bs.DataSource = bs_rashod;

            // Выводи все записи с... по...

            bs.Filter = string.Format(" CONVERT(Дата_выдачи, 'System.DateTime') >= '{0:dd.MM.yyyy}' AND CONVERT(Дата_выдачи, 'System.DateTime') <= '{1:dd.MM.yyyy}'",
            dateTimePicker1.Value.ToShortDateString(), dateTimePicker2.Value.ToShortDateString());
        }

        private void button21_Click(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = bs_rashod;
            bs.RemoveFilter();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            // --- [ ДОБАВЛЕНИЕ ] ---  ПРОДУКТЫ

            // проверим все поля на заполненность
            if (textBox_prod_name.Text == "" || textBox_prod_ediz.Text == "" || textBox_prod_cena.Text == "")
            {
                MessageBox.Show("Заполните все поля", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // запрос на добавление
            strSQL = "INSERT INTO product VALUES (@NAME, @ED, @CENA)";

            // работаем через адаптер и свойство добавления
            SQLAdapter.InsertCommand = new SqlCommand(strSQL, cn);  // новая команда создана
            // определим параметры и зададим им значения
            SQLAdapter.InsertCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_prod_name.Text;
            SQLAdapter.InsertCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = textBox_prod_ediz.Text;
            SQLAdapter.InsertCommand.Parameters.Add("@CENA", SqlDbType.Decimal).Value = textBox_prod_cena.Text;
            try
            {
                SQLAdapter.InsertCommand.ExecuteNonQuery(); // выполним запрос
                // если удачно то...
                load_product();           // обновим таблицу
                MessageBox.Show("Успешно добавлен!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // если не удачно обшика с инфой
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // --- [ ОБНОВЛЕНИЕ ] ---   ПРОДУКТЫ

            if (ds.Tables["PRODUCT"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                // проверим все поля на заполненность
                if (textBox_prod_name.Text == "" || textBox_prod_ediz.Text == "" || textBox_prod_cena.Text == "")
                {
                    MessageBox.Show("Заполните все поля", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // запрос на обновление
                strSQL = " UPDATE product SET name = @NAME, ed_izmer = @ED, cena = @CENA " +
                         " WHERE id_product = @ID_P ";

                SQLAdapter.UpdateCommand = new SqlCommand(strSQL, cn);  // команда для обноления создана
                // зададим значения параметрам 
                SQLAdapter.UpdateCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_prod_name.Text;
                SQLAdapter.UpdateCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = textBox_prod_ediz.Text;
                SQLAdapter.UpdateCommand.Parameters.Add("@CENA", SqlDbType.Decimal).Value = textBox_prod_cena.Text;
                SQLAdapter.UpdateCommand.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                    Convert.ToInt32(ds.Tables["PRODUCT"].Rows[dataGridView1.CurrentRow.Index][0]);
                try
                {
                    SQLAdapter.UpdateCommand.ExecuteNonQuery(); // выполним запрос
                    // если удачно то...
                    load_product();           // обновим таблицу
                    MessageBox.Show("Запись успешно обновлена!", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // если запрос выполнился не удачно то ошибка с инфой
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Таблица пуста", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // --- [ УДАЛЕНИЕ ВЫБРАННЫХ ] ---   ПРОДУКТЫ

            if (ds.Tables["PRODUCT"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                strSQL = " DELETE FROM product WHERE id_product = @ID_P ";

                SQLAdapter.DeleteCommand = new SqlCommand(strSQL, cn);
                // Если нажата кномка да, удаления не избежать.
                if (DialogResult.Yes == MessageBox.Show("Вы уверены в удалении? \nЗаписей:  "
                    + dataGridView1.SelectedRows.Count.ToString(), "Удаление", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    try
                    {
                        foreach (DataGridViewRow drv in dataGridView1.SelectedRows)
                        {
                            SQLAdapter.DeleteCommand.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                                Convert.ToInt32(ds.Tables["PRODUCT"].Rows[drv.Index][0]);

                            SQLAdapter.DeleteCommand.ExecuteNonQuery();
                            SQLAdapter.DeleteCommand.Parameters.Clear();
                        }
                        load_product();           // обновим таблицу
                        MessageBox.Show("Успешно удалено!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
                MessageBox.Show("Таблица пуста", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            // --- [ ДОБАВЛЕНИЕ ] ---  ПОСТАВЩИКИ

            // проверим все поля на заполненность
            if (textBox_post_name.Text == "" || textBox_post_adres.Text == ""
                || textBox_post_tel.Text == "" || textBox_post_bank.Text == "")
            {
                MessageBox.Show("Заполните все поля", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // запрос на добавление
            strSQL = "INSERT INTO postav VALUES (@NAME, @AD, @TEL, @BANK)";

            // работаем через адаптер и свойство добавления
            SQLAdapter.InsertCommand = new SqlCommand(strSQL, cn);  // новая команда создана
            // определим параметры и зададим им значения
            SQLAdapter.InsertCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_post_name.Text;
            SQLAdapter.InsertCommand.Parameters.Add("@AD", SqlDbType.VarChar).Value = textBox_post_adres.Text;
            SQLAdapter.InsertCommand.Parameters.Add("@TEL", SqlDbType.VarChar).Value = textBox_post_tel.Text;
            SQLAdapter.InsertCommand.Parameters.Add("@BANK", SqlDbType.VarChar).Value = textBox_post_bank.Text;
            try
            {
                SQLAdapter.InsertCommand.ExecuteNonQuery(); // выполним запрос
                // если удачно то...
                load_post();           // обновим таблицу
                MessageBox.Show("Успешно добавлен!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // если не удачно обшика с инфой
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            // --- [ ОБНОВЛЕНИЕ ] ---   ПОСТАВЩИКИ

            if (ds.Tables["POST"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                // проверим все поля на заполненность
                if (textBox_post_name.Text == "" || textBox_post_adres.Text == ""
                    || textBox_post_tel.Text == "" || textBox_post_bank.Text == "")
                {
                    MessageBox.Show("Заполните все поля", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // запрос на обновление
                strSQL = " UPDATE postav SET name = @NAME, adres = @AD, tel = @TEL, bank_schet = @BANK " +
                         " WHERE id_post = @ID_P ";

                SQLAdapter.UpdateCommand = new SqlCommand(strSQL, cn);  // команда для обноления создана
                // зададим значения параметрам 
                SQLAdapter.UpdateCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_post_name.Text;
                SQLAdapter.UpdateCommand.Parameters.Add("@AD", SqlDbType.VarChar).Value = textBox_post_adres.Text;
                SQLAdapter.UpdateCommand.Parameters.Add("@TEL", SqlDbType.VarChar).Value = textBox_post_tel.Text;
                SQLAdapter.UpdateCommand.Parameters.Add("@BANK", SqlDbType.VarChar).Value = textBox_post_bank.Text;
                SQLAdapter.UpdateCommand.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                    Convert.ToInt32(ds.Tables["POST"].Rows[dataGridView2.CurrentRow.Index][0]);
                try
                {
                    SQLAdapter.UpdateCommand.ExecuteNonQuery(); // выполним запрос
                    // если удачно то...
                    load_post();           // обновим таблицу
                    MessageBox.Show("Запись успешно обновлена!", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // если запрос выполнился не удачно то ошибка с инфой
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Таблица пуста", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            // --- [ УДАЛЕНИЕ ВЫБРАННЫХ ] ---   ПОСТАВЩИКИ

            if (ds.Tables["POST"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                strSQL = " DELETE FROM postav WHERE id_post = @ID_P ";

                SQLAdapter.DeleteCommand = new SqlCommand(strSQL, cn);
                // Если нажата кномка да, удаления не избежать.
                if (DialogResult.Yes == MessageBox.Show("Вы уверены в удалении? \nЗаписей:  "
                    + dataGridView2.SelectedRows.Count.ToString(), "Удаление", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    try
                    {
                        foreach (DataGridViewRow drv in dataGridView2.SelectedRows)
                        {
                            SQLAdapter.DeleteCommand.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                                Convert.ToInt32(ds.Tables["POST"].Rows[drv.Index][0]);

                            SQLAdapter.DeleteCommand.ExecuteNonQuery();
                            SQLAdapter.DeleteCommand.Parameters.Clear();
                        }
                        load_post();           // обновим таблицу
                        MessageBox.Show("Успешно удалено!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
                MessageBox.Show("Таблица пуста", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            // --- [ ДОБАВЛЕНИЕ ] ---  ЛИЦА

            // проверим все поля на заполненность
            if (textBox_mol_fio.Text == "" || textBox_mol_adres.Text == "" || textBox_mol_tel.Text == "")
            {
                MessageBox.Show("Заполните все поля", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // запрос на добавление
            strSQL = "INSERT INTO mol VALUES (@NAME, @TEL, @AD)";

            // работаем через адаптер и свойство добавления
            SQLAdapter.InsertCommand = new SqlCommand(strSQL, cn);  // новая команда создана
            // определим параметры и зададим им значения
            SQLAdapter.InsertCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_mol_fio.Text;
            SQLAdapter.InsertCommand.Parameters.Add("@TEL", SqlDbType.VarChar).Value = textBox_mol_tel.Text;
            SQLAdapter.InsertCommand.Parameters.Add("@AD", SqlDbType.VarChar).Value = textBox_mol_adres.Text;
            try
            {
                SQLAdapter.InsertCommand.ExecuteNonQuery(); // выполним запрос
                // если удачно то...
                load_mol();           // обновим таблицу
                MessageBox.Show("Успешно добавлен!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // если не удачно обшика с инфой
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            // --- [ ОБНОВЛЕНИЕ ] ---   ЛИЦА

            if (ds.Tables["MOL"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                // проверим все поля на заполненность
                if (textBox_mol_fio.Text == "" || textBox_mol_adres.Text == "" || textBox_mol_tel.Text == "")
                {
                    MessageBox.Show("Заполните все поля", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // запрос на обновление
                strSQL = " UPDATE mol SET name = @NAME, tel = @TEL, adres = @AD " +
                         " WHERE id_mol = @ID_M ";

                SQLAdapter.UpdateCommand = new SqlCommand(strSQL, cn);  // команда для обноления создана
                // зададим значения параметрам 
                SQLAdapter.UpdateCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_mol_fio.Text;
                SQLAdapter.UpdateCommand.Parameters.Add("@TEL", SqlDbType.VarChar).Value = textBox_mol_tel.Text;
                SQLAdapter.UpdateCommand.Parameters.Add("@AD", SqlDbType.VarChar).Value = textBox_mol_adres.Text;
                SQLAdapter.UpdateCommand.Parameters.Add("@ID_M", SqlDbType.Int).Value =
                    Convert.ToInt32(ds.Tables["MOL"].Rows[dataGridView4.CurrentRow.Index][0]);
                try
                {
                    SQLAdapter.UpdateCommand.ExecuteNonQuery(); // выполним запрос
                    // если удачно то...
                    load_mol();           // обновим таблицу
                    MessageBox.Show("Запись успешно обновлена!", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // если запрос выполнился не удачно то ошибка с инфой
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Таблица пуста", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            // --- [ УДАЛЕНИЕ ВЫБРАННЫХ ] ---   ЛИЦА

            if (ds.Tables["MOL"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                strSQL = " DELETE FROM mol WHERE id_mol = @ID_M ";

                SQLAdapter.DeleteCommand = new SqlCommand(strSQL, cn);
                // Если нажата кномка да, удаления не избежать.
                if (DialogResult.Yes == MessageBox.Show("Вы уверены в удалении? \nЗаписей:  "
                    + dataGridView4.SelectedRows.Count.ToString(), "Удаление", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    try
                    {
                        foreach (DataGridViewRow drv in dataGridView4.SelectedRows)
                        {
                            SQLAdapter.DeleteCommand.Parameters.Add("@ID_M", SqlDbType.Int).Value =
                                Convert.ToInt32(ds.Tables["MOL"].Rows[drv.Index][0]);

                            SQLAdapter.DeleteCommand.ExecuteNonQuery();
                            SQLAdapter.DeleteCommand.Parameters.Clear();
                        }
                        load_mol();           // обновим таблицу
                        MessageBox.Show("Успешно удалено!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
                MessageBox.Show("Таблица пуста", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            label17.Text = "Главная";
            panel3.Visible = true;
            panel3.Dock = DockStyle.Fill;
            panel1.Visible = false;
            panel2.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}