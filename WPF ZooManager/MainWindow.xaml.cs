using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace WPF_ZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["WPF_ZooManager.Properties.Settings.PanjuTutorialsDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            ShowZoos();
            ShowAnimals();
        }

        private void ShowZoos()
        {

            try
            {
                string query = "Select * from Zoo";
                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);

                    //Which information of the Table in DataTable should be shown in our ListBox?
                    listZoos.DisplayMemberPath = "Location";
                    // Which value should be delivered, when an Item from our ListBox is selected?
                    listZoos.SelectedValuePath = "Id";
                    //The Reference to the Data the ListBox should populate
                    listZoos.ItemsSource = zooTable.DefaultView;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }

        private void ShowAssociatedAnimals()
        {
           try
            {
                string query = "Select * from Animal a inner join ZooAnimal za on a.Id = za.AnimalId where za.ZooId = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    //Which information of the Table in DataTable should be shown in our ListBox?
                    listAssociatedAnimals.DisplayMemberPath = "Name";
                    // Which value should be delivered, when an Item from our ListBox is selected?
                    listAssociatedAnimals.SelectedValuePath = "Id";
                    //The Reference to the Data the ListBox should populate
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;
                }

            }
            catch (Exception ex)
            {
              //  MessageBox.Show(ex.ToString());
            }

        }


        private void ShowAnimals()
        {

            try
            {
                string query = "Select * from Animal";
                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    //Which information of the Table in DataTable should be shown in our ListBox?
                    // using hardcoded string names below is a potential source of problem for the program!!!
                    listAllAnimals.DisplayMemberPath = "Name";
                    // Which value should be delivered, when an Item from our ListBox is selected?
                    listAllAnimals.SelectedValuePath = "Id";
                    //The Reference to the Data the ListBox should populate
                    listAllAnimals.ItemsSource = animalTable.DefaultView;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // MessageBox.Show(listZoos.SelectedValue.ToString());
           ShowAssociatedAnimals();
            ShowSelectedZooInTextBox();
            ShowSelectedAnimalInTextBox();
        }

        private void Delete_Content_Copy1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Zoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Zoo where id = @ZooId";
                // below is an alternative approach to using the sqlCommand code from the sqlAdapter (not as quick and easy) it requires opening and closing as opposed to the adapter.
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
                
                // We want the associated animals to be deleted  from the zoo as well in the ZooAnimal table. This is done be adding ON DELETE CASCADE
                // to the constraints in the sql script on the ZooAnimal Design window.
            }
            catch (Exception exdeletezoo)
            {
                MessageBox.Show(exdeletezoo.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
            
            
        }

        private void AddZoo_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Zoo values (@Location)";
                // below is an alternative approach to using the sqlCommand code from the sqlAdapter (not as quick and easy) it requires opening and closing as opposed to the adapter.
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", My_Text_Box.Text);
                sqlCommand.ExecuteScalar();

                // We want the associated animals to be deleted  from the zoo as well in the ZooAnimal table. This is done be adding ON DELETE CASCADE
                // to the constraints in the sql script on the ZooAnimal Design window.
            }
            catch (Exception exAddZoo)
            {
                MessageBox.Show(exAddZoo.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        private void Delete_Animal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Animal where id = @Name";
                // below is an alternative approach to using the sqlCommand code from the sqlAdapter (not as quick and easy) it requires opening and closing as opposed to the adapter.
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();

                // We want the associated animals to be deleted  from the zoo as well in the ZooAnimal table. This is done be adding ON DELETE CASCADE
                // to the constraints in the sql script on the ZooAnimal Design window.
            }
            catch (Exception exdeleteanimal)
            {
                MessageBox.Show(exdeleteanimal.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimals();
            }


        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Animal values (@Name)";
                // below is an alternative approach to using the sqlCommand code from the sqlAdapter (not as quick and easy) it requires opening and closing as opposed to the adapter.
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", My_Text_Box.Text);
                sqlCommand.ExecuteScalar();

                // We want the associated animals to be deleted  from the zoo as well in the ZooAnimal table. This is done be adding ON DELETE CASCADE
                // to the constraints in the sql script on the ZooAnimal Design window.
            }
            catch (Exception exAddAnimal)
            {
                MessageBox.Show(exAddAnimal.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimals();
            }
        }

        private void addAnimalToZoo_Click( object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into ZooAnimal values (@ZooId, @AnimalId)";
                // below is an alternative approach to using the sqlCommand code from the sqlAdapter (not as quick and easy) it requires opening and closing as opposed to the adapter.
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();

                // We want the associated animals to be deleted  from the zoo as well in the ZooAnimal table. This is done be adding ON DELETE CASCADE
                // to the constraints in the sql script on the ZooAnimal Design window.
            }
            catch (Exception exAddZoo)
            {
                MessageBox.Show(exAddZoo.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();

            }
        }

        private void ShowSelectedZooInTextBox()
        {
            try
            {
                string query = "select location from Zoo where Id =  @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable zooDataTable = new DataTable();

                    sqlDataAdapter.Fill(zooDataTable);

                  My_Text_Box.Text = zooDataTable.Rows[0]["Location"].ToString();
                }

            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.ToString());
            }
        }

        private void ShowSelectedAnimalInTextBox()
        {
            try
            {
                string query = "select name from Animal where Id =  @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);

                    DataTable animalDataTable = new DataTable();

                    sqlDataAdapter.Fill(animalDataTable);

                    My_Text_Box.Text = animalDataTable.Rows[0]["Name"].ToString();
                }

            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.ToString());
            }
        }

        private void updateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Zoo Set Location = @Location where Id = @ZooId";
                // below is an alternative approach to using the sqlCommand code from the sqlAdapter (not as quick and easy) it requires opening and closing as opposed to the adapter.
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", My_Text_Box.Text);
                sqlCommand.ExecuteScalar();

                // We want the associated animals to be deleted  from the zoo as well in the ZooAnimal table. This is done be adding ON DELETE CASCADE
                // to the constraints in the sql script on the ZooAnimal Design window.
            }
            catch (Exception exAddZoo)
            {
                MessageBox.Show(exAddZoo.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();

            }
        }

        private void updateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Animal Set Name = @Name where Id = @AnimalId";
                // below is an alternative approach to using the sqlCommand code from the sqlAdapter (not as quick and easy) it requires opening and closing as opposed to the adapter.
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", My_Text_Box.Text);
                sqlCommand.ExecuteScalar();

                // We want the associated animals to be deleted  from the zoo as well in the ZooAnimal table. This is done be adding ON DELETE CASCADE
                // to the constraints in the sql script on the ZooAnimal Design window.
            }
            catch (Exception exAddZoo)
            {
                MessageBox.Show(exAddZoo.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimals();

            }
        }

    }

}

