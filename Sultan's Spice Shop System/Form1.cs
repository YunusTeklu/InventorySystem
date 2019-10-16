using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sultan_s_Spice_Shop_System
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            //skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panelInventoryList.Visible = false;
            
            //To read inventory name and assign to combo box items
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
               Path.DirectorySeparatorChar + "Inventory.txt";
            if (File.Exists(path))
            {
                using (StreamReader fileReader = new StreamReader(path))
                {
                    String newLine;
                    while ((newLine = fileReader.ReadLine()) != null)
                    {
                        String[] values = newLine.Split(',');
                        comboBoxNameList.Items.Add(values[0]);
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            String unitCost = txtUnitCost.Text;
            String unitPrice = txtUnitPrice.Text;
            String amount = txtAmountAquired.Text;
            double amountNum = Double.Parse(amount);
            String itemName = null;
            String cmbItemName = null;

            //To construct a pth to Inventory.txt file
            String path =Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                Path.DirectorySeparatorChar+"Inventory.txt";

            //To capture the date the inventory is recorded
            String shipmentDate=DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            //Create a file if it does not exist
            if (!File.Exists(path))
                File.Create(path).Dispose();

            //To take data either from the itemlist or item name textfield
            if (checkBoxItemExcist.Checked == true)
            {
                //ArrayList lines =new ArrayList();
                String[] lines = new String[100];
                using (StreamReader fileRead = new StreamReader(path))
                {
                    cmbItemName = comboBoxNameList.Text;
                    string newline;
                    int i = 0;
                    //Read all data from the file and update the selected one
                    while ((newline = fileRead.ReadLine()) != null)
                    {
                        lines[i] = newline;
                        String[] values = newline.Split(',');
                        
                        if (values[0] == cmbItemName)
                        {
                            values[1] = unitCost;
                            values[2] = unitPrice;
                            double valNum = Double.Parse(values[3]);
                            valNum += amountNum;
                            values[3] = valNum.ToString();
                            values[4] = shipmentDate;
                            lines[i] = values[0] + "," + values[1] + "," + values[2] + "," + values[3] + "," + values[4];
                        }
                        i++;
                    }
                }
                
                //Over ride ebtire file with updated data
                using (StreamWriter fileOverRide = new StreamWriter(path))
                {
                    foreach (String s in lines)
                    {
                        fileOverRide.WriteLine(s);
                    }
                }
            }
            else
            {
                //To insert a new csv record
                String csvData;
                using (StreamWriter fileWrite = new StreamWriter(path, true))
                {
                    itemName = txtItemName.Text;
                    csvData = itemName + "," + unitCost + "," + unitPrice + "," + amount + "," + shipmentDate;
                    fileWrite.WriteLine(csvData);
                }
                
            }
            panelInventoryList.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panelSalesList.Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panelSalesList.Visible = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panelDailyReport.Visible = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            panelDailyReport.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBoxItemExcist.Checked = true;
            txtItemName.Enabled = false;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panelHome.Visible = true;
            panelReport.Visible = true;
            panelSales.Visible = true;
            panelInventory.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panelHome.Visible = false;
            panelReport.Visible = false;
            panelSales.Visible = false;
            panelInventory.Visible = true;

            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                Path.DirectorySeparatorChar + "Inventory.txt";
            using (StreamReader file= new StreamReader(path))
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Item Name");
                dt.Columns.Add("Unit Cost");
                dt.Columns.Add("Unit Price");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Date Aquired");

                string newline;
                while ((newline = file.ReadLine()) != null)
                {
                    DataRow dr = dt.NewRow();
                    string[] values = newline.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        dr[i] = values[i];
                    }
                    dt.Rows.Add(dr);
                }
                dataGridView1.DataSource = dt;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panelHome.Visible = false;
            panelReport.Visible = false;
            panelSales.Visible = true;
            panelInventory.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panelHome.Visible = false;
            panelReport.Visible = true;
            panelSales.Visible = true;
            panelInventory.Visible = true;
        }

        private void checkBoxItemExcist_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxItemExcist.Checked == true)
            {
                txtItemName.Enabled = false;
                comboBoxNameList.Enabled = true;
            }
            else
            {
                comboBoxNameList.Enabled = false;
                txtItemName.Enabled = true;
            }
                
        }

        
        /*To Read from file and display all in DataGridView
        System.IO.StreamReader file = new System.IO.StreamReader("yourfile.txt");
        string[] columnnames = file.ReadLine().Split(' ');
        DataTable dt = new DataTable();
        foreach (string c in columnnames)
        {
            dt.Columns.Add(c);
        }
        string newline;
        while ((newline = file.ReadLine()) != null)
        {
            DataRow dr = dt.NewRow();
            string[] values = newline.Split(' ');
            for (int i = 0; i < values.Length; i++)
            {
                dr[i] = values[i];
            }
            dt.Rows.Add(dr);
        }
        file.Close();
        dataGridView1.DataSource = dt;*/
    }
}
