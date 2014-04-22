using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Crosscutting;
using DeviceFinder.Business.Interface.Factories;
using DeviceFinder.Data.Interface.Dao;
using Excel;
using Ninject;
using TestApp.Model;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {

            IExcelDataReader excelReader = null;
            try
            {
                //Reading for a OpenXml Excel file
                var strem = new FileStream("E:/dev/DeviceFinder/DeviceFinder/Excel/model_productLine.xlsm", FileMode.Open);
                //var strem = File.Open("E:/dev/DeviceFinder/DeviceFinder/Excel/model_productLine.xls", FileMode.Open, FileAccess.Read);

              //     excelReader = ExcelReaderFactory.CreateBinaryReader(strem,ReadOption.Loose);
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(strem);
              //  excelReader.IsFirstRowAsColumnNames = false;

                //DataSet
                var result = excelReader.AsDataSet();

                //foreach (DataTable table in result.Tables)
                //{
                //    Console.WriteLine(table.TableName);
                //    for (int i = 0; i < table.Rows.Count; i++)
                //    {
                //        for (int j = 0; j < table.Columns.Count; j++)
                //            Console.Write("\"" + table.Rows[i].ItemArray[j] + "\";");
                //        Console.WriteLine();
                //    }
                //}

                Cell groupCell = null;
                Cell prodLineCell = null;
                var prodlines = new List<ProductLine>();
                string propGrp = string.Empty;


                const string allowedProperty = @"[A-Z]+";
                var regex = new Regex(allowedProperty);
                

                var prodlineTable = result.Tables["ProductLine"];

                for (int i = 0; i < prodlineTable.Rows.Count; i++)
                {
                    //for (int j = 0; j < prodlineTable.Columns.Count; j++)
                   //     Console.Write("\"" + prodlineTable.Rows[i].ItemArray[j] + "\";");
                    for (int j = 0; j < prodlineTable.Columns.Count; j++)
                    {
                        // Ermittle die Gruppen Zeile
                        if ((prodlineTable.Rows[i].ItemArray[j].ToString().Contains("Tech") || prodlineTable.Rows[i].ItemArray[j].ToString().Contains("Phase")) && groupCell == null)
                        {
                            groupCell = new Cell()
                            {
                                X = j,
                                Y = i
                            };
                          //  Console.WriteLine("  ---> Group: " + groupCell);
                        }
                        // Ermittle die ProductLine Zeile 
                        if (prodlineTable.Rows[i].ItemArray[j].ToString().Contains("Line"))
                        {
                            prodLineCell = new Cell
                            {
                                X = j,
                                Y = i
                            };
                       //     Console.WriteLine("  ---> ProdLineRow: " + prodLineCell);
                        }
                      
                    }
                      if(groupCell!= null && prodLineCell != null)
                            break;
                 }
                 //  Console.WriteLine();

              
                for (int j = prodLineCell.Y+1; j < prodlineTable.Rows.Count; j++)
                {

                    var productPropertys = new List<ProductLineProperty>();
                   

                    if (!regex.Match(prodlineTable.Rows[j].ItemArray[prodLineCell.X].ToString()).Success)
                        continue;

                   // Console.WriteLine("Name: " + prodlineTable.Rows[j].ItemArray[prodLineCell.X].ToString());

                    for (int k = 1; k< prodlineTable.Columns.Count; k++)
                    {
                        if (prodlineTable.Rows[groupCell.X].ItemArray[k].ToString() != string.Empty)
                            propGrp = prodlineTable.Rows[groupCell.X].ItemArray[k].ToString();

                        var productLineProperty = new ProductLineProperty()
                        {
                            Group = propGrp,
                            Name = prodlineTable.Rows[prodLineCell.Y].ItemArray[prodLineCell.X + k].ToString(),
                            Value = !Convert.IsDBNull(prodlineTable.Rows[j].ItemArray[k])
                        };
                        productPropertys.Add(productLineProperty);

                      //  Console.WriteLine("Grp: " + propGrp  + ", Prop: " + prodlineTable.Rows[prodLineCell.Y].ItemArray[prodLineCell.X + k] + ", Value: " + !Convert.IsDBNull(prodlineTable.Rows[j].ItemArray[k]));
                    }
                    propGrp = string.Empty;

                    
                    var item = new ProductLine()
                    {
                        Name = prodlineTable.Rows[j].ItemArray[prodLineCell.X].ToString(),
                        ProductLineProperties = productPropertys
                    };
                    prodlines.Add(item);
                }

                // TestScreenoutput
                foreach (var productLine in prodlines)
                {
                    Console.WriteLine(productLine.ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (excelReader != null)
                {
                    excelReader.Close();
                    excelReader.Dispose();
                }
            }

            //var f =  KernelManager.Instance.GetKernel().Get<IDeviceFinderFactory>();

            //var models = f.GetAllModelsFromExcelSheet();

            //Console.WriteLine("Model Count: " + models.Count()); // OKAY
            
            //Debug.WriteLine(Guid.NewGuid());
            Console.Read();
        }
    }
}
