﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TopEditor
{
    public partial class Form1 : Form
    {
        Analyzer testAnalyzer = new Analyzer();
        Module testModule;
        string path = @"d:\Test.txt";
        Module[] newlistOfModules;
        Module[] listOfModules = new Module [100];
        int curModNumber = 0;
        int curInstNumber = 0;
        int curConntNumber = 0;
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();
        DataTable dt4 = new DataTable();
        Instance [] listOfInstances = new Instance [100];
        Connection[] listOfConnections = new Connection[100];

        
      
        public Form1()
        {
            InitializeComponent();
            dt1.Columns.Add("Dir");
            dt1.Columns.Add("Data type");
            dt1.Columns.Add("Size");
            dt1.Columns.Add("Name");
            dataGridView1.DataSource = dt1;
            listOfModuleLB.Click += new EventHandler(showPortsBtn_Click);

            dt2.Columns.Add("Dir");
            dt2.Columns.Add("Data type");
            dt2.Columns.Add("Size");
            dt2.Columns.Add("Name");
            dataGridView2.DataSource = dt2;
            listOfInstLB.Click += new EventHandler(listOfInstLB_Click);

            dt3.Columns.Add("Dir");
            dt3.Columns.Add("Data type");
            dt3.Columns.Add("Size");
            dt3.Columns.Add("Name");
            dataGridView3.DataSource = dt3;
            listOfInstLB2.Click += new EventHandler(listOfInstLB2_Click);


            dt4.Columns.Add("Inst 1");
            dt4.Columns.Add("Port 1");
            dt4.Columns.Add("Inst 2");
            dt4.Columns.Add("Port 2");
            dataGridView4.DataSource = dt4;
            listOfConnLB.Click += new EventHandler(listOfConnLB_Click);

        }

        private void button1_Click(object sender, EventArgs e)
        {
          int i = 0;
          int j = 0;
          int modAlrdyExist = 0;
          newlistOfModules = testAnalyzer.analizeFile(textBox1.Text);
          for (i = 0; i < newlistOfModules.Length; i++)
            if (newlistOfModules[i] != null)
            {
              modAlrdyExist = 0;
              for (j = 0; j < listOfModules.Length; j++)
              {
                if (listOfModules[j] != null && newlistOfModules[i].getModName() == listOfModules[j].getModName())
                  modAlrdyExist++;
              }
              if (modAlrdyExist > 0)
                MessageBox.Show("Модуль " + newlistOfModules[i].getModName() + " уже существует.");
              else
                {
                  listOfModules[curModNumber] = newlistOfModules[i];
                  curModNumber++;
                }
              //newlistOfModules[i].showModDeclaration();
            }
          this.updateListOfModules(listOfModules);
        }


        private void updateListOfModules(Module[] listOfModules)
        {
          int i = 0;
          listOfModuleLB.Items.Clear();
          for (i = 0; i < listOfModules.Length; i++)
            if (listOfModules[i] != null)
            {
              listOfModuleLB.Items.Add(listOfModules[i].getModName());
              //newlistOfModules[i].showModDeclaration();
            }
        }

        public void deleteModule(string modName)
        {
          int i = 0;
          for (i = 0; i < listOfModules.Length; i++)
            if (listOfModules[i] != null && listOfModules[i].getModName() == modName)
            {
              listOfModules[i] = null;
              this.updateListOfModules(listOfModules);
            }
        }

        Module getModule(string modName)
        {
          int i = 0;
          for (i = 0; i < listOfModules.Length; i++)
            if (listOfModules[i] != null && listOfModules[i].getModName() == modName)
            {
              return listOfModules[i];
            }
          return null;
        }

        private void showModPorts(string modName, DataTable dt)
        {
          int i = 0;
          int j = 0;
          dt.Clear();

          for (i = 0; i < listOfModules.Length; i++)
          {
            if (listOfModules[i] != null && listOfModules[i].getModName() == modName)
              for (j = 0; j < listOfModules[i].listOfPorts.Length; j++)
                if (listOfModules[i].listOfPorts[j] != null)
                  dt.Rows.Add(listOfModules[i].listOfPorts[j].dir, listOfModules[i].listOfPorts[j].data_type, listOfModules[i].listOfPorts[j].dim, listOfModules[i].listOfPorts[j].name);
          }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
          if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
          {
            textBox1.Text = openFileDialog1.FileName;
          }
        }

        private void delModBtn_Click(object sender, EventArgs e)
        {
          deleteModule(listOfModuleLB.SelectedItem.ToString());
        }

        private void showPortsBtn_Click(object sender, EventArgs e)
        {
          showModPorts(listOfModuleLB.SelectedItem.ToString(), dt1);
        }

        private void listOfInstLB_Click(object sender, EventArgs e)
        {
          showInstPorts(listOfInstLB.SelectedItem.ToString(), dt2);
        }

        private void listOfInstLB2_Click(object sender, EventArgs e)
        {
          showInstPorts(listOfInstLB2.SelectedItem.ToString(), dt3);
        }

        private void createInstBtn_Click(object sender, EventArgs e)
        {
          addInstance(listOfModuleLB.SelectedItem.ToString(), instNameTB.Text);
        }

        public void addInstance (string baseModName, string newInstName)
        {
          int i = 0;
          int j = 0;
          int instAlrdyExist = 0;
          int baseModNumber = 0;
          
          for (i = 0; i < listOfModules.Length; i++)
            if (listOfModules[i] != null && baseModName == listOfModules[i].getModName())
            {
              baseModNumber = i;
            }

          for (j = 0; j < listOfInstances.Length; j++)
            {
              if (listOfInstances[j] != null && newInstName == listOfInstances[j].Name)
                instAlrdyExist++;
            }
          if (instAlrdyExist > 0)
            MessageBox.Show("Экземпляр " + newInstName + " уже существует.");
            else
              {

                listOfInstances[curInstNumber] = new Instance(newInstName, listOfModules[baseModNumber]);
                curInstNumber++;
              }

          this.updateListOfInstances(listOfInstances, listOfInstLB);
          this.updateListOfInstances(listOfInstances, listOfInstLB2);
        }

        private void updateListOfInstances(Instance [] listOfInstances, ListBox lb)
        {
          int i = 0;
          lb.Items.Clear();
          for (i = 0; i < listOfInstances.Length; i++)
            if (listOfInstances[i] != null)
            {
              lb.Items.Add(listOfInstances[i].Name);
              //newlistOfModules[i].showModDeclaration();
            }
        }

        public void deleteInstance (string InstName)
        {
          int i = 0;
          for (i = 0; i < listOfInstances.Length; i++)
            if (listOfInstances[i] != null && listOfInstances[i].Name == InstName)
            {
              listOfInstances[i] = null;
              this.updateListOfInstances(listOfInstances, listOfInstLB);
              this.updateListOfInstances(listOfInstances, listOfInstLB2);
            }
        }

        Instance getInstance(string InstName)
        {
          int i = 0;
          for (i = 0; i < listOfInstances.Length; i++)
            if (listOfInstances[i] != null && listOfInstances[i].Name == InstName)
            {
              return listOfInstances[i];
            }
          return null;
        }


        private void delInstBtn_Click(object sender, EventArgs e)
        {
          deleteInstance(listOfInstLB.SelectedItem.ToString());
        }

        private void showInstPorts(string InstName, DataTable dt)
        {
          int i = 0;
          int j = 0;
          for (i = 0; i < listOfInstances.Length; i++)
          {
            if (listOfInstances[i] != null && listOfInstances[i].Name == InstName)
              showModPorts(listOfInstances[i].BaseModule.modName, dt);
          }
        }

        public void addConnection(string connName, string inst1_name, string inst1_port_name, string inst2_name, string inst2_port_name)
        {
          int i = 0;
          int j = 0;
          int connAlrdyExist = 0;
          Instance inst1;
          Port inst1_port;
          Instance inst2;
          Port inst2_port;

          inst1 = this.getInstance(inst1_name);
          inst1_port = inst1.BaseModule.getPort(inst1_port_name);
          inst2 = this.getInstance(inst2_name);
          inst2_port = inst2.BaseModule.getPort(inst2_port_name);

          Console.WriteLine(inst1.Name);

          for (j = 0; j < listOfConnections.Length; j++)
          {
            if (listOfConnections[j] != null && connName == listOfConnections[j].Name)
              connAlrdyExist++;
          }
          if (connAlrdyExist > 0)
            MessageBox.Show("Экземпляр " + connName + " уже существует.");
          else
          {

            listOfConnections[curConntNumber] = new  Connection (connName, inst1, inst1_port, inst2, inst2_port);
            Console.WriteLine(listOfConnections[curConntNumber].inst_1.Name);
            curConntNumber++;
          }

          this.updateListOfConnections(listOfConnections, listOfConnLB);
        }

        private void updateListOfConnections(Connection[] listOfConnections, ListBox lb)
        {
          int i = 0;
          lb.Items.Clear();
          for (i = 0; i < listOfConnections.Length; i++)
            if (listOfConnections[i] != null)
            {
              lb.Items.Add(listOfConnections[i].Name);
              //newlistOfModules[i].showModDeclaration();
            }
        }

        private void createConnBtn_Click(object sender, EventArgs e)
        {
          addConnection(connectionTB.Text, listOfInstLB.SelectedItem.ToString(), dataGridView2.CurrentCell.Value.ToString(), listOfInstLB2.SelectedItem.ToString(), dataGridView3.CurrentCell.Value.ToString());
          Console.WriteLine(dataGridView2.CurrentCell.Value.ToString());
        }

        private void listOfConnLB_Click(object sender, EventArgs e)
        {
          showConnections(listOfConnLB.SelectedItem.ToString(), dt4);
        }

        private void showConnections(string connName, DataTable dt)
        {
          int i = 0;
          int j = 0;
          dt.Clear();
          for (i = 0; i < listOfConnections.Length; i++)
          {
            if (listOfConnections[i] != null && listOfConnections[i].Name == connName)
              dt.Rows.Add(listOfConnections[i].inst_1.Name, listOfConnections[i].inst_1_port.name , listOfConnections[i].inst_2.Name, listOfConnections[i].inst_2_port.name);
          }
        }

    }
}
