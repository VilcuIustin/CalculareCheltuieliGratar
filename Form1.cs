﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;



namespace Calculare_Cheltuieli_Gratar
{
    public partial class Calculator_Gratar : Form
    {
        public Calculator_Gratar()
        {
            InitializeComponent();
        }

        async void adauga_randuri_tab(int nr_randuri)
        {
            
            for (int i = 0; i < nr_randuri; i++)
            { 
                Data1.DataTable1.Rows.Add(null,"", 0,0);
               

            }
                
        }

        async void adauga_randuri_tab1(int nr)
        {
            for (int i = 0; i < nr; i++)
            {
                Data1.Preferinte.Rows.Add(null,"", true, false, false, 0);
            }
        }


        private void Resetare_inc(dynamic tabel)
        {
            tabel.IDColumn.AutoIncrementSeed = -1;
            tabel.IDColumn.AutoIncrementStep = -1;
            tabel.IDColumn.AutoIncrementSeed = 1;
            tabel.IDColumn.AutoIncrementStep = 1;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            int nr;
            Data1.DataTable1.Clear();
            Resetare_inc(Data1.DataTable1);

            if(Regex.IsMatch(textBox1.Text , @"^\d+$"))
            {

                nr = int.Parse(textBox1.Text);
                adauga_randuri_tab(nr);
                D1.DataSource=Data1.DataTable1;
                
    
            }
            else
            {
                MessageBox.Show("Nu ai introdus un numar! Te rog sa mai incerci.");
            }
           
           
       
        } 
        private void reset_Click(object sender, EventArgs e)
        {
            Data1.DataTable1.Rows.Clear();
            Resetare_inc(Data1.DataTable1);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox2.Text != String.Empty)
            {
                int nr;
                int.TryParse(textBox2.Text,out nr);
                Data1.Preferinte.Clear();
                Data1.Piata.Clear();
                Resetare_inc(Data1.Preferinte);
                adauga_randuri_tab1(nr);
                D2.DataSource = Data1.Preferinte;
                Data1.Piata.Rows.Add(0, 0, 0);
                D3.DataSource = Data1.Piata;
                
            }
            else
            {
                MessageBox.Show("Introdu date in casuta!");
            }
            


        }

       

        private void reset2_Click(object sender, EventArgs e)
        {
            Data1.Preferinte.Rows.Clear();
            Resetare_inc(Data1.Preferinte);
        }

        async Task<double> Calculator(int nr, string alegere, int i)
        {
            DataRowCollection dr = Data1.Piata.Rows;
            double rezultat = Convert.ToDouble(dr[0][i])/ Convert.ToDouble(nr);
           
          
            return rezultat;
        }

        async Task<dynamic> Nr_persoane_alegere()
        {
            int mancare = 0, bere = 0, altele = 0;
            foreach (DataRow dr in Data1.Preferinte)
            {
                if (dr.Field<bool>("Mancare"))
                {
                    mancare++;
                }

                if (dr.Field<bool>("Bere"))
                {
                    bere++;
                }
                if (dr.Field<bool>("Wiskey/Altele"))
                {
                    altele++;
                }
            }
           
           
                return (mancare, bere , altele);
          
        }

        async void Calculare_pret_persoana(double mancare, double bere, double altele)
        {
            foreach(DataRow dr in Data1.Preferinte.Rows)
            {
                double suma = 0;
                if (dr.Field<bool>("Mancare"))
                {
                    suma += mancare;
                }
                if (dr.Field<bool>("Bere"))
                {
                    suma += bere;
                }
                if (dr.Field<bool>("Wiskey/Altele"))
                {
                    suma += altele;
                }
                dr.SetField<double>("Pret total", Math.Round(suma));

            }
        }

        async void de_primit()
        {
           foreach(DataRow dr in Data1.Preferinte.Rows)
            {
                foreach(DataRow dr1 in Data1.DataTable1.Rows)
                {
                    if (dr1.Field<string>("Nume") == dr.Field<string>("Nume"))
                    {
                        double rezultat = dr1.Field<double>("Suma") - dr.Field<double>("Pret total");
                        Console.WriteLine(rezultat);
                        dr1.SetField<double>("Suma de primit", Math.Round(rezultat));
                        break;
                    }
                }
            }
        }


        private async void button2_Click(object sender, EventArgs e)
        {

            dynamic rezultat1  = Nr_persoane_alegere().Result;

          
            int mancare = rezultat1.Item1;
            int bere = rezultat1.Item2;
            int altceva = rezultat1.Item3;
            
            
            dynamic a =  Calculator(mancare, "Mancare",0).Result;
            dynamic b = Calculator(bere, "bere",1).Result;
            dynamic c = Calculator(altceva, "Wiskey/Altele",2).Result;
            Calculare_pret_persoana(a, b, c);
            de_primit();
            
            




        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) 
            {
                string filePath = openFileDialog1.FileName;
                Data1.Clear();
                Data1.ReadXml(filePath);
                D2.DataSource = Data1.Preferinte;
                D3.DataSource = Data1.Piata;
                D1.DataSource = Data1.DataTable1;
 

            }
            else
            {
                MessageBox.Show("Ai inchis dialogul.");
            }
           

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Data1.WriteXml(saveFileDialog1.FileName);
            }
                
               
        }

       
    }
}
