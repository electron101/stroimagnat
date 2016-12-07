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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        public void Form6_Load(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
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
            Form3.strSQL = "INSERT INTO postav VALUES (@NAME, @AD, @TEL, @BANK)";

            // работаем через адаптер и свойство добавления
            Form3.SQLAdapter.InsertCommand = new SqlCommand(Form3.strSQL, Form3.cn);  // новая команда создана
            // определим параметры и зададим им значения
            Form3.SQLAdapter.InsertCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_post_name.Text;
            Form3.SQLAdapter.InsertCommand.Parameters.Add("@AD", SqlDbType.VarChar).Value = textBox_post_adres.Text;
            Form3.SQLAdapter.InsertCommand.Parameters.Add("@TEL", SqlDbType.VarChar).Value = textBox_post_tel.Text;
            Form3.SQLAdapter.InsertCommand.Parameters.Add("@BANK", SqlDbType.VarChar).Value = textBox_post_bank.Text;
            try
            {
                Form3.SQLAdapter.InsertCommand.ExecuteNonQuery(); // выполним запрос
                // если удачно то...
                Form3.load_post();           // обновим таблицу
                MessageBox.Show("Успешно добавлен!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // если не удачно обшика с инфой
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // --- [ ОБНОВЛЕНИЕ ] ---   ПОСТАВЩИКИ

            if (Form3.ds.Tables["POST"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                // проверим все поля на заполненность
                if (textBox_post_name.Text == "" || textBox_post_adres.Text == ""
                    || textBox_post_tel.Text == "" || textBox_post_bank.Text == "")
                {
                    MessageBox.Show("Заполните все поля", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // запрос на обновление
                Form3.strSQL = " UPDATE postav SET name = @NAME, adres = @AD, tel = @TEL, bank_schet = @BANK " +
                         " WHERE id_post = @ID_P ";

                Form3.SQLAdapter.UpdateCommand = new SqlCommand(Form3.strSQL, Form3.cn);  // команда для обноления создана
                // зададим значения параметрам 
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_post_name.Text;
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@AD", SqlDbType.VarChar).Value = textBox_post_adres.Text;
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@TEL", SqlDbType.VarChar).Value = textBox_post_tel.Text;
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@BANK", SqlDbType.VarChar).Value = textBox_post_bank.Text;
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                    Convert.ToInt32(Form3.ds.Tables["POST"].Rows[dataGridView2.CurrentRow.Index][0]);
                try
                {
                    Form3.SQLAdapter.UpdateCommand.ExecuteNonQuery(); // выполним запрос
                    // если удачно то...
                    Form3.load_post();           // обновим таблицу
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

        private void button4_Click(object sender, EventArgs e)
        {
            // --- [ УДАЛЕНИЕ ВЫБРАННЫХ ] ---   ПОСТАВЩИКИ

            if (Form3.ds.Tables["POST"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                Form3.strSQL = " DELETE FROM postav WHERE id_post = @ID_P ";

                Form3.SQLAdapter.DeleteCommand = new SqlCommand(Form3.strSQL, Form3.cn);
                // Если нажата кномка да, удаления не избежать.
                if (DialogResult.Yes == MessageBox.Show("Вы уверены в удалении? \nЗаписей:  "
                    + dataGridView2.SelectedRows.Count.ToString(), "Удаление", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    try
                    {
                        foreach (DataGridViewRow drv in dataGridView2.SelectedRows)
                        {
                            Form3.SQLAdapter.DeleteCommand.Parameters.Add("@ID_P", SqlDbType.Int).Value =
                                Convert.ToInt32(Form3.ds.Tables["POST"].Rows[drv.Index][0]);

                            Form3.SQLAdapter.DeleteCommand.ExecuteNonQuery();
                            Form3.SQLAdapter.DeleteCommand.Parameters.Clear();
                        }
                        Form3.load_post();           // обновим таблицу
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