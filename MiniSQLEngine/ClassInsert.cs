﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSQLEngine
{
    public class ClassInsert : Query
    {
        private string aTable;
        private string[] values;
        private string result = "";
        private string[] atributes;
        public ClassInsert(String table, String[] myArray, String[] myArray2)
        {
            aTable = table;
            values = myArray;
            atributes = myArray2;
        }
        public string GetTable()
        {
            return aTable;
        }

        public string[] GetValues()
        {
            return values;
        }


        public override string getClass()
        {
            return "insert";
        }

        public override string getResult()
        {
            return result;
        }

        public override void Run(string dbname)
        {
            string pathfileDATA = @"..//..//..//data//" + dbname + "//" + aTable + ".data";

            if (!File.Exists(pathfileDATA))
            {
                result = Constants.TableDoesNotExist;
            }

            else
            {
                if (atributes==null)
                {
                string pathfileDEF = @"..//..//..//data//" + dbname + "//" + aTable + ".def";
                string line1;
                List<string> columns = new List<string>();

                using (StreamReader sr = new StreamReader(pathfileDEF))
                {
                    while ((line1 = sr.ReadLine()) != null)
                    {
                        string[] parts = line1.Split(',');
                        foreach (string element in parts)
                        {
                            string[] atributes = element.Split(' ');
                            string type = atributes[1];
                            columns.Add(type);
                        }
                    }
                }
                    if (columns.Count() != values.Length)
                    {
                        result = Constants.WrongSyntax;
                    }

                    else
                    {
                        int index = 0;
                        foreach (string c in columns)
                        {

                            if (c.ToLower().Equals("int"))
                            {
                                try
                                {
                                    int.Parse(values[index]);
                                }
                                catch
                                {
                                    result = Constants.IncorrectDataType;
                                }
                            }
                            index++;
                        }

                        if (result == "")
                        {
                            string texto = "";
                            for (int i = 0; i < values.Length; i++)
                            {
                                if (i == values.Length - 1)
                                {
                                    texto = texto + values[i];
                                }
                                else
                                {
                                    texto = texto + values[i] + ",";

                                }
                            }

                            using (StreamWriter file = File.AppendText(pathfileDATA))
                            {
                                //Data added to the document
                                file.WriteLine(texto);
                                file.Close();
                                result = Constants.InsertSuccess;
                            }
                        }
                    }
                }
                else
                {
                    String[] lineadef = System.IO.File.ReadAllLines("..//..//..//data//" + dbname + "//" + aTable + ".def");
                    String[] porComas = lineadef[0].Split(',');
                    String[] atributosDEF = new String[lineadef.Length];
                    int contador = 0;
                    foreach(String actual in porComas)
                    {
                        String[] espacio=actual.Split(' ');
                        atributosDEF[contador] = espacio[0];
                        contador++;
                    }
                    String linea = "";
                    foreach (String atriActual in atributosDEF)
                    {
                        foreach (String valor)
                        {

                        }
                    }
                }
            }
        }
    }
}
