using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DeviceFinder.Data.Impl.Entity;
using DeviceFinder.Data.Impl.Vo;
using DeviceFinder.Data.Interface.Dao;
using DeviceFinder.Data.Interface.Vo;
using Crosscutting.Enums;
using Excel;

namespace DeviceFinder.Data.Impl.Dao
{
    public class DataAccessObject : IDataAccessObject
    {
        #region Fields

#pragma warning disable 169
        private static DataAccessObject _instance;
#pragma warning restore 169
        private string _filename;
        private DataSet _dataResult;
        private static FlowConfigurationEntities _entities = new FlowConfigurationEntities();

        private int _langId;

        #endregion

        #region Singleton

        //public static IDataAccessObject Instance
        //{
        //    get { return _instance ?? (_instance = new DataAccessObject()); }
        //}

        //private DataAccessObject()
        //{
        //}

        #endregion

        /// <summary>
        /// Initailiere die DAO
        /// </summary>
        /// <param name="filename">Der Pfad zur Excel Datei</param>
        public void Init(string filename)
        {
            Init(filename, ExcelFileFormat.XLSX);
        }

        /// <summary>
        /// Initialisiere die DAO 
        /// </summary>
        /// <param name="filename">Der Pfad zur Excel Datei</param>
        /// <param name="format">Excel file format  XLSX or XLS</param>
        public void Init(string filename, ExcelFileFormat format)
        {
            _filename = filename;
            Debug.WriteLine("\n ##### Filename: " + filename + " \n");
            _dataResult = GetExcelDataSet(format);
            _langId = -1;
        }


        /// <summary>
        /// Liest Alle Modelnamen aus der Excel aus und gibt sie als Vos zurück
        /// </summary>
        /// <returns>ModelVos</returns>
        public IEnumerable<IModelVo> GetAllExcelModelVos()
        {
            //Create ModelVos
            var models = new List<IModelVo>();


            // Prüfe ob ein DatenContext vorhanden ist
            if (_dataResult == null)
                return models;

            // Lese aus dem Stream die Tabele für "Models" 
            var dataTable = _dataResult.Tables["Models"];

            // check for Modelname
            const string propertyRegex = @"[A-Z]+[0-9]+";
            var regex = new Regex(propertyRegex);

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                var modelName = dataTable.Rows[i].ItemArray[0].ToString();

                // Prüfe ob es sich im Datensatz um ein gültiges Modell handelt.
                var match = regex.Match(modelName).Success;
                if (!match) continue;

                var item = new ModelVo
                {
                    Matnr = dataTable.Rows[i].ItemArray[0].ToString(),
                    Coriolis = ConvertDbNullToBool(dataTable.Rows[i].ItemArray[1]),
                    Rotameter = ConvertDbNullToBool(dataTable.Rows[i].ItemArray[2]),
                    Magnetic = ConvertDbNullToBool(dataTable.Rows[i].ItemArray[3]),
                    Ultrasonic = ConvertDbNullToBool(dataTable.Rows[i].ItemArray[4]),
                    Vortex = ConvertDbNullToBool(dataTable.Rows[i].ItemArray[5]),
                    Pressure = ConvertDbNullToBool(dataTable.Rows[i].ItemArray[6]),
                    Liquide = ConvertDbNullToBool(dataTable.Rows[i].ItemArray[7]),
                    Gas = ConvertDbNullToBool(dataTable.Rows[i].ItemArray[8]),
                    Steam = ConvertDbNullToBool(dataTable.Rows[i].ItemArray[9]),
                };

                models.Add(item);
            }
            return models.ToArray();
        }

        public IEnumerable<IViewVo> GetViewVos(string sheetName)
        {
            var views = new List<IViewVo>();

            if (_dataResult == null)
                return views;

            var dataTable = _dataResult.Tables[sheetName];

            for (int i = 1; i < dataTable.Rows.Count; i++)
            {
                var currentRow = dataTable.Rows[i];
                if (currentRow.ItemArray[0].ToString() == string.Empty)
                    continue;
                var item = new ViewVo()
                {
                    FieldName = currentRow.ItemArray[0].ToString(),
                    OrderId = null, // TODO --> currentRow.ItemArray[1]
                    Visible = currentRow.ItemArray[2].ToString() == "true"
                };
                views.Add(item);
            }
            return views;
        }

        /// <summary>
        /// Liest alle produktLinien aus der Exceltabelle aus
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public IEnumerable<IProductLineVo> GetAllProductLinesVo(string sheetName)
        {
            //Create ProduktLine
            var products = new List<IProductLineVo>();

            // CellPosition
            ICell groupCell = null;
            ICell prodLineCell = null;

            //Property Group Name
            string propertyGroupName = string.Empty;

            // Prüfe ob ein DatenContext vorhanden ist
            if (_dataResult == null)
                return products;

            // Lese aus dem Stream die Tabele für "ProductLines" 
            var dataTable = _dataResult.Tables[sheetName];

            const string allowedProperty = @"[A-Z]+";
            var regex = new Regex(allowedProperty);

            //// Annahme: erste Zeile ist eine überschrift.
            //for (var i = 1; i < dataTable.Rows.Count; i++)
            //{
            //    var currentRow = dataTable.Rows[i];

            //    //Prüfe ob das Feld leer ist
            //    if(currentRow.ItemArray[0].ToString() == string.Empty)
            //        continue;

            //    // Handelt es ich um ein gültigen Parameter handelt
            //    if(regex.Match(currentRow.ItemArray[1].ToString()).Success)
            //     continue;

            //    var item = GetCurrentProductLineVo(currentRow);
            //    products.Add(item);
            //}

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                //Ermittle die Position  der Gruppen und Produkt Linien
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    // Ermittle die Gruppen Zeile
                    //todo contains noch ändern.
                    if ((dataTable.Rows[i].ItemArray[j].ToString().Contains("Tech") ||
                         dataTable.Rows[i].ItemArray[j].ToString().Contains("Phase")) && groupCell == null)
                    {
                        groupCell = new Cell()
                        {
                            X = j,
                            Y = i
                        };
                    }

                    // Ermittle die ProductLine Zeile 
                    if (dataTable.Rows[i].ItemArray[j].ToString().Contains("Line"))
                    {
                        prodLineCell = new Cell
                        {
                            X = j,
                            Y = i
                        };
                    }
                }
                if (groupCell != null && prodLineCell != null)
                    break;
            }

            // Lese die Daten aus 
            for (int j = prodLineCell.Y + 1; j < dataTable.Rows.Count; j++)
            {
                var productPropertys = new List<IProductLinePropertyVo>();


                if (!regex.Match(dataTable.Rows[j].ItemArray[prodLineCell.X].ToString()).Success)
                    continue;

                for (int k = 1; k < dataTable.Columns.Count; k++)
                {
                    if (dataTable.Rows[groupCell.X].ItemArray[k].ToString() != string.Empty)
                        propertyGroupName = dataTable.Rows[groupCell.X].ItemArray[k].ToString();

                    // unsauber
                    if (dataTable.Rows[prodLineCell.X + 3].ItemArray[k].ToString() == "")
                        continue;

                    var productLineProperty = new ProductLinePropertyVo()
                    {
                        Group = propertyGroupName,
                        Name = dataTable.Rows[prodLineCell.Y].ItemArray[prodLineCell.X + k].ToString(),
                        Value = !Convert.IsDBNull(dataTable.Rows[j].ItemArray[k])
                    };
                    productPropertys.Add(productLineProperty);
                }
                propertyGroupName = string.Empty;

                var item = new ProductLineVo()
                {
                    Name = dataTable.Rows[j].ItemArray[prodLineCell.X].ToString(),
                    ProductLineProperties = productPropertys
                };
                products.Add(item);
            }
            return products.ToArray();
        }


        /// <summary>
        /// Liest die Übersezungen aus der Excel datei aus und gibt sie als <see cref="ITranslationVo"/>s wieder
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public IEnumerable<ITranslationVo> GetTranslationVos(string sheetName)
        {
            var translations = new List<TranslationVo>();

            // Prüfe ob ein DatenContext vorhanden ist
            if (_dataResult == null)
                return translations;

            // Lese aus dem Stream die Tabele für "ProductLines" 
            var dataTable = _dataResult.Tables[sheetName];

            // regex
            //const string allowedLanguageId = @"[A-Z]+[a-z]+[ ]+[(][0-9]+[)]"; // todo: regex fehler -> Language (0) to --> Language 
            //var regex = new Regex(allowedLanguageId);

            // lese die translations aus
            for (int j = 1; j < dataTable.Columns.Count; j++)
            {
                //Debug.WriteLine( "Lang: " + dataTable.Rows[0].ItemArray[j]);

                var data = new TranslationVo()
                {
                    Id = j,
                    Language = dataTable.Rows[0].ItemArray[j].ToString(),
                    Dictionary = new Dictionary<string, string>()
                };

                // var language = dataTable.Rows[0].ItemArray[j].ToString();
                // Debug.WriteLine("Language: " + language); // ok
                for (int i = 1; i < dataTable.Rows.Count; i++)
                {
                    // geht Language Name
                    //var lang = dataTable.Rows[0].ItemArray[j].ToString();
                    //var match = regex.Match(lang).Success;
                    // splitstring

                    // Debug.WriteLine(lang + " : " + match);
                    // Debug.Write(" " + dataTable.Rows[i].ItemArray[j].ToString());

                    var key = dataTable.Rows[i].ItemArray[0].ToString();
                    var translation = dataTable.Rows[i].ItemArray[j].ToString();

                    // Debug.WriteLine("Key: " + key + " , translation: " + translation); // ok
                    // wenn keine Übersetzung vorhanden ist. ist key  gleich translation
                    data.Dictionary.Add(key, translation != string.Empty ? translation : key);
                }
                translations.Add(data);
            }

            
            return translations;
        }

        /// <summary>
        /// Gibt eine aufzählung von <see cref="ICommonLanguageVo"/>s wieder
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ICommonLanguageVo> GetCommonLanguageVos()
        {
            return _entities.Common_Language.AsNoTracking().ToArray();
        }

        /// <summary>
        /// Gibt eine aufzählung von <see cref="ICommonProductLineVo"/>s wieder
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ICommonProductLineVo> GetCommonProductLineVos()
        {
            return _entities.Common_ProductLine.AsNoTracking().ToArray();
        }

        /// <summary>
        ///  
        /// Liest Alle ModelNamen aus der DB aus gibt sie als Vos zruück
        /// </summary>
        /// <returns>Model Vos</returns>
        public IEnumerable<IModelVo> GetAllDbModelsVos()
        {
           throw new NotImplementedException();
        }


        /// <summary>
        /// Liest alle Realationen ProductLine - PropertyRelation aus der Datenbank FlowConfiguration
        /// </summary>
        /// <returns>GetCommonProductLinePropertyRelationVos</returns>
        public IEnumerable<ICommonProductLinePropertyRelationVo> GetCommonProductLinePropertyRelationVos(string conString)
        {
            var items = new List<ICommonProductLinePropertyRelationVo>();
            using (var connection = new SqlConnection(conString))
            {
                connection.Open();
                try
                {
                    using (
                        var command =
                            new SqlCommand(
                                "SELECT [productLineID],[productPropertyID] FROM [FlowConfiguration].[dbo].[Common_ProductLinePropertyRelation]",
                                connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new CommonProductLinePropertyRelationVo
                                {
                                    ProductLineId = Convert.ToInt32(reader.GetValue(0)),
                                    PropertyId = Convert.ToInt32(reader.GetValue(1))
                                };
                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Can not access the Database: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return items;
        }

        public IEnumerable<ICommonProductPropertyVo> GetCommonProductPropertyVos()
        {
            return _entities.Common_ProductProperty.AsNoTracking();
        }

        public IEnumerable<ICommonProductPropertyGroupVo> GetCommonProductPropertyGroupVos()
        {
            return _entities.Common_ProductPropertyGroup.AsNoTracking();
        }

        public IEnumerable<ICommonTranslateSizingWordVo> GetCommonTranslateSizingWordVos()
        {
            return _entities.Common_TranslateSizingWord.AsNoTracking();
        }

        public IEnumerable<ICommonTranslateSizingWordIdVo> GetCommonTranslateSizingWordIdVos()
        {
            return _entities.Common_TranslateSizingWordId.AsNoTracking();
        }
        

        /// <summary>
        ///  schreibt eine neue Sprache in db
        /// </summary>
        /// <param name="languages"></param>
        /// <param name="id"></param>
        public void AddLanguageToDb(string languages, int id)
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddLanguageToDb", "ENTER", DateTime.Now);
            //var splitLang = languages.Substring(0, 3);
            //var id = _entities.Common_Language.AsNoTracking().Select(j=>j).First(i => i.Name.ToLower().Contains(splitLang));
           
            if (!_entities.Common_Language.Any(i => i.LangId == id))
            {
                _entities.Common_Language.Add(new CommonLanguageVo
                {
                    LangId = id,
                    Name = languages
                });
            }
            _langId = id;
            
            //save
            SaveChanges();
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddLanguageToDb", "EXIT", DateTime.Now);
        }

        /// <summary>
        /// fügt eine neue productLine der db hinzu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void AddProductLine(int id, string name)
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductLine", "ENTER", DateTime.Now);
            if (_langId == -1)
                throw new Exception("Use Method AddLanguageToDb first");

            var qry = _entities.Common_ProductLine.AsNoTracking();

            var wordId = GetWordId(name);
            AddWord(_langId, wordId, name);

            if(qry.Any(j=>j.ProductLineId==id)) return;
            _entities.Common_ProductLine.Add(new CommonProductLineVo
            {
                ProductLineId = id,
                DisplaySequence = 0,
                ProductLineName = wordId
            });

            SaveChanges();
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductLine", "EXIT", DateTime.Now);
        }

        /// <summary>
        ///  fügt zur Datenbank eine neue property hinzu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="grpId"></param>
        /// <param name="grpName"></param>
        public void AddProductProperty(int id, string name, int grpId, string grpName)
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductProperty", "ENTER", DateTime.Now);
            if (_langId == -1)
                throw new Exception("Use Method AddLanguageToDb first");

            Debug.WriteLine("Id: {0}, name: {1}, grpid: {2}, grpName: {3}", id,name, grpId, grpName);
            // fügt eine Gruppe hinzu
            AddProductPropertyGroup(grpId, grpName);

            var qry = _entities.Common_ProductProperty.AsNoTracking();
            
            // fügt die productproperty hinzu
            if (qry.Any(j => j.ProductPropertyId == id)) return;
            var wordId = GetWordId(name);
            AddWord(_langId,wordId,name);
            _entities.Common_ProductProperty.Add(new CommonProductPropertyVo
            {
                ProductPropertyGroupId = grpId,
                ProductPropertyNameId = wordId, //GetWordId(name, _langId)
                ProductPropertyId = id,
                DisplaySequence = 0
            });


            SaveChanges();
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductProperty", "EXIT", DateTime.Now);
        }

        /// <summary>
        /// fügt die Realtion zw ProductLine und Product Property Hinzu
        /// </summary>
        /// <param name="productLineId"></param>
        /// <param name="productPropertyId"></param>
        /// <param name="connectionString"></param>
        public void AddProductLinePropertyRelation(int productLineId, int productPropertyId, string connectionString)
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductLinePropertyRelation", "ENTER", DateTime.Now);
            //if (_langId == -1)
            //    throw new Exception("Use Method AddLanguageToDb first");
           
            var vos =
                GetCommonProductLinePropertyRelationVos(connectionString);
            if (vos.Any(i => i.ProductLineId == productLineId && i.PropertyId == productPropertyId)) return;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    var command =
                        new SqlCommand(
                            "INSERT INTO [FlowConfiguration].[dbo].[Common_ProductLinePropertyRelation] (productLineID,productPropertyID) VALUES (" +
                            productLineId + "," + productPropertyId + ");",
                            connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Can not access the Database: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductLinePropertyRelation", "EXIT", DateTime.Now);
        }

        #region Private Methods

        /// <summary>
        /// Gibt die WordId des Aktuellen Wortes mit verbindung zur Sprache Wieder
        ///  --> wenn nicht vorhanden, wird neue WordId wiedergegeben.
        /// </summary>
        /// <param name="word">akteulles word</param>
        /// <param name="langId">sprachenId</param>
        /// <returns>wordId</returns>
        private int GetWordId(string word, int langId)
        {
            if (_entities.Common_TranslateSizingWord.AsNoTracking().Any(i => i.Word == word && i.LangId == langId))
            {
                return
                    _entities.Common_TranslateSizingWord.AsNoTracking()
                        .First(i => i.Word == word && i.LangId == langId)
                        .WordId;
            }

            var last =_entities.Common_TranslateSizingWordId.AsNoTracking().Last();
            return ++last.WordId;
        }


        /// <summary>
        /// Gibt die WordId des Aktuellen Wortes mit verbindung zur Sprache Wieder
        ///  --> wenn nicht vorhanden, wird neue WordId wiedergegeben.
        /// </summary>
        /// <param name="word">akteulles word</param>
        /// <returns>wordId</returns>
        private int GetWordId(string word)
        {
            var splitword = word;
            if(word.Length > 4)
                splitword = word.Substring(0, 4);
            if (_entities.Common_TranslateSizingWord.AsNoTracking().Any(i => i.Word == splitword))
            {
                return
                    _entities.Common_TranslateSizingWord.AsNoTracking()
                        .First(i => i.Word == word)
                        .WordId;
            }

            var last = _entities.Common_TranslateSizingWordId.AsNoTracking().ToList().LastOrDefault(); // slow
            return (last.WordId+1);
        }


        /// <summary>
        /// Fügt die PropertyGroup Hinzu
        /// </summary>
        /// <param name="grpId"></param>
        /// <param name="grpName"></param>
        private void AddProductPropertyGroup(int grpId, string grpName)
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductPropertyGroup", "ENTER", DateTime.Now);
            var qry = _entities.Common_ProductPropertyGroup.AsNoTracking();
            if(qry.Any(j=>j.ProductPropertyGroupId == grpId)) return;
            var wordId = GetWordId(grpName);
            AddWord(_langId,wordId,grpName);
            _entities.Common_ProductPropertyGroup.Add(new CommonProductPropertyGroupVo
            {
                ProductPropertyGroupId = grpId,
                ProductPropertyGroupNameId = wordId,
                DisplaySequence = 0,
            });

            SaveChanges();
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddProductPropertyGroup", "EXIT", DateTime.Now);
        }

        /// <summary>
        /// Fügt eine Neue WordId hinzu, wenn sie in der Tabelle nicht vorhanden ist
        /// </summary>
        /// <param name="wordId"></param>
        private void AddWordId(int wordId)
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddWordId", "ENTER", DateTime.Now);
            if (!_entities.Common_TranslateSizingWordId.AsNoTracking().Any(j=>j.WordId == wordId))
                _entities.Common_TranslateSizingWordId.Add(new CommonTranslateSizingWordIdVo
                {
                    WordId = wordId
                });

            SaveChanges();
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddWordId", "ENTER", DateTime.Now);
        }

        /// <summary>
        ///  Schreibt ein Word in die Db
        /// </summary>
        /// <param name="langId"></param>
        /// <param name="wordId"></param>
        /// <param name="word"></param>
        private void AddWord(int langId, int wordId, string word)
        {
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddWord", "ENTER", DateTime.Now);
            AddWordId(wordId);
            var qry = _entities.Common_TranslateSizingWord.AsNoTracking();
            Debug.WriteLine("AddWord: langId: {0}, wordId: {1}, word: {2}, can add: {3}", langId, wordId, word, !qry.Any(j => j.WordId == wordId && j.LangId == langId));
            if(!qry.Any(j=>j.WordId == wordId && j.LangId == langId))
            _entities.Common_TranslateSizingWord.Add(new CommonTranslateSizingWordVo
            {
                LangId = langId,
                WordId = wordId,
                Word = word,
            });

            SaveChanges();
            Debug.WriteLine("###### {0}: {1} ({2}) ", "AddWord", "EXIT", DateTime.Now);
        }
      
        private void SaveChanges()
        {
            try
            {
                _entities.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static bool ConvertDbNullToBool(object o)
        {
            return !Convert.IsDBNull(o);
        }

        private DataSet GetExcelDataSet(ExcelFileFormat format)
        {
            IExcelDataReader excelReader = null;

            // try to read all files from an OpenXml Excel file
            try
            {
                //Reading from an OpenXml Excel file
                var stream = new FileStream(_filename, FileMode.Open);
                // var stream = File.Open(_filename, FileMode.Open, FileAccess.Read);
                if (format == ExcelFileFormat.XLSX)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                if (format == ExcelFileFormat.XLS)
                    throw new NotSupportedException("XLS format not supported yet!");
                // excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

                //DataSet
                if (excelReader != null)
                    return excelReader.AsDataSet();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Couldn't read from file: " + _filename);
                Debug.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Close connection to EXCEL file..
                if (excelReader != null)
                {
                    excelReader.Close();
                    excelReader.Dispose();
                }
            }
            return null;
        }
        #endregion
    }
}                   