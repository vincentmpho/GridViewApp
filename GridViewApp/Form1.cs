using Microsoft.VisualBasic.ApplicationServices;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GridViewApp
{
    public partial class Form1 : Form
    {
        
        SqlConnection connection;
        SqlDataAdapter adapter; 
        SqlCommandBuilder cmdBuilder;
        DataSet ds = new DataSet();
        DataSet changes;
        DataTable dataTable; 
        string Sql;
        Int32 i;
         

        public Form1()
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source=LKALULED\\SQLEXPRESS;Initial Catalog=studentdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");

            dataTable = new DataTable();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
          
            SqlCommand cmd = new SqlCommand("insert into students (first_name, last_name, email, gender) values (@first_name, @last_name, @email, @gender)", connection);

            connection.Open();
                
                cmd.Parameters.Add("@first_name", SqlDbType.VarChar).Value = textBox1.Text.ToString();
                cmd.Parameters.Add("@last_name", SqlDbType.VarChar).Value = textBox2.Text.ToString();
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = textBox3.Text.ToString();
                cmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = comboBox1.SelectedItem?.ToString();

                int result = cmd.ExecuteNonQuery();
            connection.Close();
                if (result == 1)
                {
                    LoadGridView();
                    MessageBox.Show( "Record saved", "Message",MessageBoxButtons.YesNo);
                    Clear();


                }
                else
                {
                    MessageBox.Show("Record Was not Saved","Error",  MessageBoxButtons.OKCancel);
                }
        }

        public void LoadGridView()
        {
             
            Sql = "select * from students";
            try
            {
                dataTable = new DataTable();
                cmdBuilder = new SqlCommandBuilder(adapter);
                connection.Open();
                adapter = new SqlDataAdapter(Sql, connection);
                adapter.Fill(dataTable);
                connection.Close();
                dataGridView1.DataSource = dataTable;
                 
                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                adapter.Update(dataTable);
               
                MessageBox.Show("Changes Done","Message",  MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             
            // Populate DataTable from the database
            string selectQuery = "SELECT * from students";
            adapter = new SqlDataAdapter(selectQuery, connection);
            cmdBuilder = new SqlCommandBuilder(adapter);
            adapter.Fill(dataTable);

            // Bind DataTable to the DataGridView
            dataGridView1.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                SqlCommand cmd = new SqlCommand("update  students set first_name = @first_name, last_name=@last_name, email=@email, gender=@gender where id=@id", connection);

                connection.Open();

                cmd.Parameters.Add("@first_name", SqlDbType.VarChar).Value = textBox1.Text.ToString();
                cmd.Parameters.Add("@last_name", SqlDbType.VarChar).Value = textBox2.Text.ToString();
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = textBox3.Text.ToString();
                cmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = comboBox1.SelectedItem?.ToString();
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 2;

                int result = cmd.ExecuteNonQuery();
                connection.Close();
                if (result == 1)
                {
                    LoadGridView();
                    MessageBox.Show("Record saved", "Message", MessageBoxButtons.YesNo);
                    Clear();
                }
                else
                {
                    MessageBox.Show("Record Was not Saved", "Error", MessageBoxButtons.OKCancel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong", "Error");
               
            }
          
        }

        private void Clear()
        {
            textBox2.Text = string.Empty;
            textBox2.Text = "";
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
        }
    }
}