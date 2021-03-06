﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSQLEngine
{
    public class SecGrant : Query
    {
        private string privilege_type,table, security_profile;
        private string result;

        public SecGrant(string pprivilege_type, string ptable,string psecurity_profile)
        {
            privilege_type = pprivilege_type;
            table = ptable;
            security_profile = psecurity_profile;
        }
        public override string getClass()
        {
            return "grant";
        }

        public override string getResult()
        {
            return result;
        }

        public override void Run(string dbname)
        {
            string pathProfiles = @"..\\..\\..\\data\\" + dbname + "\\profiles\\" + security_profile+".pf";
            string pathUssers = @"..\\..\\..\\data\\" + dbname + "\\"+table+".sec";
            if (File.Exists(pathProfiles)==false)
            {
                result = Constants.SecurityProfileDoesNotExist;
            }
            else
            {
                if (File.Exists(pathUssers) == false)
                {
                    result = Constants.TableDoesNotExist;
                }
                else
                {
                    String[] lineasSec = System.IO.File.ReadAllLines(pathUssers);
                    int contar = 0;
                    int contaroficial = -1;
                    foreach (string actual in lineasSec)
                    {
                        string[] actualSplit = actual.Split(',');
                        if (actualSplit[0].Contains(security_profile))
                        {
                            contaroficial = contar;
                        }
                        else
                        {
                            contar++;
                        }
                    }
                    if (contaroficial == -1)
                    {
                        using (StreamWriter stream2 = File.AppendText(pathUssers))
                        {
                            String linea = security_profile+","+ privilege_type;
                            stream2.WriteLine(linea);
                        }
                        result = Constants.SecurityPrivilegeGranted;
                    }
                    else
                    {
                        string linea = lineasSec[contaroficial];
                        string[] lineaSplit = linea.Split(',');
                        string profile = lineaSplit[0];

                        string[] privi = lineaSplit[1].Split('/');
                        Boolean lotiene = false;
                        foreach(string actual in privi)
                        {
                            if (actual== privilege_type.ToUpper())
                            {
                                lotiene = true;
                            }
                        }
                        if (lotiene==false)
                        {
                            string lalinea = linea + "/" + privilege_type.ToUpper();
                            lineasSec[contaroficial] = lalinea;
                        }

                        using (StreamWriter stream3 = File.CreateText(pathUssers))
                        {
                            foreach(string ahora in lineasSec){
                                stream3.WriteLine(ahora);
                            }
                        }
                        result = Constants.SecurityPrivilegeGranted;
                    }
                }
            }
        }
    }
}
