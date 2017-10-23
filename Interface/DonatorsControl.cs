﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.Collections;

namespace Interface
{
    public partial class DonatorsControl : UserControl
    {

        private DataView dv;
        private DataTable dt;
        private int sortColumn = -1;

        public DonatorsControl()
        {
            InitializeComponent();

            
        }

        private void DonatorsControl_Load(object sender, EventArgs e)
        {
            // Destalhes da listview 
            listView1.View = View.Details;
            ReloadListView();
           
            FilterBox.Items.Add("Nome");
            FilterBox.Items.Add("Sexo");
            FilterBox.Items.Add("Idade");
            FilterBox.Items.Add("GrupoSanguineo");
            FilterBox.Items.Add("IMC");
            FilterBox.SelectedIndex = 0;

        }

        private void ReloadListView()
        {
            

            dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Nome");
            dt.Columns.Add("Sexo");
            dt.Columns.Add("Idade");
            dt.Columns.Add("GrupoSanguineo");
            dt.Columns.Add("IMC");
            int[] posicao = {1,2,3,4 };
            List<BloodDonator> donators = new List<BloodDonator>();
            

            donators = CreateListDonators.ListDonators(); // listar recebe a lista completa

            ExportDonators.ExportDonator(donators, 0, posicao);

            if (donators.Count == 0)
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Please import data from a source file .txt!";
            }
            else
            {
                statusLabel.ForeColor = Color.Green;
                statusLabel.Text = "OK, " + donators.Count + " Donators Load.";
            }

            CarregarDadosTabela(donators);
            dv = new DataView(dt);

            CarregarDataProcura(dv);
        }

        private void CarregarDadosTabela(List<BloodDonator> donators)
        {

            foreach (BloodDonator bd in donators.OrderBy(c => c.Number))
            {
                int id = bd.Number;
                String p_nome = bd.FirstName + "" + bd.LastName;
                String sexo = bd.Sexo;
                int idade = bd.Age;
                String g_sangue = bd.BloodType;
                long telefone = bd.TelephoneNumber;
                String cidade = bd.City;
                double imc = bd.IMC;

                dt.Rows.Add(id, p_nome, sexo, idade, g_sangue, String.Format("{0:0.00}", imc));
            }
        }


        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            dv.RowFilter = string.Format(FilterBox.Text + " Like'%{0}%'", SearchBox.Text);
            CarregarDataProcura(dv);
        }

        private void FilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            dv.RowFilter = string.Format(FilterBox.Text + " Like '%{0}%'", SearchBox.Text);
            CarregarDataProcura(dv);
        }

        private void CarregarDataProcura(DataView dv)
        {
            listView1.Items.Clear();
            foreach (DataRow row in dv.ToTable().Rows)
            {
                listView1.Items.Add(new ListViewItem(new String[]
                    {row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString()}));
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            

           /* foreach (var b in selectedItemIndexes)
            {
                MessageBox.Show(Convert.ToString(b));
            }*/
        }

        private void listView1_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            ItemComparer sorter = listView1.ListViewItemSorter as ItemComparer;
            if (sorter == null)
            {
                sorter = new ItemComparer(e.Column);
                sorter.Order = SortOrder.Ascending;
                listView1.ListViewItemSorter = sorter;
            }
            // if clicked column is already the column that is being sorted
            if (e.Column == sorter.Column)
            {
                // Reverse the current sort direction
                if (sorter.Order == SortOrder.Ascending)
                    sorter.Order = SortOrder.Descending;
                else
                    sorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.Column = e.Column;
                sorter.Order = SortOrder.Ascending;
            }
            listView1.Sort();


        }

        

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void importButton_Click(object sender, EventArgs e)
        {
            
            if (ReadFile.Devolver() == false)
            {
                MessageBox.Show("Error parsing source file.");
            }
            else
            {
                ReloadListView();
                MessageBox.Show("Data successfully load.");
            }
           
        }

        private void label1_Click(object sender, EventArgs e)
        {
            List<int> selectedItemIndexes = new List<int>();
            foreach (ListViewItem a in listView1.SelectedItems)
            {
                var x = a.Text;
                label1.Text += (Convert.ToString(x));
            }
        }
    }


    
}

