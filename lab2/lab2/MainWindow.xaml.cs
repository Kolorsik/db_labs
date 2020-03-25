using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace lab2
{
    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable employeeTable;
        SqlConnection connection = null;

        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Connection();
            UpdateDB();
        }

        private void RefreshBut_Click(object sender, RoutedEventArgs e)
        {
            UpdateDB();
        }

        private void Connection()
        {
            try
            {
                employeeTable = new DataTable();
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Update()
        {
            string sql = string.Format("Update Employee Set Name = '{0}' Where IdEmpoyee = '{1}'", nameBox.Text, idText.Text);
            using (SqlCommand cmd = new SqlCommand(sql, this.connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void Delete()
        {
            string sql = string.Format("Delete from Employee where IdEmpoyee = '{0}'", idText.Text);
            using (SqlCommand cmd = new SqlCommand(sql, this.connection))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Id is incorrect!", ex);
                    throw error;
                }
            }
        }

        private void Add()
        {
            string sql = string.Format("Insert Into Employee" + "(IdEmpoyee, Name, IdSportsFfculty) Values(@IdEmp, @Name, @IdSp)");

            using (SqlCommand cmd = new SqlCommand(sql, this.connection))
            {
                // Добавить параметры
                cmd.Parameters.AddWithValue("@IdEmp", idText.Text);
                cmd.Parameters.AddWithValue("@Name", nameBox.Text);
                cmd.Parameters.AddWithValue("@IdSp", idfacBox.Text);

                cmd.ExecuteNonQuery();
            }
    }

        private void UpdateDB()
        {
            DataTable dt = new DataTable();
            string sql = "select * from Employee";
            using (SqlCommand cmd = new SqlCommand(sql, this.connection))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                dr.Close();
            }
            employeeGrid.ItemsSource = dt.DefaultView;
        }

        private void DeleteBut_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            UpdateDB();
        }

        private void UpdateBut_Click(object sender, RoutedEventArgs e)
        {
            Update();
            UpdateDB();
        }

        private void AddBut_Click(object sender, RoutedEventArgs e)
        {
            Add();
            UpdateDB();
        }
    }
}