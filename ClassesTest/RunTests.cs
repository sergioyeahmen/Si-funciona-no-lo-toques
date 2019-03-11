﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniSQLEngine;
using System.Text.RegularExpressions;
using System.IO;

namespace ClassesTest
{
    [TestClass]
    public class RunTests
    {
        [TestMethod]
        public void TestCreateDropDatabaseTable()
        {
            string myDB = "myDB";
            ClassCreateDatabase db = new ClassCreateDatabase(myDB);
            db.Run(myDB);
            bool exists = Directory.Exists(@"..//..//..//data//myDB");
            //Testing CreateDatabase
            Assert.AreEqual(true, exists);

            string myTable = "myTable";
            string[] values = new string[2];
            values[0] = "column1 string true";
            values[1] = "column2 int false";
            ClassCreateTable ctable = new ClassCreateTable(myTable, values);
            ctable.Run(myDB);
            //Testing CreateTable
            bool existsDef = File.Exists(@"..//..//..//data//myDB//myTable.def");
            Assert.AreEqual(true, existsDef);
            bool existsData = File.Exists(@"..//..//..//data//myDB//myTable.data");
            Assert.AreEqual(true, existsData);

            ClassDropTable dtable = new ClassDropTable(myTable);
            dtable.Run(myDB);
            //Testing DropTable
            bool existsDefDrop = File.Exists(@"..//..//..//data//myDB//myTable.def");
            Assert.AreEqual(false, existsDefDrop);
            bool existsDataDrop = File.Exists(@"..//..//..//data//myDB//myTable.data");
            Assert.AreEqual(false, existsDataDrop);

            ClassDropDatabase drop = new ClassDropDatabase(myDB);
            drop.Run(myDB);
            //Testing DropDatabase
            bool existsDrop = Directory.Exists(@"..//..//..//data//myDB");
            Assert.AreEqual(false, existsDrop);
        }
        [TestMethod]
        public void ExecuteTest()
        {
            string dbname = "myDB";
            //string dbname1 = "myDB1";
            string myTable = "thisTable";
            string[] values = { "One String true", "Two String false", "Three String false", "Caramba String false" };
            string[] valuesToInsert = { "One", "Two", "Three", "Caramba" };
            //string pathfileDEF = @"..//..//..//data//" + dbname + "//" + myTable + ".def";
            string rutaCompleta = @"..//..//..//data//" + dbname + "//" + myTable + ".data";
            /*string queryCreateDB = @"CREATE DATABASE myDB;";
            string queryCreateTable = @"CREATE TABLE myTable (column1 int true,column2 string false,column3 int false);";
            string queryInsert = @"INSERT INTO table1 VALUES (10,abc,8);";
            */
            ClassCreateDatabase newDB = new ClassCreateDatabase(dbname);
            ClassCreateTable newTable = new ClassCreateTable(myTable, values);
            ClassInsert inserted = new ClassInsert(myTable, valuesToInsert);
            newDB.Run(dbname);
            bool exists = Directory.Exists(@"..//..//..//data//" + dbname);
            Assert.AreEqual(true, exists);
            newTable.Run(dbname);
            /*bool existsTables = File.Exists(@"..//..//..//data//"+dbname+"//"+myTable+".data");
            Assert.AreEqual(true, existsTables);
            */
            inserted.Run(dbname);
            Assert.AreNotSame(0, rutaCompleta.Length);
            /*string fullPath = @"..//..//..//data//myDB//thisTable.data";
            StreamWriter file = new StreamWriter(fullPath, true);
            for (int i = 0; i < valuesToInsert.Length; i++)
            {
                file.WriteLine(valuesToInsert[i]);
            }*/
        }
        [TestMethod]
        public void TestUpdate()
        {
            //Database
            string myDB = "myDB2";
            ClassCreateDatabase db = new ClassCreateDatabase(myDB);
            db.Run(myDB);
            //Table
            string myTable = "myTable2";
            string[] values = new string[2];
            values[0] = "column1 string true";
            values[1] = "column2 int false";
            ClassCreateTable ctable = new ClassCreateTable(myTable, values);
            ctable.Run(myDB);
            //Insert
            string[] values2 = new string[2];
            values2[0] = "hola";
            values2[1] = "7";
            ClassInsert cinsert = new ClassInsert(myTable, values2);
            cinsert.Run(myDB);
            
            //Update 1
            string[] values3 = new string[2];
            values3[0] = "column1=lola";
            values3[1] = "column2=5";
            string cond = "column1=hola";
            ClassUpdate cupdate = new ClassUpdate(myTable,values3,cond);
            cupdate.Run(myDB);
            //Testing update 1
            String[] lineas = System.IO.File.ReadAllLines("..//..//..//data//" + myDB + "//" + myTable + ".data");
            string actual = "lola,5";
            Assert.AreEqual(lineas[0], actual);

            //Update 2
            string[] values4 = new string[2];
            values4[0] = "column1=antonio";
            values4[1] = "column2=7";
            string cond2 = "column2>3";
            ClassUpdate cupdate2 = new ClassUpdate(myTable, values4, cond2);
            cupdate2.Run(myDB);
            //Testing update 2
            String[] lineas2 = System.IO.File.ReadAllLines("..//..//..//data//" + myDB + "//" + myTable + ".data");
            string actual2 = "antonio,7";
            Assert.AreEqual(lineas2[0], actual2);

            //Update 2
            string[] values5 = new string[2];
            values5[0] = "column1=francisco";
            values5[1] = "column2=9";
            string cond3 = "column2<20";
            ClassUpdate cupdate3 = new ClassUpdate(myTable, values5, cond3);
            cupdate3.Run(myDB);
            //Testing update 2
            String[] lineas3 = System.IO.File.ReadAllLines("..//..//..//data//" + myDB + "//" + myTable + ".data");
            string actual3 = "francisco,9";
            Assert.AreEqual(lineas3[0], actual3);
        }

        [TestMethod]
        public void TestDelete()
        {
            string myDB = "myDB";
            ClassCreateDatabase db = new ClassCreateDatabase(myDB);
            db.Run(myDB);

            string myTable = "myTable";
            string[] values = new string[2];
            values[0] = "name string true";
            values[1] = "edad int false";
            ClassCreateTable ctable = new ClassCreateTable(myTable, values);
            ctable.Run(myDB);

            string[] insert1 = new string[2];
            insert1[0] = "Asier";
            insert1[1] = "1";

            string[] insert2 = new string[2];
            insert2[0] = "Leire";
            insert2[1] = "3";

            string[] insert3 = new string[2];
            insert3[0] = "Navarro";
            insert3[1] = "5";

            string[] insert4 = new string[2];
            insert4[0] = "Hernando";
            insert4[1] = "2";

            string[] insert5 = new string[2];
            insert5[0] = "Josu";
            insert5[1] = "4";

            ClassInsert ins1 = new ClassInsert(myTable, insert1);
            ins1.Run(myDB);

            ClassInsert ins2 = new ClassInsert(myTable, insert2);
            ins2.Run(myDB);

            ClassInsert ins3 = new ClassInsert(myTable, insert3);
            ins3.Run(myDB);

            ClassInsert ins4 = new ClassInsert(myTable, insert4);
            ins4.Run(myDB);

            ClassInsert ins5 = new ClassInsert(myTable, insert5);
            ins5.Run(myDB);

            string cond = "edad>3";
            ClassDelete del = new ClassDelete(myTable, cond);
            del.Run(myDB);

            string pathfileDATA = @"..//..//..//data//" + myDB + "//" + myTable + ".data";
            String[] lines = System.IO.File.ReadAllLines(pathfileDATA);
            //Testing Delete with >
            Assert.AreEqual(3, lines.Length);
            Assert.AreEqual(insert1[0] + "," + insert1[1], lines[0]);
            Assert.AreEqual(insert2[0] + "," + insert2[1], lines[1]);
            Assert.AreEqual(insert4[0] + "," + insert4[1], lines[2]);

            string cond2 = "name=Leire";
            ClassDelete del2 = new ClassDelete(myTable, cond2);
            del2.Run(myDB);

            String[] lines2 = System.IO.File.ReadAllLines(pathfileDATA);
            //Testing Delete with string
            Assert.AreEqual(2, lines2.Length);
            Assert.AreEqual(insert1[0] + "," + insert1[1], lines2[0]);
            Assert.AreEqual(insert4[0] + "," + insert4[1], lines2[1]);

            string cond3 = "edad<2";
            ClassDelete del3 = new ClassDelete(myTable, cond3);
            del3.Run(myDB);

            String[] lines3 = System.IO.File.ReadAllLines(pathfileDATA);
            //Testing Delete with <
            Assert.AreEqual(1, lines3.Length);
            Assert.AreEqual(insert4[0] + "," + insert4[1], lines3[0]);
        }
    }
}
