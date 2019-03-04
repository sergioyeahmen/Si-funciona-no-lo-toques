﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSQLEngine
{
    public class ClassUpdate : Query
    {
        private string Table,Condition;
        //atrb=date ; atrib=date
        private string[] Column;
        
        public ClassUpdate(string pTable,string[] pColumn,string pCondition)
        {
            Table = pTable;
            Column = pColumn;
            Condition = pCondition;
        }
        public string GetTable()
        {
            return Table;
        }

        public string[] GetColumns()
        {
            return Column;
        }
        public string GetCondition()
        {
            return Condition;
        }

        public override string getClass()
        {
            return "update";
        }

        public override void Run(string dbname)
        {
            String[] elements = new String[2];
            String operador = "";
            int posicion = 0;

            //I need to know the operator of the condition
            if (Condition.Contains("="))
            {
                elements = Condition.Split('=');
                operador = "=";
            }
            else if (Condition.Contains("<"))
            {
                elements = Condition.Split('<');
                operador = "<";
            }
            else if (Condition.Contains(">"))
            {
                elements = Condition.Split('>');
                operador = ">";
            }

            //I need a new line with the new dates of the row
            String newRow = "";
            int longitud = Column.Length;
            int cuenta = 1;
            foreach (String colum in Column)
            {
                if (cuenta!=longitud)
                {
                    String[] actual = colum.Split('=');
                    newRow = newRow + actual[1] + ",";
                }
                else
                {
                    String[] actual = colum.Split('=');
                    newRow = newRow + actual[1];
                }
                cuenta++;
            }

            //Open te file .def
            String allFile = System.IO.File.ReadAllText("..//..//..//data//" + dbname + "//" + Table + ".def");
            String[] atrib = allFile.Split(',');

            //Search the postion of the atribute that appears in the condition
            String buscar = elements[0];
            Boolean parar = false;
            foreach (String atributo in atrib)
            {
                if (!parar)
                {
                    if (!atributo.Contains(buscar))
                    {
                        posicion = posicion + 1;
                    }
                    else
                    {
                        parar = true;
                    }
                }
            }

            //Open te file .data
            String[] lineas = System.IO.File.ReadAllLines("..//..//..//data//" + dbname + "//" + Table + ".data");

            //Make the update
            int inde = 0;
            foreach (String linea in lineas)
            {

                String[] datos = linea.Split(',');
                if (operador == "<")
                {
                    int numero = Int32.Parse(datos[posicion]);
                    if (numero < Int32.Parse(elements[1]))
                    {
                        lineas.SetValue(newRow, inde);
                    }
                    inde++;
                }
                else if (operador == ">")
                {
                    int numero = Int32.Parse(datos[posicion]);
                    if (numero > Int32.Parse(elements[1]))
                    {
                        lineas.SetValue(newRow, inde);
                    }
                    inde++;

                }
                else if (operador == "=")
                {
                    if (datos[posicion] == elements[1])
                    {
                        lineas.SetValue(newRow, inde);
                    }
                    inde++;
                }
            }

            //Make changes on the file
            using (StreamWriter sw = System.IO.File.CreateText("..//..//..//data//" + dbname + "//" + Table + ".data"))
                foreach (String linea in lineas)
                {
                    sw.WriteLine(linea);
                }
        }
    }
}

