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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }
        
        static public void load_mol()                   // функция для отображения информации
        {
            Form3.ds.Tables["MOL"].Clear();
            Form3.strSQL = " SELECT id_mol AS '№_Ответсвенного лица', " +
                           " name AS 'ФИО', adres AS 'Адрес', tel AS 'Телефон' FROM mol ";
            Form3.SQLAdapter = new SqlDataAdapter(Form3.strSQL, Form3.cn);

            Form3.SQLAdapter.Fill(Form3.ds, "MOL");

            Form3.bs_mol.DataSource = Form3.ds.Tables["MOL"];
            Program.F7.dataGridView4.DataSource = Form3.bs_mol;
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            //
            // --- [ ЗАГРУЗКА ] ---   ЛИЦА (МОЛ) ----------------------------------------------------
            Form3.ds.Tables.Add("MOL");
            load_mol();
            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            textBox_mol_fio.DataBindings.Add(new Binding("Text", Form3.bs_mol, "ФИО", false, DataSourceUpdateMode.Never));
            textBox_mol_adres.DataBindings.Add(new Binding("Text", Form3.bs_mol, "Адрес", false, DataSourceUpdateMode.Never));
            textBox_mol_tel.DataBindings.Add(new Binding("Text", Form3.bs_mol, "Телефон", false, DataSourceUpdateMode.Never));
            // --------------------------------------------------------------------------------------

        }

        private void button12_Click(object sender, EventArgs e)
        {
            // --- [ ДОБАВЛЕНИЕ ] ---  ЛИЦА

            // проверим все поля на заполненность
            if (textBox_mol_fio.Text == "" || textBox_mol_adres.Text == "" || textBox_mol_tel.Text == "")
            {
                MessageBox.Show("Заполните все поля", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // запрос на добавление
            Form3.strSQL = "INSERT INTO mol VALUES (@NAME, @TEL, @AD)";

            // работаем через адаптер и свойство добавления
            Form3.SQLAdapter.InsertCommand = new SqlCommand(Form3.strSQL, Form3.cn);  // новая команда создана
            // определим параметры и зададим им значения
            Form3.SQLAdapter.InsertCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_mol_fio.Text;
            Form3.SQLAdapter.InsertCommand.Parameters.Add("@TEL", SqlDbType.VarChar).Value = textBox_mol_tel.Text;
            Form3.SQLAdapter.InsertCommand.Parameters.Add("@AD", SqlDbType.VarChar).Value = textBox_mol_adres.Text;
            try
            {
                Form3.SQLAdapter.InsertCommand.ExecuteNonQuery(); // выполним запрос
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

        private void button11_Click(object sender, EventArgs e)
        {
            // --- [ ОБНОВЛЕНИЕ ] ---   ЛИЦА

            if (Form3.ds.Tables["MOL"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                // проверим все поля на заполненность
                if (textBox_mol_fio.Text == "" || textBox_mol_adres.Text == "" || textBox_mol_tel.Text == "")
                {
                    MessageBox.Show("Заполните все поля", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // запрос на обновление
                Form3.strSQL = " UPDATE mol SET name = @NAME, tel = @TEL, adres = @AD " +
                         " WHERE id_mol = @ID_M ";

                Form3.SQLAdapter.UpdateCommand = new SqlCommand(Form3.strSQL, Form3.cn);  // команда для обноления создана
                // зададим значения параметрам 
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@NAME", SqlDbType.VarChar).Value = textBox_mol_fio.Text;
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@TEL", SqlDbType.VarChar).Value = textBox_mol_tel.Text;
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@AD", SqlDbType.VarChar).Value = textBox_mol_adres.Text;
                Form3.SQLAdapter.UpdateCommand.Parameters.Add("@ID_M", SqlDbType.Int).Value =
                    Convert.ToInt32(Form3.ds.Tables["MOL"].Rows[dataGridView4.CurrentRow.Index][0]);
                try
                {
                    Form3.SQLAdapter.UpdateCommand.ExecuteNonQuery(); // выполним запрос
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

        private void button10_Click(object sender, EventArgs e)
        {
            // --- [ УДАЛЕНИЕ ВЫБРАННЫХ ] ---   ЛИЦА

            if (Form3.ds.Tables["MOL"].Rows.Count > 0)              // проверка на наличие строк в таблице
            {
                Form3.strSQL = " DELETE FROM mol WHERE id_mol = @ID_M ";

                Form3.SQLAdapter.DeleteCommand = new SqlCommand(Form3.strSQL, Form3.cn);
                // Если нажата кномка да, удаления не избежать.
                if (DialogResult.Yes == MessageBox.Show("Вы уверены в удалении? \nЗаписей:  "
                    + dataGridView4.SelectedRows.Count.ToString(), "Удаление", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    try
                    {
                        foreach (DataGridViewRow drv in dataGridView4.SelectedRows)
                        {
                            Form3.SQLAdapter.DeleteCommand.Parameters.Add("@ID_M", SqlDbType.Int).Value =
                                Convert.ToInt32(Form3.ds.Tables["MOL"].Rows[drv.Index][0]);

                            Form3.SQLAdapter.DeleteCommand.ExecuteNonQuery();
                            Form3.SQLAdapter.DeleteCommand.Parameters.Clear();
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
    }
}
