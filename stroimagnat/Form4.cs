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

namespace stroimagnat
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        static public void load_product()                   // функция для отображения информации о продукте в datagridview
        {
            Form3.ds.Tables["PRODUCT"].Clear();
            Form3.strSQL = "SELECT id_product AS '№_Продукта', name AS 'Наименование', ed_izmer AS 'Единица измерения', cena AS 'Цена' FROM product";
            Form3.SQLAdapter = new SqlDataAdapter(Form3.strSQL, Form3.cn);

            Form3.SQLAdapter.Fill(Form3.ds, "PRODUCT");

            Form3.bs_product.DataSource = Form3.ds.Tables["PRODUCT"];
            Program.F4.dataGridView1.DataSource = Form3.bs_product;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            //
            // --- [ ЗАГРУЗКА ] ---   ПРОДУКТЫ ------------------------------------------------------
            Form3.ds.Tables.Add("PRODUCT");
            load_product();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            textBox_prod_name.DataBindings.Add(new Binding("Text", Form3.bs_product, "Наименование", false, DataSourceUpdateMode.Never));
            textBox_prod_ediz.DataBindings.Add(new Binding("Text", Form3.bs_product, "Единица измерения", false, DataSourceUpdateMode.Never));
            textBox_prod_cena.DataBindings.Add(new Binding("Text", Form3.bs_product, "Цена", false, DataSourceUpdateMode.Never));

            // --------------------------------------------------------------------------------------
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // --- [ ДОБАВЛЕНИЕ ] ---  ПРОДУКТЫ

            // проверим все поля на заполненность
            if (textBox_prod_name.Text == "" || textBox_prod_ediz.Text == "" || textBox_prod_cena.Text == "")
            {
                MessageBox.Show("Заполните все поля", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // запрос на добавление
            Form3.strSQL = "INSERT INTO product VALUES (@NAME, @ED, @CENA)";

            // работаем через адаптер и свойство добавления
            Form3.SQLAdapter.InsertCommand = new SqlCommand(Form3.strSQL, Form3.cn);  // новая команда создана
            // определим параметры и зададим им значения
            Form3.SQLAdapter.InsertCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_prod_name.Text;
            Form3.SQLAdapter.InsertCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = textBox_prod_ediz.Text;
            Form3.SQLAdapter.InsertCommand.Parameters.Add("@CENA", SqlDbType.Decimal).Value = textBox_prod_cena.Text;
            try
            {
                Form3.SQLAdapter.InsertCommand.ExecuteNonQuery(); // выполним запрос
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

        private void button2_Click(object sender, EventArgs e)
        {
            // --- [ ОБНОВЛЕНИЕ ] ---   ПРОДУКТЫ

            if (Form3.ds.Tables["PRODUCT"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                // проверим все поля на заполненность
                if (textBox_prod_name.Text == "" || textBox_prod_ediz.Text == "" || textBox_prod_cena.Text == "")
                {
                    MessageBox.Show("Заполните все поля", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // запрос на обновление
                Form3.strSQL = " UPDATE product SET name = @NAME, ed_izmer = @ED, cena = @CENA " +
                         " WHERE id_product = @ID_P ";

                Form3.SQLAdapter.UpdateCommand = new SqlCommand(Form3.strSQL, Form3.cn);  // команда для обноления создана
                // зададим значения параметрам 
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_prod_name.Text;
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = textBox_prod_ediz.Text;
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@CENA", SqlDbType.Decimal).Value = textBox_prod_cena.Text;
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                    Convert.ToInt32(Form3.ds.Tables["PRODUCT"].Rows[dataGridView1.CurrentRow.Index][0]);
                try
                {
                    Form3.SQLAdapter.UpdateCommand.ExecuteNonQuery(); // выполним запрос
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

        private void button3_Click(object sender, EventArgs e)
        {
            // --- [ УДАЛЕНИЕ ВЫБРАННЫХ ] ---   ПРОДУКТЫ

            if (Form3.ds.Tables["PRODUCT"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                Form3.strSQL = " DELETE FROM product WHERE id_product = @ID_P ";

                Form3.SQLAdapter.DeleteCommand = new SqlCommand(Form3.strSQL, Form3.cn);
                // Если нажата кномка да, удаления не избежать.
                if (DialogResult.Yes == MessageBox.Show("Вы уверены в удалении? \nЗаписей:  "
                    + dataGridView1.SelectedRows.Count.ToString(), "Удаление", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    try
                    {
                        foreach (DataGridViewRow drv in dataGridView1.SelectedRows)
                        {
                            Form3.SQLAdapter.DeleteCommand.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                                Convert.ToInt32(Form3.ds.Tables["PRODUCT"].Rows[drv.Index][0]);

                            Form3.SQLAdapter.DeleteCommand.ExecuteNonQuery();
                            Form3.SQLAdapter.DeleteCommand.Parameters.Clear();
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
    }
}