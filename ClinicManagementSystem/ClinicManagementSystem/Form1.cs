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


namespace ClinicManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = ClinicManagementSystem.Properties.Resources.ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand command = con.CreateCommand();
            command.CommandText = "SELECT user_id FROM [user] WHERE user_username=@username AND user_password=@password";
            command.Parameters.AddWithValue("@username", richTextBox1.Text);
            command.Parameters.AddWithValue("@password", richTextBox2.Text);
            con.Open();
            var result = command.ExecuteScalar();
            if (result != null)
            {
                // authenticated
                if(richTextBox1.Text == "admin")
                {
                    // admin panel
                    Hide();
                    AdminPanel admin = new AdminPanel();
                    admin.ShowDialog();
                    Show();
                  //  con.Close();
                }
                else
                {
                //    con.Open();
                    command.CommandText = "SELECT account_id, account_type FROM account WHERE account_user_id=@user_id";
                    command.Parameters.AddWithValue("@user_id", result.ToString());
                    SqlDataReader reader = command.ExecuteReader();
                    
                    if(reader.Read())
                    {
                        int account_id = reader.GetInt32(0);
                        int account_type = reader.GetInt32(1);
                        con.Close();

                        if (account_type == 2)
                        {
                            //Secertary panel
                            Hide();
                            SecretaryPanel secretary = new SecretaryPanel(account_id);
                            secretary.ShowDialog();
                            Show();

                        }
                        else if(account_type == 3)
                        {
                            // Doctor panel
                            Hide();
                            DoctorPanel doctor = new DoctorPanel(account_id);
                            doctor.ShowDialog();
                            Show();
                        }
                        else
                        {
                            MessageBox.Show("else");
                        }

                    }

                }
            }
            else
            {
                // Authentication error
                MessageBox.Show("Authentication Failed!");
            }
        }
    }
}
